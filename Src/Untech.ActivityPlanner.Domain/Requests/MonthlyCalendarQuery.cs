using System;
using Untech.ActivityPlanner.Domain.Views;
using Untech.Practices.CQRS;

namespace Untech.ActivityPlanner.Domain.Requests
{
	public class MonthlyCalendarQuery : IQuery<MonthlyCalendar>
	{
		public MonthlyCalendarQuery(int year, int fromMonth, int toMonth)
		{
			Occurrences = new OccurrencesQuery(new DateTime(year, fromMonth, 1), new DateTime(year, toMonth, 1));
		}

		public OccurrencesQuery Occurrences { get; }
	}
}