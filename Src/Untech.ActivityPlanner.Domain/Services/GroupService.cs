using System.Linq;
using Untech.ActivityPlanner.Domain.Models;
using Untech.ActivityPlanner.Domain.Requests;
using Untech.ActivityPlanner.Domain.Requests.Activity;
using Untech.ActivityPlanner.Domain.Requests.Group;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.DataStorage;

namespace Untech.ActivityPlanner.Domain.Services
{
	public class GroupService : ICommandHandler<CreateGroup, Group>,
		ICommandHandler<UpdateGroup, Group>,
		ICommandHandler<DeleteGroup, bool>
	{
		private readonly IDataStorage<Group> _dataStorage;
		private readonly IQueryDispatcher _disaptcher;

		public GroupService(IDataStorage<Group> dataStorage, IQueryDispatcher disaptcher)
		{
			_dataStorage = dataStorage;
			_disaptcher = disaptcher;
		}

		public Group Handle(CreateGroup request)
		{
			return _dataStorage.Create(new Group(0, request.Name));
		}

		public Group Handle(UpdateGroup request)
		{
			var group = _dataStorage.Find(request.Key);

			group.Name = request.Name;

			return _dataStorage.Update(group);
		}

		public bool Handle(DeleteGroup request)
		{
			var cannotDelete = _disaptcher
				.Fetch(new ActivitiesQuery(request.Key))
				.Any();

			if (cannotDelete)
			{
				return false;
			}

			var group = _dataStorage.Find(request.Key);
			return _dataStorage.Delete(group);
		}
	}
}
