using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.SQLite;
using Untech.ActivityPlanner.Domain.Models;
using Untech.Home.Data;

namespace Untech.ActivityPlanner.Data
{
	public class ActivityPlannerContext : DataConnection
	{
		public ActivityPlannerContext(SqliteConnectionStringFactory connectionStringFactory)
			: base(new SQLiteDataProvider(), connectionStringFactory.GetConnectionString("activity_planner.db"))
		{
			var b = MappingSchema.GetFluentMappingBuilder();

			b.Entity<Group>().HasTableName("Groups")
				.Property(n => n.Key).IsIdentity().IsPrimaryKey()
				.Property(n => n.Name);

			b.Entity<Activity>().HasTableName("Activities")
				.Property(n => n.Key).IsIdentity().IsPrimaryKey()
				.Property(n => n.GroupKey)
				.Property(n => n.Name);

			b.Entity<ActivityOccurrence>().HasTableName("ActivityOccurrences")
				.Property(n => n.Key).IsIdentity().IsPrimaryKey()
				.Property(n => n.ActivityKey)
				.Property(n => n.Note).IsNullable()
				.Property(n => n.Highlighted)
				.Property(n => n.Missed)
				.Property(n => n.When)
				.Property(n => n.Ongoing).IsNotColumn();
		}

		public ITable<Group> Groups => GetTable<Group>();

		public ITable<Activity> Activities => GetTable<Activity>();

		public ITable<ActivityOccurrence> ActivityOccurrences => GetTable<ActivityOccurrence>();
	}
}