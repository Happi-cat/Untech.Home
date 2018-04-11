using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Untech.ActivityPlanner.Domain.Models;
using Untech.ActivityPlanner.Domain.Requests;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.DataStorage;

namespace Untech.ActivityPlanner.Domain.Services
{
	public class GroupService : ICommandAsyncHandler<CreateGroup, Group>,
		ICommandAsyncHandler<UpdateGroup, Group>,
		ICommandAsyncHandler<DeleteGroup, bool>
	{
		private readonly IAsyncDataStorage<Group> _dataStorage;
		private readonly IQueryDispatcher _disaptcher;

		public GroupService(IAsyncDataStorage<Group> dataStorage, IQueryDispatcher disaptcher)
		{
			_dataStorage = dataStorage;
			_disaptcher = disaptcher;
		}

		public Task<Group> HandleAsync(CreateGroup request, CancellationToken cancellationToken)
		{
			return _dataStorage.CreateAsync(new Group(0, request.Name), cancellationToken);
		}

		public async Task<Group> HandleAsync(UpdateGroup request, CancellationToken cancellationToken)
		{
			var group = await _dataStorage.FindAsync(request.Key, cancellationToken);

			group.Name = request.Name;

			return await _dataStorage.UpdateAsync(group, cancellationToken);
		}

		public async Task<bool> HandleAsync(DeleteGroup request, CancellationToken cancellationToken)
		{
			var activities = await _disaptcher
				.FetchAsync(new ActivitiesQuery(request.Key), cancellationToken);
			var cannotDelete = activities
				.Any();

			if (cannotDelete)
			{
				return false;
			}

			var group = await _dataStorage.FindAsync(request.Key, cancellationToken);
			return await _dataStorage.DeleteAsync(group, cancellationToken);
		}
	}
}
