using Untech.DailySurvey.Domain.Models;
using Untech.Practices.ObjectMapping;

namespace Untech.DailySurvey.Data
{
	public class DaoMapper : IMap<QuestionDao, Question>,
		IMap<Question, QuestionDao>,
		IMap<AnswerDao, Answer>,
		IMap<Answer, AnswerDao>
	{
		public static readonly DaoMapper Instance = new DaoMapper();

		private DaoMapper()
		{
		}

		public Question Map(QuestionDao input)
		{
			return QuestionDao.ToEntity(input);
		}

		public QuestionDao Map(Question input)
		{
			return new QuestionDao(input);
		}

		public Answer Map(AnswerDao input)
		{
			return AnswerDao.ToEntity(input);
		}

		public AnswerDao Map(Answer input)
		{
			return new AnswerDao(input);
		}
	}
}