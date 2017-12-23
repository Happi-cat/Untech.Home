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
	public class ActivityOccurrencesService : ICommandHandler<ToggleActivityOccurrence, Nothing>,
		ICommandHandler<UpdateActivityOccurrence, Nothing>
	{
		private readonly IDataStorage<ActivityOccurrence> _occurrencesDataStorage;
		private readonly IDispatcher _dispatcher;
		private readonly IDataStorage<Activity> _activityDataStorage;

		public ActivityOccurrencesService(IDataStorage<Activity> activityDataStorage, IDataStorage<ActivityOccurrence> occurrencesDataStorage, IDispatcher dispatcher)
		{
			_activityDataStorage = activityDataStorage;
			_occurrencesDataStorage = occurrencesDataStorage;
			_dispatcher = dispatcher;
		}

		public Nothing Handle(ToggleActivityOccurrence request)
		{
			var activity = _activityDataStorage.Find(request.ActivityKey);

			var occurrence = _dispatcher.Fetch(new OccurrencesQuery(request.When, request.When))
				.SingleOrDefault(_ => _.ActivityKey == activity.Key && _.When == request.When.Date);

			if (occurrence == null)
			{
				occurrence = _occurrencesDataStorage.Create(new ActivityOccurrence(0, activity.Key, request.When));

				_dispatcher.Publish(new ActivityOccurrenceSaved(activity, occurrence));
			}
			else
			{
				_occurrencesDataStorage.Delete(occurrence);

				_dispatcher.Publish(new ActivityOccurrenceDeleted(activity, occurrence));
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

			_dispatcher.Publish(new ActivityOccurrenceSaved(activity, occurrence));

			return Nothing.AtAll;
		}
	}
}
