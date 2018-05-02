using LinqToDB.Data;
using LinqToDB.DataProvider.SQLite;
using Untech.Home.Data;
using Untech.Users.Domain;

namespace Untech.Users.Data
{
	public class UsersContext : DataConnection
	{
		public UsersContext(IConnectionStringFactory connectionStringFactory)
			: base(new SQLiteDataProvider(), connectionStringFactory.GetConnectionString("users.db"))
		{
			var b = MappingSchema.GetFluentMappingBuilder();

			b.Entity<User>().HasTableName("Users")
				.Property(n => n.Key).IsPrimaryKey()
				.Property(n => n.TelegramId).IsNullable();
		}
	}
}