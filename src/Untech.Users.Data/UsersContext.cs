using LinqToDB.Data;
using LinqToDB.DataProvider.SQLite;
using Untech.Home;
using Untech.Home.Data;
using Untech.Users.Domain.Models;

namespace Untech.Users.Data
{
	public class UsersContext : DataConnection, IDbInitializer
	{
		public UsersContext(IConnectionStringFactory connectionStringFactory)
			: base(new SQLiteDataProvider(), connectionStringFactory.GetConnectionString("users.db"))
		{
			var b = MappingSchema.GetFluentMappingBuilder();

			b.Entity<User>().HasTableName("Users")
				.Property(n => n.Key).IsIdentity().IsPrimaryKey()
				.Property(n => n.TelegramId).IsNullable();
		}

		public void InitializeDb()
		{
			this.EnsureTableExists<User>();
		}
	}
}