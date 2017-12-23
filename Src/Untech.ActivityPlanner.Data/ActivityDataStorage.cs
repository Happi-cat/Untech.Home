using System;
using System.Linq;
using LinqToDB;
using Untech.ActivityPlanner.Domain.Models;

namespace Untech.ActivityPlanner.Data
{
	public class ActivityDataStorage : GenericDataStorage<Activity>
	{
		public ActivityDataStorage(Func<IDataContext> contextFactory) : base(contextFactory)
		{
		}

		public override bool Delete(Activity entity)
		{
			using (var context = GetContext())
			{
				context
					.GetTable<ActivityOccurrence>()
					.Where(n => n.ActivityKey == entity.Key)
					.Delete();
			}

			return base.Delete(entity);
		}
	}
}