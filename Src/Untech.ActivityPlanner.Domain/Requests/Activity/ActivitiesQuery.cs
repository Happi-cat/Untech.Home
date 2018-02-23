using System.Collections.Generic;
using Untech.Practices.CQRS;

namespace Untech.ActivityPlanner.Domain.Requests.Activity
{
	public class ActivitiesQuery : IQuery<IEnumerable<Models.Activity>>
	{
		public ActivitiesQuery(int groupKey)
		{
			GroupKey = groupKey;
		}

		public int GroupKey { get; }
	}
}