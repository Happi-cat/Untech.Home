using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.ActivityPlanner.Domain.Models;

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
			DayOfWeek = thatDay.DayOfWeek == System.DayOfWeek.Sunday ? 7 : (int)thatDay.DayOfWeek;
		}

		[DataMember]
		public int Year { get; private set; }

		[DataMember]
		public int Month { get; private set; }

		[DataMember]
		public int Day { get; private set; }

		[DataMember]
		public int DayOfWeek { get; private set; }

		[DataMember]
		public IReadOnlyCollection<ActivityOccurrence> Activities { get; set; }
	}
}