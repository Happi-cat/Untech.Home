using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Untech.ActivityPlanner.Domain.Models;
using Untech.ActivityPlanner.Domain.Notifications;
using Untech.ActivityPlanner.Domain.Requests;
using Untech.Practices;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.DataStorage;

namespace Untech.ActivityPlanner.Domain.Services
{
	public class ActivityService : ICommandAsyncHandler<CreateActivity, Activity>,
		ICommandAsyncHandler<UpdateActivity, Activity>,
		ICommandAsyncHandler<DeleteActivity, bool>,
		ICommandAsyncHandler<ToggleActivityOccurrence, Nothing>,
		ICommandAsyncHandler<UpdateActivityOccurrence, Nothing>
	{
		private readonly IAsyncDataStorage<Group> _groupDataStorage;
		private readonly IAsyncDataStorage<Activity> _activityDataStorage;
		private readonly IAsyncDataStorage<ActivityOccurrence> _occurrencesDataStorage;

		private readonly IDispatcher _dispatcher;
		private readonly IQueueDispatcher _queueDispatcher;

		public ActivityService(
			IAsyncDataStorage<Group> groupDataStorage,
			IAsyncDataStorage<Activity> activityDataStorage,
			IAsyncDataStorage<ActivityOccurrence> occurrencesDataStorage,
			IDispatcher dispatcher,
			IQueueDispatcher queueDispatcher)
		{
			_groupDataStorage = groupDataStorage;
			_activityDataStorage = activityDataStorage;
			_occurrencesDataStorage = occurrencesDataStorage;

			_dispatcher = dispatcher;
			_queueDispatcher = queueDispatcher;
		}

		public async Task<Activity> HandleAsync(CreateActivity request, CancellationToken cancellationToken)
		{
			var group = await _groupDataStorage.FindAsync(request.GroupKey, cancellationToken);
			var activity = new Activity(0, group.Key, request.Name);

			return await _activityDataStorage.CreateAsync(activity, cancellationToken);
		}

		public async Task<Activity> HandleAsync(UpdateActivity request, CancellationToken cancellationToken)
		{
			var activity = await _activityDataStorage.FindAsync(request.Key, cancellationToken);

			activity.Name = request.Name;

			return await _activityDataStorage.UpdateAsync(activity, cancellationToken);
		}

		public async Task<bool> HandleAsync(DeleteActivity request, CancellationToken cancellationToken)
		{
			var occurrences = await _dispatcher
				.FetchAsync(new OccurrencesQuery(DateTime.Today, TimeSpan.FromDays(365)), cancellationToken);
			var cannotDelete =
				occurrences
				.Any(n => n.ActivityKey == request.Key);

			if (cannotDelete)
			{
				return false;
			}

			var activity = await _activityDataStorage.FindAsync(request.Key, cancellationToken);
			return await _activityDataStorage.DeleteAsync(activity, cancellationToken);
		}

		public async Task<Nothing> HandleAsync(ToggleActivityOccurrence request, CancellationToken cancellationToken)
		{
			var activity = await _activityDataStorage.FindAsync(request.ActivityKey, cancellationToken);

			var occurrences = await _dispatcher
				.FetchAsync(new OccurrencesQuery(request.When, TimeSpan.FromDays(1)), cancellationToken);
			var occurrence = occurrences
				.SingleOrDefault(n => n.ActivityKey == activity.Key && n.When == request.When.Date);

			if (occurrence == null)
			{
				occurrence = await _occurrencesDataStorage.CreateAsync(new ActivityOccurrence(0, activity.Key, request.When), cancellationToken);

				_queueDispatcher.Enqueue(new ActivityOccurrenceSaved(activity, occurrence));
			}
			else
			{
				await _occurrencesDataStorage.DeleteAsync(occurrence, cancellationToken);

				_queueDispatcher.Enqueue(new ActivityOccurrenceDeleted(activity, occurrence));
			}

			return Nothing.AtAll;
		}

		public async Task<Nothing> HandleAsync(UpdateActivityOccurrence request, CancellationToken cancellationToken)
		{
			var occurrence = await _occurrencesDataStorage.FindAsync(request.Key, cancellationToken);

			var activity = await _activityDataStorage.FindAsync(occurrence.ActivityKey, cancellationToken);

			occurrence.Note = request.Note;
			occurrence.Highlighted = request.Highlighted;
			occurrence.Missed = request.Missed;

			occurrence = await _occurrencesDataStorage.UpdateAsync(occurrence, cancellationToken);

			_queueDispatcher.Enqueue(new ActivityOccurrenceSaved(activity, occurrence));

			return Nothing.AtAll;
		}
	}
}
