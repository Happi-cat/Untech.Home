using System;
using Untech.ActivityPlanner.Domain.Models;
using Untech.Home.Data;

namespace Untech.ActivityPlanner.Data
{
	public class ActivityOccurrenceDataStorage : GenericDataStorage<ActivityOccurrence>
	{
		public ActivityOccurrenceDataStorage(Func<ActivityPlannerContext> contextFactory) : base(contextFactory)
		{

		}
	}
}