using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.SQLite;
using Untech.Home.Data;
using Untech.ReadingList.Domain.Models;

namespace Untech.ReadingList.Data
{
	public class ReadingListContext : DataConnection
	{
		public ReadingListContext(IConnectionStringFactory connectionStringFactory)
			: base(new SQLiteDataProvider(), connectionStringFactory.GetConnectionString("reading_list.db"))
		{
			var b = MappingSchema.GetFluentMappingBuilder();

			b.Entity<ReadingListEntry>().HasTableName("ReadingListEntries")
				.Property(n => n.Key).IsIdentity().IsPrimaryKey()
				.Property(n => n.Author)
				.Property(n => n.Title)
				.Property(n => n.Status)
				.Property(n => n.ReadingStarted).IsNullable()
				.Property(n => n.ReadingCompleted).IsNullable()
				.Property(n => n.DaysReading).IsNotColumn();
		}

		public ITable<ReadingListEntry> ReadingListEntries => GetTable<ReadingListEntry>();
	}
}