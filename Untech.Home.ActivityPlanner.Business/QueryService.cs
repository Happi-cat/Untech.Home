using System.Collections.Generic;
using System.Linq;
using Untech.Home.ActivityPlanner.Domain.Models;
using Untech.Home.ActivityPlanner.Domain.Queries;
using Untech.Home.ActivityPlanner.Domain.Views;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.Repos.Queryable;

namespace Untech.Home.ActivityPlanner.Business
{
	public class QueryService :
		IQueryHandler<ActivityCalendarQuery, IEnumerable<Activity>>,
		IQueryHandler<ActivityGroupsCalendarQuery, IEnumerable<ActivityGroupCalendar>>
	{
		private readonly IReadOnlyRepository<ActivityGroup> _activityGroups;
		private readonly IReadOnlyRepository<Activity> _activities;

		public QueryService(IReadOnlyRepository<Activity> activities, IReadOnlyRepository<ActivityGroup> activityGroups)
		{
			_activityGroups = activityGroups;
			_activities = activities;
		}

		public IEnumerable<ActivityGroupCalendar> Handle(ActivityGroupsCalendarQuery request)
		{
			foreach (var group in _activityGroups.GetAll())
			{
				var activities = _activities.GetAll()
					.Where(n => n.GroupId == group.Id && n.When >= request.From && n.When <= request.To)
					.ToList();

				yield return new ActivityGroupCalendar
				{
					Group = group,
					Occurrencies = activities
				};
			}
		}

		public IEnumerable<Activity> Handle(ActivityCalendarQuery request)
		{
			return _activities.GetAll()
				.Where(n => n.When >= request.From && n.When <= request.To)
				.ToList();
		}
	}
}
