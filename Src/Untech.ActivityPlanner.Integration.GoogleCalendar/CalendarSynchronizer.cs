using System;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Untech.ActivityPlanner.Domain.Notifications;
using Untech.Practices.CQRS.Handlers;

namespace Untech.ActivityPlanner.Integration.GoogleCalendar
{
	public class CalendarSynchronizer : INotificationHandler<ActivityOccurrenceSaved>,
			INotificationHandler<ActivityOccurrenceDeleted>
	{
		private readonly CalendarService _service;

		public CalendarSynchronizer(BaseClientService.Initializer initializer)
		{
			_service = new CalendarService(initializer);
		}

		public void Publish(ActivityOccurrenceSaved notification)
		{
			throw new NotImplementedException();
		}

		public void Publish(ActivityOccurrenceDeleted notification)
		{
			throw new NotImplementedException();
		}
	}
}