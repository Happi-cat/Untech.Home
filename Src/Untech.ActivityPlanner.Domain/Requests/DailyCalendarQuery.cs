using System;
using Untech.ActivityPlanner.Domain.Views;
using Untech.Practices.CQRS;

namespace Untech.ActivityPlanner.Domain.Requests
{
	public class DailyCalendarQuery : IQuery<DailyCalendar>
	{
		public DailyCalendarQuery(DateTime from, DateTime to)
		{
			Occurrences = new OccurrencesQuery(from, to);
		}

		public OccurrencesQuery Occurrences { get; private set; }
	}
}