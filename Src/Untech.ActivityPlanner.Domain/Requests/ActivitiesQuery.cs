using System.Collections.Generic;
using Untech.ActivityPlanner.Domain.Models;
using Untech.Practices.CQRS;

namespace Untech.ActivityPlanner.Domain.Requests
{
	public class ActivitiesQuery : IQuery<IEnumerable<Activity>>
	{
		public ActivitiesQuery(int groupKey)
		{
			GroupKey = groupKey;
		}

		public int GroupKey { get; }
	}
}