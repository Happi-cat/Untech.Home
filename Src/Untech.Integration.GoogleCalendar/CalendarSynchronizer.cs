using System;
using System.Collections.Generic;
using System.Linq;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util;
using Untech.ActivityPlanner.Domain.Models;
using Untech.ActivityPlanner.Domain.Notifications;
using Untech.Practices.CQRS.Handlers;

namespace Untech.ActivityPlanner.Integration.GoogleCalendar
{
	public class CalendarSynchronizer : INotificationHandler<ActivityOccurrenceSaved>,
		INotificationHandler<ActivityOccurrenceDeleted>
	{
		private const string CalendarKey = "untech.home.activityPlanner.occurrenceKey";

		private readonly CalendarService _service;

		public CalendarSynchronizer(BaseClientService.Initializer initializer)
		{
			_service = new CalendarService(initializer);
		}

		public void Publish(ActivityOccurrenceSaved notification)
		{
			var when = notification.Occurrence.When;
			Event calendarEvent = FindCalendarEvent(notification)
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
				_service.Events
					.Insert(calendarEvent, "primary")
					.Execute();
			}
			else
			{
				_service.Events
					.Update(calendarEvent, "primary", calendarEvent.Id)
					.Execute();
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
						[CalendarKey] = notification.Occurrence.Key.ToString()
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

		public void Publish(ActivityOccurrenceDeleted notification)
		{
			Event calendarEvent = FindCalendarEvent(notification);

			if (calendarEvent != null)
			{
				_service.Events
					.Delete("primary", calendarEvent.Id)
					.Execute();
			}
		}

		private Event FindCalendarEvent(ActivityOccurrenceNotification notification)
		{
			var eventsRequest = _service.Events.List("primary");

			eventsRequest.MaxResults = 1;
			eventsRequest.PrivateExtendedProperty = new Repeatable<string>(new[]
			{
				$"{CalendarKey}={notification.Occurrence.Key}"
			});

			return eventsRequest
				.Execute()
				.Items
				.SingleOrDefault();
		}

		private IEnumerable<string> GetPrefixes(ActivityOccurrence occurrence)
		{
			if (occurrence.Highlighted) yield return "!";
			if (occurrence.Missed) yield return "-";
		}
	}
}