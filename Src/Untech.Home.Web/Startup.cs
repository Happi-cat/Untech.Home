using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using Untech.ActivityPlanner.Data;
using Untech.ActivityPlanner.Domain.Models;
using Untech.ActivityPlanner.Integration.GoogleCalendar;
using Untech.FinancePlanner.Data;
using Untech.FinancePlanner.Data.Cache;
using Untech.FinancePlanner.Domain.Models;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.CQRS.Pipeline;
using Untech.Practices.DataStorage;
using Untech.Practices.DataStorage.Cache;

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
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
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
			var dispatcher = new Dispatcher(new DispatcherContainer(container));

			container.RegisterSingleton<Func<FinancialPlannerContext>>(() => new FinancialPlannerContext());
			container.RegisterSingleton<Func<ActivityPlannerContext>>(() => new ActivityPlannerContext());
			container.RegisterSingleton(GetInitializer());

			container.Register<ICacheStorage, CacheStorage>();
			container.Register<IDataStorage<FinancialJournalEntry>, FinancialJournalEntryStorage>();
			container.Register<IDataStorage<Taxon>, TaxonStorage>();

			container.Register<IDataStorage<Group>, GenericDataStorage<Group>>();
			container.Register<IDataStorage<Activity>, ActivityDataStorage>();
			container.Register<IDataStorage<ActivityOccurrence>, GenericDataStorage<ActivityOccurrence>>();

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
	}
}
