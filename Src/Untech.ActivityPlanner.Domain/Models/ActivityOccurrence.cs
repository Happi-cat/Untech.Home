using System;
using System.Runtime.Serialization;
using Untech.Practices.DataStorage;

namespace Untech.ActivityPlanner.Domain.Models
{
	[DataContract]
	public class ActivityOccurrence : IAggregateRoot
	{
		public ActivityOccurrence(int key, int activityKey, DateTime thatDay)
		{
			Key = key;
			ActivityKey = activityKey;
			When = thatDay.Date;
		}

		[DataMember]
		public int Key { get; private set; }

		[DataMember]
		public int ActivityKey { get; private set; }

		[DataMember]
		public DateTime When { get; private set; }

		[DataMember]
		public string Note { get; set; }

		[DataMember]
		public bool Highlighted { get; set; }

		[DataMember]
		public bool Missed { get; set; }

		[DataMember]
		public bool Ongoing => When > DateTime.Today;
	}
}