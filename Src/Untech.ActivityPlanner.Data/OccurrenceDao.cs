using System;
using LinqToDB.Mapping;

namespace Untech.ActivityPlanner.Data
{
	[Table("Occurrences")]
	public class OccurrenceDao
	{
		[Column, PrimaryKey, Identity]
		public int Key { get; set; }

		[Column]
		public int ActivityKey { get; set; }

		[Column]
		public DateTime When { get; set; }

		[Column]
		public string Note { get; set; }

		[Column]
		public bool Highlighted { get; set; }
	}
}