using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Untech.Home.Data;

namespace Untech.Home.Web
{
	public class Program
	{
		public static void Main(string[] args)
		{
			EnsureDatabaseCreated();
			BuildWebHost(args).Run();
		}

		public static IWebHost BuildWebHost(string[] args)
		{
			var config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("hosting.json", optional: true)
				.AddCommandLine(args)
				.Build();

			return WebHost.CreateDefaultBuilder(args)
				.UseConfiguration(config)
				.UseStartup<Startup>()
				.Build();
		}

		public static void EnsureDatabaseCreated()
		{
			var connectionStringFactory = new SqliteConnectionStringFactory("./databases");
			FinancePlanner.Data.Initializations.DbInitializer.Initialize(() => new FinancePlanner.Data.FinancialPlannerContext(connectionStringFactory), @"..\..\Configs\");
			ActivityPlanner.Data.Initializations.DbInitializer.Initialize(() => new ActivityPlanner.Data.ActivityPlannerContext(connectionStringFactory));
		}
	}
}
