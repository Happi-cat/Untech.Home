using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.SQLite;

namespace Untech.DailySurvey.Data
{
	public class DailySurveyContext : DataConnection
	{
		public DailySurveyContext()
			: base(new SQLiteDataProvider(), "Data Source=daily_survey.db")
		{
		}

		public ITable<QuestionDao> Questions => GetTable<QuestionDao>();

		public ITable<AnswerDao> Answers => GetTable<AnswerDao>();
	}
}