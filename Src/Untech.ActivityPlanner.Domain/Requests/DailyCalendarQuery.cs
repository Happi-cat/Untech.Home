using System;
using Untech.ActivityPlanner.Domain.Requests.ActivityOccurrence;
using Untech.ActivityPlanner.Domain.Views;
using Untech.Practices.CQRS;

namespace Untech.ActivityPlanner.Domain.Requests
{
	public class DailyCalendarQuery : IQuery<DailyCalendar>
	{
		public DailyCalendarQuery(int from, int to)
			: this(DateTime.Today.AddDays(from), DateTime.Today.AddDays(to))
		{

		}

		public DailyCalendarQuery(DateTime from, DateTime to)
		{
			Occurrences = new OccurrencesQuery(from.Date, to.Date);
		}

		public OccurrencesQuery Occurrences { get; }
	}
}