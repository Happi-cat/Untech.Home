using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using Untech.ActivityPlanner.Data;
using Untech.ActivityPlanner.Domain.Models;
using Untech.ActivityPlanner.Integration.GoogleCalendar;
using Untech.FinancePlanner.Data;
using Untech.FinancePlanner.Data.Cache;
using Untech.FinancePlanner.Domain.Models;
using Untech.Home.Data;
using Untech.Practices;
using Untech.Practices.CQRS;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.CQRS.Pipeline;
using Untech.Practices.DataStorage;
using Untech.Practices.DataStorage.Cache;
using Activity = Untech.ActivityPlanner.Domain.Models.Activity;

namespace Untech.Home.Web
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton(ConfigureDispatcher());
			services.AddMvc();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			Logging.ConfigureLogger(loggerFactory);
			Logging.LoggerFactory = loggerFactory;

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
				{
					HotModuleReplacement = true,
					ReactHotModuleReplacement = false
				});
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseStaticFiles();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");

				routes.MapSpaFallbackRoute(
					name: "spa-fallback",
					defaults: new { controller = "Home", action = "Index" });
			});
		}

		public IDispatcher ConfigureDispatcher()
		{
			var container = new Container();
			var dispatcher = new LoggableDispatcher(
				Logging.LoggerFactory.CreateLogger<IDispatcher>(),
				new Dispatcher(new DispatcherContainer(container)));
			var queueDispatcher = new SimpleQueueDispatcher(dispatcher);

			var connectionStringFactory = new SqliteConnectionStringFactory("./databases");

			container.RegisterSingleton<Func<FinancialPlannerContext>>(() => new FinancialPlannerContext(connectionStringFactory));
			container.RegisterSingleton<Func<ActivityPlannerContext>>(() => new ActivityPlannerContext(connectionStringFactory));
			container.RegisterSingleton(GetInitializer());

			container.Register<ICacheStorage, CacheStorage>();
			container.Register<IDataStorage<FinancialJournalEntry>, FinancialJournalEntryStorage>();
			container.Register<IDataStorage<Taxon>, TaxonStorage>();

			container.Register<IDataStorage<Group>, GroupDataStorage>();
			container.Register<IDataStorage<Activity>, ActivityDataStorage>();
			container.Register<IDataStorage<ActivityOccurrence>, ActivityOccurrenceDataStorage>();

			var assembly = new[]
			{
				typeof(FinancialPlannerContext).Assembly,
				typeof(FinancialJournalEntry).Assembly,

				typeof(ActivityPlannerContext).Assembly,
				typeof(Activity).Assembly,

				typeof(CalendarSynchronizer).Assembly
			};

			container.RegisterCollection(typeof(IPipelinePreProcessor<>), assembly);
			container.RegisterCollection(typeof(IPipelinePostProcessor<,>), assembly);
			container.Register(typeof(IQueryHandler<,>), assembly);
			container.Register(typeof(ICommandHandler<,>), assembly);
			container.RegisterCollection(typeof(INotificationHandler<>), assembly);


			container.RegisterSingleton<IDispatcher>(dispatcher);
			container.RegisterSingleton<IQueryDispatcher>(dispatcher);
			container.RegisterSingleton<IQueueDispatcher>(queueDispatcher);

			container.Verify();

			return dispatcher;
		}

		private static BaseClientService.Initializer GetInitializer()
		{
			UserCredential credential;

			using (var stream = File.OpenRead("google.client_secret.json"))
			{
				string credPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				credPath = Path.Combine(credPath, ".credentials/untech.home/calendar.json");

				credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
					GoogleClientSecrets.Load(stream).Secrets,
					new [] { CalendarService.Scope.Calendar },
					"user",
					CancellationToken.None,
					new FileDataStore(credPath, true)).Result;
				Console.WriteLine("Credential file saved to: " + credPath);
			}

			// Create Google Calendar API service.
			return new BaseClientService.Initializer
			{
				HttpClientInitializer = credential,
				ApplicationName = "Activity Planner",
			};
		}

		private class DispatcherContainer : ITypeResolver
		{
			private readonly Container _container;

			public DispatcherContainer(Container container)
			{
				_container = container;
			}

			public IEnumerable<T> ResolveMany<T>() where T : class
			{
				try
				{
					return _container.GetServices<T>();
				}
				catch
				{
					return new T[] { };
				}
			}

			public T ResolveOne<T>() where T : class => _container.GetService<T>();
		}

		private class LoggableDispatcher : IDispatcher
		{
			private readonly ILogger _logger;
			private readonly IDispatcher _dispatcher;

			public LoggableDispatcher(ILogger logger, IDispatcher dispatcher)
			{
				_logger = logger;
				_dispatcher = dispatcher;
			}

			public TResult Fetch<TResult>(IQuery<TResult> query)
			{
				return Execute("Fetch", query.GetType(), () => _dispatcher.Fetch(query));
			}

			public Task<TResult> FetchAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken)
			{
				return Execute("FetchAsync", query.GetType(), () => _dispatcher.FetchAsync(query, cancellationToken));
			}

			public TResult Process<TResult>(ICommand<TResult> command)
			{
				return Execute("Process", command.GetType(), () => _dispatcher.Process(command));
			}

			public Task<TResult> ProcessAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken)
			{
				return Execute("ProcessAsync", command.GetType(), () => _dispatcher.ProcessAsync(command, cancellationToken));
			}

			public void Publish(INotification notification)
			{
				Execute("Publish", notification.GetType(), () =>
				{
					_dispatcher.Publish(notification);
					return Nothing.AtAll;
				});
			}

			public Task PublishAsync(INotification notification, CancellationToken cancellationToken)
			{
				return Execute("PublishAsync", notification.GetType(), () => _dispatcher.PublishAsync(notification, cancellationToken));
			}

			private T Execute<T>(string methodName, Type request, Func<T> action)
			{
				try
				{
					var sw = new Stopwatch();
					sw.Start();
					var result = action();
					sw.Stop();

					_logger.LogInformation("{0}({1}) finished in {2}ms", methodName, request, sw.ElapsedMilliseconds);

					return result;
				}
				catch (Exception e)
				{
					_logger.LogError(e, "{0}({1}) failed", methodName, request);
					throw;
				}
			}
		}
	}
}
