using System;
using System.Data;
using LinqToDB;
using LinqToDB.Data;
using Untech.ActivityPlanner.Domain.Models;

namespace Untech.ActivityPlanner.Data.Initializations
{
	public static class DbInitializer
	{
		public static void Initialize(Func<ActivityPlannerContext> contextFactory)
		{
			using (var context = contextFactory())
			{
				EnsureTableExists<Group>(context);
				EnsureTableExists<Activity>(context);
				EnsureTableExists<ActivityOccurrence>(context);
			}
		}

		private static void EnsureTableExists<T>(DataConnection context)
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
