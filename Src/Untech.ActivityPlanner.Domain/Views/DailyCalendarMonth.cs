using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.Home;

namespace Untech.ActivityPlanner.Domain.Views
{
	[DataContract]
	public class DailyCalendarMonth : IHasMonthInfo
	{
		public DailyCalendarMonth(DateTime thatMonth)
		{
			Year = thatMonth.Year;
			Month = thatMonth.Month;
		}

		[DataMember]
		public int Year { get; private set; }

		[DataMember]
		public int Month { get; private set; }

		[DataMember]
		public IReadOnlyCollection<DailyCalendarDay> Days { get; set; }
	}
}