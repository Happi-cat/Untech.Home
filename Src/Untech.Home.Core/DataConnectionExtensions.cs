using System;
using System.Data;
using LinqToDB;
using LinqToDB.Data;

namespace Untech.Home
{
	public static class DataConnectionExtensions
	{
		public static void EnsureTableExists<T>(this  DataConnection context)
		{
			if (IsTableExists<T>(context))
			{
				return;
			}

			context.CreateTable<T>();
		}

		private static bool IsTableExists<T>(DataConnection context)
		{
			var entityDescriptor = context.MappingSchema.GetEntityDescriptor(typeof(T));

			return IsTableExists(context, entityDescriptor.TableName);
		}

		private static bool IsTableExists(DataConnection context, string tableName)
		{
			tableName = tableName ?? throw new ArgumentNullException(nameof(tableName));

			var command = context.CreateCommand();
			command.CommandText = "SELECT 1 FROM sqlite_master WHERE type='table' AND name=@pName";
			command.CommandType = CommandType.Text;

			var pName = command.CreateParameter();
			pName.ParameterName = "@pName";
			pName.Value = tableName;

			command.Parameters.Add(pName);

			var result = (long?)command.ExecuteScalar() ?? 0;

			return result == 1;
		}
	}
}