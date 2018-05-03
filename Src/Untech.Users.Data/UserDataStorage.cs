using System;
using System.Linq;
using Untech.Home.Data;
using Untech.Practices.CQRS.Handlers;
using Untech.Users.Domain;
using Untech.Users.Domain.Models;
using Untech.Users.Domain.Requests;

namespace Untech.Users.Data
{
	public class UserDataStorage : GenericDataStorage<User>,
		IQueryHandler<FindUserByTelegramId, User>
	{
		public UserDataStorage(Func<UsersContext> contextFactory) : base(contextFactory)
		{
		}

		public User Handle(FindUserByTelegramId request)
		{
			using (var context = GetContext())
			{
				return GetTable(context)
					.SingleOrDefault(n => n.TelegramId == request.TelegramId);
			}
		}
	}
}