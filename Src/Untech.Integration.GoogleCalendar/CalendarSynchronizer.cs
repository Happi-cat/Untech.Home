using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util;
using Untech.ActivityPlanner.Domain.Models;
using Untech.ActivityPlanner.Domain.Notifications;
using Untech.Practices.CQRS.Handlers;

namespace Untech.ActivityPlanner.Integration.GoogleCalendar
{
	public class CalendarSynchronizer : INotificationAsyncHandler<ActivityOccurrenceSaved>,
		INotificationAsyncHandler<ActivityOccurrenceDeleted>
	{
		private const string CalendarKey = "untech.home.activityPlanner.occurrenceKey";

		private readonly CalendarService _service;

		public CalendarSynchronizer(BaseClientService.Initializer initializer)
		{
			_service = new CalendarService(initializer);
		}

		public async Task PublishAsync(ActivityOccurrenceSaved notification, CancellationToken cancellationToken)
		{
			if (notification.Occurrence.ExternalKey == Guid.Empty) return;

			var when = notification.Occurrence.When;
			Event calendarEvent = await FindCalendarEventAsync(notification, cancellationToken)
				?? GetDefaultCalendarEvent(notification, when);

			var prefix = string.Join(" ", GetPrefixes(notification.Occurrence).Select(n => $"[{n}]"));

			calendarEvent.Summary = $"{prefix} {notification.Activity.Name}".TrimStart();
			calendarEvent.Description = notification.Occurrence.Note;
			calendarEvent.Start = new EventDateTime
			{
				Date = when.ToString("yyyy-MM-dd")
			};
			calendarEvent.End = calendarEvent.Start;

			if (string.IsNullOrEmpty(calendarEvent.Id))
			{
				await _service.Events
					.Insert(calendarEvent, "primary")
					.ExecuteAsync(cancellationToken);
			}
			else
			{
				await _service.Events
					.Update(calendarEvent, "primary", calendarEvent.Id)
					.ExecuteAsync(cancellationToken);
			}
		}

		private static Event GetDefaultCalendarEvent(ActivityOccurrenceSaved notification, DateTime when)
		{
			return new Event
			{
				ExtendedProperties = new Event.ExtendedPropertiesData
				{
					Private__ = new Dictionary<string, string>
					{
						[CalendarKey] = notification.Occurrence.ExternalKey.ToString()
					}
				},
				Reminders = new Event.RemindersData
				{
					UseDefault = false,
					Overrides = new List<EventReminder>
					{
						new EventReminder
						{
							Method = "popup",
							Minutes = (int) (when - when.AddDays(-1).AddHours(18)).TotalMinutes
						}
					}
				}
			};
		}

		public async Task PublishAsync(ActivityOccurrenceDeleted notification, CancellationToken cancellationToken)
		{
			if (notification.Occurrence.ExternalKey == Guid.Empty) return;

			Event calendarEvent = await FindCalendarEventAsync(notification, cancellationToken);

			if (calendarEvent != null)
			{
				await _service.Events
					.Delete("primary", calendarEvent.Id)
					.ExecuteAsync(cancellationToken);
			}
		}

		private async Task<Event> FindCalendarEventAsync(ActivityOccurrenceNotification notification, CancellationToken cancellationToken)
		{
			var eventsRequest = _service.Events.List("primary");

			eventsRequest.MaxResults = 1;
			eventsRequest.PrivateExtendedProperty = new Repeatable<string>(new[]
			{
				$"{CalendarKey}={notification.Occurrence.ExternalKey}"
			});

			var response = await eventsRequest
				.ExecuteAsync(cancellationToken);

			return response.Items.SingleOrDefault();
		}

		private IEnumerable<string> GetPrefixes(ActivityOccurrence occurrence)
		{
			if (occurrence.Highlighted) yield return "!";
			if (occurrence.Missed) yield return "-";
		}
	}
}