using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Untech.ActivityPlanner.Domain.Views
{
	[DataContract]
	public class DailyCalendarDay
	{
		public DailyCalendarDay(DateTime thatDay)
		{
			Year = thatDay.Year;
			Month = thatDay.Month;
			Day = thatDay.Day;
		}

		[DataMember]
		public int Year { get; private set; }

		[DataMember]
		public int Month { get; private set; }

		[DataMember]
		public int Day { get; private set; }

		[DataMember]
		public IReadOnlyCollection<DailyCalendarDayActivity> Activities { get; set; }
	}
}