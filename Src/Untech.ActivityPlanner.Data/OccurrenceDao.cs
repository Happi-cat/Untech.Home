using System;
using LinqToDB.Mapping;

namespace Untech.ActivityPlanner.Data
{
	[Table("Occurrences")]
	public class OccurrenceDao
	{
		[Column]
		public int ActivityKey { get; set; }
		
		[Column]
		public DateTime When { get; set; }
		
		
	}
}