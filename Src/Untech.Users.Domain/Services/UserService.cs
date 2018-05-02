using System;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.DataStorage;
using Untech.Users.Domain.Models;
using Untech.Users.Domain.Requests;

namespace Untech.Users.Domain.Services
{
	public class UserService : ICommandHandler<CreateUser, User>
	{
		private readonly IDataStorage<User> _dataStorage;
		private readonly IQueryDispatcher _queryDispatcher;

		public UserService(IDataStorage<User> dataStorage, IQueryDispatcher queryDispatcher)
		{
			_dataStorage = dataStorage;
			_queryDispatcher = queryDispatcher;
		}

		public User Handle(CreateUser request)
		{
			if (request.TelegramId.HasValue)
			{
				var user = _queryDispatcher.Fetch(new FindUserByTelegramId(request.TelegramId.Value));

				if (user != null) throw new ArgumentException("TelegramId");
			}

			return _dataStorage.Create(new User
			{
				TelegramId = request.TelegramId
			});
		}
	}
}