using System;
using System.Runtime.Serialization;
using Untech.Practices.DataStorage;

namespace Untech.ActivityPlanner.Domain.Models
{
	[DataContract]
	public class ActivityOccurrence : IAggregateRoot<string>
	{
		public ActivityOccurrence(string key, int activityKey, DateTime thatDay)
		{
			Key = key;
			When = thatDay.Date;
		}

		[DataMember]
		public DateTime When { get; private set; }

		[DataMember]
		public string Note { get; set; }

		[DataMember]
		public bool Highlighted { get; set; }

		[DataMember]
		public bool Missed { get; set; }

		[DataMember]
		public bool IsPast => When.Date < DateTime.Today;

		[DataMember]
		public bool IsToday => When.Date == DateTime.Today;

		public string Key { get; }

		public int ActivityKey { get; set; }
	}
}
