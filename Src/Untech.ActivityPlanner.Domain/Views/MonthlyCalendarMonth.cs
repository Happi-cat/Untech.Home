using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Untech.ActivityPlanner.Domain.Views
{
	[DataContract]
	public class MonthlyCalendarMonth : IHasMonthInfo
	{
		public MonthlyCalendarMonth(DateTime thatMonth)
		{
			Year = thatMonth.Year;
			Month = thatMonth.Month;
		}

		[DataMember]
		public int Year { get; private set; }

		[DataMember]
		public int Month { get; private set; }

		[DataMember]
		public bool IsThisMonth => DateTime.Today.Year == Year && DateTime.Today.Month == Month;

		[DataMember]
		public IReadOnlyCollection<MonthlyCalendarMonthActivity> Activities { get; set; }
	}
}