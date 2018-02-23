﻿using System;
using Untech.ActivityPlanner.Domain.Views;
using Untech.Practices.CQRS;

namespace Untech.ActivityPlanner.Domain.Requests
{
	public class MonthlyCalendarQuery : IQuery<MonthlyCalendar>
	{
		public MonthlyCalendarQuery(int from, int to)
			: this(DateTime.Today.AddMonths(from), DateTime.Today.AddMonths(to))
		{

		}

		public MonthlyCalendarQuery(DateTime from, DateTime to)
		{
			Occurrences = new OccurrencesQuery(AlignToMonthStart(from), AlignToMonthStart(to));
		}

		public OccurrencesQuery Occurrences { get; }

		private static DateTime AlignToMonthStart(DateTime dateTime) => new DateTime(dateTime.Year, dateTime.Month, 1);
	}
}