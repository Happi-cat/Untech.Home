using System;
using System.Linq;
using Untech.DailySurvey.Domain.Requests;
using Untech.DailySurvey.Domain.Views;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;

namespace Untech.DailySurvey.Domain.Services
{
	public class SurveyResultService : IQueryHandler<SurveyResultQuery, SurveyResult>
	{
		private readonly IQueryDispatcher _queryDispatcher;

		public SurveyResultService(IQueryDispatcher queryDispatcher)
		{
			_queryDispatcher = queryDispatcher;
		}

		public SurveyResult Handle(SurveyResultQuery request)
		{
			var dateTo = DateTime.Today.AddDays(1);
			var dateFrom = dateTo.AddDays(-request.NumberOfLastDays);

			var answersGroupedByQuestions = _queryDispatcher
				.Fetch(new AnswersQuery(dateFrom, dateTo))
				.GroupBy(n => n.QuestionKey)
				.ToDictionary(n => n.Key);

			var allQuestions = _queryDispatcher.Fetch(new QuestionsQuery())
				.ToDictionary(n => n.Key);

			return new SurveyResult
			{
				Questions = answersGroupedByQuestions.Keys
					.Where(allQuestions.ContainsKey)
					.Select(questionKey => new SurveyResultQuestion(allQuestions[questionKey])
					{
						Answers = answersGroupedByQuestions[questionKey]
							.Select(answer => new SurveyResultAnswer
							{
								Answer = string.Join(", ", answer.SelectedOptions)
							})
							.ToList()
					})
					.ToList()
			};
		}
	}
}