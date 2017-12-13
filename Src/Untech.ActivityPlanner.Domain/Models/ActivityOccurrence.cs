using System;
using System.Runtime.Serialization;

namespace Untech.ActivityPlanner.Domain.Models
{
	[DataContract]
	public class ActivityOccurrence
	{
		public ActivityOccurrence(DateTime thatDay)
		{
			When = thatDay.Date;
		}

		[DataMember]
		public DateTime When { get; private set; }

		[DataMember]
		public string Note { get; set; }

		[DataMember]
		public bool Highlighted { get; set; }

		[DataMember]
		public bool IsPast => When.Date < DateTime.Today;

		[DataMember]
		public bool IsToday => When.Date == DateTime.Today;
	}
}
