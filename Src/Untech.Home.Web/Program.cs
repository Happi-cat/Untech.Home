using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

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
			FinancePlanner.Data.Initializations.DbInitializer.Initialize(() => new FinancePlanner.Data.FinancialPlannerContext(), @"..\..\Configs\");
			ActivityPlanner.Data.Initializations.DbInitializer.Initialize(() => new ActivityPlanner.Data.ActivityPlannerContext());
		}
	}
}
