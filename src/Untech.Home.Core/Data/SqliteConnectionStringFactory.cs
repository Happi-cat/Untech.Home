using System.Data.Common;
using System.IO;

namespace Untech.Home.Data
{
	public class SqliteConnectionStringFactory : IConnectionStringFactory
	{
		private readonly string _databaseFolder;

		public SqliteConnectionStringFactory(string databaseFolder)
		{
			_databaseFolder = databaseFolder;
		}

		public string GetConnectionString(string fileName)
		{
			var builder = new DbConnectionStringBuilder
			{
				{ "Data Source", Path.Combine(_databaseFolder, fileName) },
			};

			return builder.ConnectionString;
		}
	}
}