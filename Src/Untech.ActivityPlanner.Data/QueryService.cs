using System;
using System.Collections.Generic;
using System.Linq;
using LinqToDB;
using Untech.ActivityPlanner.Domain.Models;
using Untech.ActivityPlanner.Domain.Requests;
using Untech.Practices.CQRS.Handlers;

namespace Untech.ActivityPlanner.Data
{
	public class QueryService : IQueryHandler<GroupsQuery, IEnumerable<Group>>,
		IQueryHandler<ActivitiesQuery, IEnumerable<Activity>>,
		IQueryHandler<OccurrencesQuery, IEnumerable<ActivityOccurrence>>
	{
		private readonly Func<IDataContext> _contextFactory;

		public QueryService(Func<ActivityPlannerContext> contextFactory)
		{
			_contextFactory = contextFactory;
		}

		public IEnumerable<Group> Handle(GroupsQuery request)
		{
			using (var context = _contextFactory())
			{
				return context.GetTable<Group>().ToList();
			}
		}

		public IEnumerable<Activity> Handle(ActivitiesQuery request)
		{
			using (var context = _contextFactory())
			{
				return context
					.GetTable<Activity>()
					.Where(n => n.GroupKey == request.GroupKey)
					.ToList();
			}
		}

		public IEnumerable<ActivityOccurrence> Handle(OccurrencesQuery request)
		{
			using (var context = _contextFactory())
			{
				return context
					.GetTable<ActivityOccurrence>()
					.Where(n => request.From <= n.When && n.When < request.To)
					.ToList();
			}
		}
	}
}