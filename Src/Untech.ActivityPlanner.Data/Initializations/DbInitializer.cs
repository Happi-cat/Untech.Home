using System;
using Untech.ActivityPlanner.Domain.Models;
using Untech.Home;

namespace Untech.ActivityPlanner.Data.Initializations
{
	public static class DbInitializer
	{
		public static void Initialize(Func<ActivityPlannerContext> contextFactory)
		{
			using (var context = contextFactory())
			{
				context.EnsureTableExists<Group>();
				context.EnsureTableExists<Activity>();
				context.EnsureTableExists<ActivityOccurrence>();
			}
		}
	}
}
