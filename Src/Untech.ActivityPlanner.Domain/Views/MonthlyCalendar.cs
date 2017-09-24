using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Untech.ActivityPlanner.Domain.Views
{
	[DataContract]
	public class MonthlyCalendar
	{
		public MonthlyCalendar(DateTime from, DateTime to)
		{
			From = from;
			To = to;
		}

		[DataMember]
		public DateTime From { get; private set; }

		[DataMember]
		public DateTime To { get; private set; }

		[DataMember]
		public ICollection<MonthlyCalendarGroup> Groups { get; set; }
	}
}