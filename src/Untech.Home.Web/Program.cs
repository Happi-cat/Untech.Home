using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Configuration;
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

			EnsureDatabaseCreated(config);

			return WebHost.CreateDefaultBuilder(args)
				.UseConfiguration(config)
				.UseStartup<Startup>()
				.Build();
		}

		public static void EnsureDatabaseCreated(IConfiguration configuration)
		{
			var connectionStringFactory = new SqliteConnectionStringFactory(configuration["Databases:Folder"]);
			FinancePlanner.Data.Initializations.DbInitializer
				.Initialize(() => new FinancePlanner.Data.FinancialPlannerContext(connectionStringFactory), @"..\..\Configs\");
			ActivityPlanner.Data.Initializations.DbInitializer
				.Initialize(() => new ActivityPlanner.Data.ActivityPlannerContext(connectionStringFactory));
		}
	}
}