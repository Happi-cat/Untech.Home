using LinqToDB.Data;
using LinqToDB.DataProvider.SQLite;

namespace Untech.FinancePlanner.Data
{
	public class FinancialPlannerContext : DataConnection
	{
		public FinancialPlannerContext()
			: base(new SQLiteDataProvider(), "Data Source=financial_planner.db")
		{
		}
	}
}
