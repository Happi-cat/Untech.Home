using System;
using Untech.ActivityPlanner.Domain.Views;
using Untech.Practices.CQRS;

namespace Untech.ActivityPlanner.Domain.Requests
{
	public class MonthlyCalendarQuery : IQuery<MonthlyCalendar>
	{
		public MonthlyCalendarQuery(OccurrencesQuery occurrencesQuery)
		{
			Occurrences = occurrencesQuery;
		}

		public MonthlyCalendarQuery(DateTime from, DateTime to)
			: this(new OccurrencesQuery(from, to))
		{

		}

		public MonthlyCalendarQuery(int year, int fromMonth, int toMonth)
			: this(new DateTime(year, fromMonth, 1), new DateTime(year, toMonth, 1))
		{

		}

		public OccurrencesQuery Occurrences { get; }
	}
}