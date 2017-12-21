using System.Collections.Generic;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Untech.ActivityPlanner.Domain.Models;
using Untech.Practices.DataStorage;

namespace Untech.ActivityPlanner.Data
{
	public class ActivityOccurrenceStorage : IDataStorage<ActivityOccurrence, string>
	{
		private readonly CalendarService _service;

		public ActivityOccurrence Find(string key)
		{
			var activityEvent = _service.Events.Get("primary", key).Execute();

			return new ActivityOccurrence(activityEvent.Id, activityEvent.Start.DateTime.Value)
			{
				Note = activityEvent.Description,
			};
		}

		public ActivityOccurrence Create(ActivityOccurrence entity)
		{
			var request = _service.Events.Insert(new Event
			{
				Summary = "",
				Description = entity.Note,
				ExtendedProperties = new Event.ExtendedPropertiesData
				{
					Private__ = new Dictionary<string, string>
					{
						["activityId"] = ""
					}
				}
			}, "primary");

			var activityEvent = request.Execute();
			return Find(activityEvent.Id);
		}

		public ActivityOccurrence Update(ActivityOccurrence entity)
		{

			throw new System.NotImplementedException();
		}

		public bool Delete(ActivityOccurrence entity)
		{
			throw new System.NotImplementedException();
		}
	}
}