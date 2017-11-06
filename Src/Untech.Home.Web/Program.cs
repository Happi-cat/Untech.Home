using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Untech.FinancePlanner.Data;
using Untech.FinancePlanner.Data.Initializations;

namespace Untech.Home.Web
{
	public class Program
	{
		public static void Main(string[] args)
		{
			EnsureDatabaseCreated();
			BuildWebHost(args).Run();
		}

		public static IWebHost BuildWebHost(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>()
				.Build();

		public static void EnsureDatabaseCreated()
		{
			using (var context = new FinancialPlannerContext())
			{
				if (context.Database.EnsureCreated())
				{
					TaxonInitializer.Initialize(context, @"..\..\Configs\");
				}
			}
		}
	}
}
