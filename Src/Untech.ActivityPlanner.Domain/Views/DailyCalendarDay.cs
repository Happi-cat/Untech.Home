using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.ActivityPlanner.Domain.Models;
using Untech.Home;

namespace Untech.ActivityPlanner.Domain.Views
{
	[DataContract]
	public class DailyCalendarDay : IHasDayInfo
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
		public bool IsThisDay => new DateTime(Year, Month, Day) == DateTime.Today;

		[DataMember]
		public IReadOnlyCollection<ActivityOccurrence> Activities { get; set; }
	}
}