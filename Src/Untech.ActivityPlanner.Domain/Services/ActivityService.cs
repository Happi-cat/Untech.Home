using System;
using System.Linq;
using Untech.ActivityPlanner.Domain.Models;
using Untech.ActivityPlanner.Domain.Notifications;
using Untech.ActivityPlanner.Domain.Requests;
using Untech.Practices;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.DataStorage;

namespace Untech.ActivityPlanner.Domain.Services
{
	public class ActivityService : ICommandHandler<CreateActivity, Activity>,
		ICommandHandler<UpdateActivity, Activity>,
		ICommandHandler<DeleteActivity, bool>,
		ICommandHandler<ToggleActivityOccurrence, Nothing>,
		ICommandHandler<UpdateActivityOccurrence, Nothing>
	{
		private readonly IDataStorage<Group> _groupDataStorage;
		private readonly IDataStorage<Activity> _activityDataStorage;
		private readonly IDataStorage<ActivityOccurrence> _occurrencesDataStorage;

		private readonly IDispatcher _dispatcher;
		private readonly IQueueDispatcher _queueDispatcher;

		public ActivityService(
			IDataStorage<Group> groupDataStorage,
			IDataStorage<Activity> activityDataStorage,
			IDataStorage<ActivityOccurrence> occurrencesDataStorage,
			IDispatcher dispatcher,
			IQueueDispatcher queueDispatcher)
		{
			_groupDataStorage = groupDataStorage;
			_activityDataStorage = activityDataStorage;
			_occurrencesDataStorage = occurrencesDataStorage;

			_dispatcher = dispatcher;
			_queueDispatcher = queueDispatcher;
		}

		public Activity Handle(CreateActivity request)
		{
			var group = _groupDataStorage.Find(request.GroupKey);
			var activity = new Activity(0, group.Key, request.Name);

			return _activityDataStorage.Create(activity);
		}

		public Activity Handle(UpdateActivity request)
		{
			var activity = _activityDataStorage.Find(request.Key);

			activity.Rename(request.Name);

			return _activityDataStorage.Update(activity);
		}

		public bool Handle(DeleteActivity request)
		{
			var cannotDelete = _dispatcher
				.Fetch(new OccurrencesQuery(DateTime.Today, DateTime.Today.AddYears(1)))
				.Any(n => n.ActivityKey == request.Key);

			if (cannotDelete)
			{
				return false;
			}

			var activity = _activityDataStorage.Find(request.Key);
			return _activityDataStorage.Delete(activity);
		}

		public Nothing Handle(ToggleActivityOccurrence request)
		{
			var activity = _activityDataStorage.Find(request.ActivityKey);

			var occurrence = _dispatcher.Fetch(new OccurrencesQuery(request.When, request.When.AddDays(1)))
				.SingleOrDefault(n => n.ActivityKey == activity.Key && n.When == request.When.Date);

			if (occurrence == null)
			{
				occurrence = _occurrencesDataStorage.Create(new ActivityOccurrence(0, activity.Key, request.When));

				_queueDispatcher.Enqueue(new ActivityOccurrenceSaved(activity, occurrence));
			}
			else
			{
				_occurrencesDataStorage.Delete(occurrence);

				_queueDispatcher.Enqueue(new ActivityOccurrenceDeleted(activity, occurrence));
			}

			return Nothing.AtAll;
		}

		public Nothing Handle(UpdateActivityOccurrence request)
		{
			var occurrence = _occurrencesDataStorage.Find(request.Key);

			var activity = _activityDataStorage.Find(occurrence.ActivityKey);

			occurrence.Note = request.Note;
			occurrence.Highlighted = request.Highlighted;
			occurrence.Missed = request.Missed;

			occurrence = _occurrencesDataStorage.Update(occurrence);

			_queueDispatcher.Enqueue(new ActivityOccurrenceSaved(activity, occurrence));

			return Nothing.AtAll;
		}
	}
}
