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

		public OccurrencesQuery Occurrences { get; }
	}
}