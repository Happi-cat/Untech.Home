using LinqToDB.Mapping;

namespace Untech.FinancePlanner.Data.Cache
{
	[Table("CacheEntries")]
	public class CacheEntry
	{
		[Column, PrimaryKey]
		public string Key { get; set; }

		[Column]
		public string Json { get; set; }
	}
}