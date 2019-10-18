using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.SQLite;
using Untech.Home;
using Untech.Home.Data;

namespace Untech.DailySurvey.Data
{
	public class DailySurveyContext : DataConnection, IDbInitializer
	{
		public DailySurveyContext(IConnectionStringFactory connectionStringFactory)
			: base(new SQLiteDataProvider(), connectionStringFactory.GetConnectionString("daily_survey.db"))
		{
		}

		public ITable<QuestionDao> Questions => GetTable<QuestionDao>();

		public ITable<AnswerDao> Answers => GetTable<AnswerDao>();

		public void InitializeDb()
		{
			this.EnsureTableExists<QuestionDao>();
			this.EnsureTableExists<AnswerDao>();
		}
	}
}