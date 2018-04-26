using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.SQLite;
using Untech.Home.Data;

namespace Untech.DailySurvey.Data
{
	public class DailySurveyContext : DataConnection
	{
		public DailySurveyContext(SqliteConnectionStringFactory connectionStringFactory)
			: base(new SQLiteDataProvider(), connectionStringFactory.GetConnectionString("daily_survey.db"))
		{
		}

		public ITable<QuestionDao> Questions => GetTable<QuestionDao>();

		public ITable<AnswerDao> Answers => GetTable<AnswerDao>();
	}
}