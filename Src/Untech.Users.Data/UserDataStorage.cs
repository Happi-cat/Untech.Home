using System;
using Untech.Home.Data;
using Untech.Users.Domain;

namespace Untech.Users.Data
{
	public class UserDataStorage : GenericDataStorage<User>
	{
		public UserDataStorage(Func<UsersContext> contextFactory) : base(contextFactory)
		{
		}
	}
}