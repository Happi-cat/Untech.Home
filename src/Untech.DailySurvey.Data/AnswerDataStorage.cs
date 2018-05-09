using System;
using System.Collections.Generic;
using System.Linq;
using Untech.DailySurvey.Domain.Models;
using Untech.DailySurvey.Domain.Requests;
using Untech.Home.Data;
using Untech.Practices.CQRS.Handlers;

namespace Untech.DailySurvey.Data
{
	public class AnswerDataStorage : GenericDataStorage<Answer, AnswerDao>,
		IQueryHandler<AnswersQuery, IEnumerable<Answer>>
	{
		public AnswerDataStorage(Func<DailySurveyContext> contextFactory)
			: base(contextFactory, DaoMapper.Instance, DaoMapper.Instance)
		{
		}

		public IEnumerable<Answer> Handle(AnswersQuery request)
		{
			using (var context = GetContext())
			{
				return GetTable(context)
					.Where(n => request.From < n.When && n.When < request.To)
					.AsEnumerable()
					.Select(ToEntity)
					.ToList();
			}
		}
	}
}