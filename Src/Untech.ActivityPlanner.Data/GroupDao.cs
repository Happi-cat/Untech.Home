using LinqToDB.Mapping;

namespace Untech.ActivityPlanner.Data
{
	[Table("Groups")]
	public class GroupDao
	{
		[Column, PrimaryKey, Identity]
		public int Key { get; set; }

		[Column, NotNull]
		public string Name { get; set; }
	}
}