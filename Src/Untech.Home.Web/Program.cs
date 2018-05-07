using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Untech.ActivityPlanner.Data;
using Untech.FinancePlanner.Data;
using Untech.Home.Data;

namespace Untech.Home.Web
{
	public class Program
	{
		public static void Main(string[] args)
		{
			BuildWebHost(args).Run();
		}

		public static IWebHost BuildWebHost(string[] args)
		{
			var config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("hosting.json", optional: true)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddCommandLine(args)
				.Build();

			EnsureDatabasesCreated(config);

			return WebHost.CreateDefaultBuilder(args)
				.UseConfiguration(config)
				.UseStartup<Startup>()
				.Build();
		}

		public static void EnsureDatabasesCreated(IConfiguration configuration)
		{
			var connectionStringFactory = new SqliteConnectionStringFactory(configuration["Databases:Folder"]);

			foreach (var initializer in GetInitializers())
			{
				initializer.InitializeDb();
			}

			FinancePlanner.Data.Initializations.DbInitializer
				.Initialize(() => new FinancialPlannerContext(connectionStringFactory), @"..\..\Configs\");

			IEnumerable<IDbInitializer> GetInitializers()
			{
				yield return new FinancialPlannerContext(connectionStringFactory);
				yield return new ActivityPlannerContext(connectionStringFactory);
			}
		}
	}
}
