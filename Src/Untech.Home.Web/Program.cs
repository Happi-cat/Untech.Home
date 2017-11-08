using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

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
			try
			{
				// using (var context = new FinancialPlannerContext())
				// {
				// 	//if (context.Database.EnsureCreated())
				// 	//{
				// 	//	TaxonInitializer.Initialize(context, @"..\..\Configs\");
				// 	//}
				// }
			}
			catch
			{

			}
		}
	}
}
