using System;
using System.Collections.Generic;
using Untech.ActivityPlanner.Domain.Models;
using Untech.ActivityPlanner.Domain.Views;
using Untech.Practices.CQRS;

namespace Untech.ActivityPlanner.Domain.Requests
{
	public class ActivitiesViewQuery : IQuery<ActivitiesView>
	{

	}

	public class GroupsQuery : IQuery<IEnumerable<Group>>
	{

	}

	public class ActivitiesQuery : IQuery<IEnumerable<Activity>>
	{
		public ActivitiesQuery(int groupKey)
		{
			GroupKey = groupKey;
		}

		public int GroupKey { get; }
	}

	public class OccurrencesQuery : IQuery<IEnumerable<ActivityOccurrence>>
	{
		public OccurrencesQuery(DateTime from, DateTime to)
		{
			From = from;
			To = to;
		}

		public DateTime From { get; private set; }

		public DateTime To { get; private set; }
	}
}