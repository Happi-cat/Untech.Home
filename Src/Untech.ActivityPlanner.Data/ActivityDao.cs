using LinqToDB.Mapping;

namespace Untech.ActivityPlanner.Data
{
	[Table("Activities")]
	public class ActivityDao
	{
		[Column, PrimaryKey, Identity]
		public int Key { get; set; }

		[Column]
		public string Name { get; set; }

	}
}