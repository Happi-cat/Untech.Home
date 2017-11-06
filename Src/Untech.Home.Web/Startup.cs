using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SimpleInjector;
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
					ReactHotModuleReplacement = true
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

			Func<DbContext> contextFactory = () => new FinancialPlannerContext();

			container.RegisterSingleton<ICacheStorage>(new CacheStorage(contextFactory));
			container.RegisterSingleton<IDataStorage<FinancialJournalEntry>>(new GenericStorage<FinancialJournalEntry>(contextFactory));
			container.RegisterSingleton<IDataStorage<Taxon>>(new GenericStorage<Taxon>(contextFactory));

			var assembly = new[] { typeof(FinancialJournalEntry).Assembly };

			container.RegisterCollection(typeof(IPipelinePreProcessor<>), assembly);
			container.RegisterCollection(typeof(IPipelinePostProcessor<,>), assembly);
			container.Register(typeof(IQueryHandler<,>), assembly);
			container.Register(typeof(ICommandHandler<,>), assembly);
			container.RegisterCollection(typeof(INotificationHandler<>), assembly);

			var dispatcher = new Dispatcher(new DispatcherContainer(container));
			container.RegisterSingleton<IDispatcher>(dispatcher);

			container.Verify();

			return dispatcher;
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
