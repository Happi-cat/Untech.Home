using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LinqToDB;
using Untech.ActivityPlanner.Domain.Models;
using Untech.ActivityPlanner.Domain.Requests;
using Untech.Practices.CQRS.Handlers;

namespace Untech.ActivityPlanner.Data
{
	public class QueryService : IQueryAsyncHandler<GroupsQuery, IEnumerable<Group>>,
		IQueryAsyncHandler<ActivitiesQuery, IEnumerable<Activity>>,
		IQueryAsyncHandler<OccurrencesQuery, IEnumerable<ActivityOccurrence>>
	{
		private readonly Func<IDataContext> _contextFactory;

		public QueryService(Func<ActivityPlannerContext> contextFactory)
		{
			_contextFactory = contextFactory;
		}

		public async Task<IEnumerable<Group>> HandleAsync(GroupsQuery request, CancellationToken cancellationToken)
		{
			using (var context = _contextFactory())
			{
				return await context.GetTable<Group>()
					.ToListAsync(cancellationToken);
			}
		}

		public async Task<IEnumerable<Activity>> HandleAsync(ActivitiesQuery request, CancellationToken cancellationToken)
		{
			using (var context = _contextFactory())
			{
				return await context
					.GetTable<Activity>()
					.Where(n => n.GroupKey == request.GroupKey)
					.ToListAsync(cancellationToken);
			}
		}

		public async Task<IEnumerable<ActivityOccurrence>> HandleAsync(OccurrencesQuery request, CancellationToken cancellationToken)
		{
			using (var context = _contextFactory())
			{
				return await context
					.GetTable<ActivityOccurrence>()
					.Where(n => request.From <= n.When && n.When < request.To)
					.ToListAsync(cancellationToken);
			}
		}
	}
}