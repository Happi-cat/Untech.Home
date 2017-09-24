using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Untech.ActivityPlanner.Domain.Views
{
	[DataContract]
	public class DailyCalendar
	{
		public DailyCalendar(DateTime from, DateTime to)
		{
			From = from;
			To = to;
		}

		[DataMember]
		public DateTime From { get; private set; }

		[DataMember]
		public DateTime To { get; private set; }

		[DataMember]
		public ICollection<DailyCalendarGroup> Groups { get; set; }
	}
}
