using System.Data.Common;
using System.IO;

namespace Untech.Home.Data
{
	public class SqliteConnectionStringFactory
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
				{ "Pooling", "True" },
				{ "Max Pool Size", 100 }
			};

			return builder.ConnectionString;
		}
	}
}