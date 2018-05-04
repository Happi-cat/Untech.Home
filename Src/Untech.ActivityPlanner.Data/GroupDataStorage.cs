using System;
using Untech.ActivityPlanner.Domain.Models;
using Untech.Home.Data;

namespace Untech.ActivityPlanner.Data
{
	public class GroupDataStorage : GenericDataStorage<Group>
	{
		public GroupDataStorage(Func<ActivityPlannerContext> contextFactory) : base(contextFactory)
		{
			
		}
	}
}