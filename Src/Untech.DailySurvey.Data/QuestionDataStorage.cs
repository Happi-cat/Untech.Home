using System;
using System.Collections.Generic;
using System.Linq;
using Untech.DailySurvey.Domain.Models;
using Untech.DailySurvey.Domain.Requests;
using Untech.Home.Data;
using Untech.Practices.CQRS.Handlers;

namespace Untech.DailySurvey.Data
{
	public class QuestionDataStorage : GenericDataStorage<Question, QuestionDao>,
		IQueryHandler<QuestionsQuery, IEnumerable<Question>>,
		IQueryHandler<DailyQuestionsQuery, IEnumerable<Question>>
	{
		public QuestionDataStorage(Func<DailySurveyContext> contextFactory)
			: base(contextFactory, DaoMapper.Instance, DaoMapper.Instance)
		{
		}

		public IEnumerable<Question> Handle(QuestionsQuery request)
		{
			using (var context = GetContext())
			{
				return GetTable(context)
					.Where(n => request.IncludeObsolete || !request.IncludeObsolete && !n.IsObsolete)
					.AsEnumerable()
					.Select(ToEntity)
					.ToList();
			}
		}

		public IEnumerable<Question> Handle(DailyQuestionsQuery request)
		{
			var periodicityToFetch = GetPeriodicities().ToList();

			using (var context = GetContext())
			{
				return GetTable(context)
					.Where(n=>!n.IsObsolete && periodicityToFetch.Contains(n.Periodicity))
					.AsEnumerable()
					.Select(ToEntity)
					.ToList();
			}

			IEnumerable<QuestionPeriodicity> GetPeriodicities()
			{
				yield return QuestionPeriodicity.Daily;

				var today = DateTime.Today;
				if (today.DayOfWeek == DayOfWeek.Sunday)
				{
					yield return QuestionPeriodicity.Weekly;
				}

				if (DateTime.DaysInMonth(today.Year, today.Month) == today.Day)
				{
					yield return QuestionPeriodicity.Monthly;
				}
			}
		}
	}
}