using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.SQLite;
using Untech.ActivityPlanner.Domain.Models;

namespace Untech.ActivityPlanner.Data
{
	public class ActivityPlannerContext : DataConnection
	{
		public ActivityPlannerContext()
			: base(new SQLiteDataProvider(), "Data Source=activity_planner.db")
		{
			var b = MappingSchema.GetFluentMappingBuilder();

			b.Entity<Group>().HasTableName("Groups")
				.Property(_ => _.Key).IsIdentity().IsPrimaryKey()
				.Property(_ => _.Name);

			b.Entity<Activity>().HasTableName("Activities")
				.Property(_ => _.Key).IsIdentity().IsPrimaryKey()
				.Property(_ => _.GroupKey)
				.Property(_ => _.Name);

			b.Entity<ActivityOccurrence>().HasTableName("ActivityOccurrences")
				.Property(_ => _.Key).IsIdentity().IsPrimaryKey()
				.Property(_ => _.ActivityKey)
				.Property(_ => _.Note).IsNullable()
				.Property(_ => _.Highlighted)
				.Property(_ => _.Missed)
				.Property(_ => _.When);
		}

		public ITable<Group> Groups => GetTable<Group>();

		public ITable<Activity> Activities => GetTable<Activity>();

		public ITable<ActivityOccurrence> ActivityOccurrences => GetTable<ActivityOccurrence>();
	}
}