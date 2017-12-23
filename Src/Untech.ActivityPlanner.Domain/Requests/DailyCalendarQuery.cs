using System;
using Untech.ActivityPlanner.Domain.Views;
using Untech.Practices.CQRS;

namespace Untech.ActivityPlanner.Domain.Requests
{
	public class DailyCalendarQuery : IQuery<DailyCalendar>
	{
		public DailyCalendarQuery(OccurrencesQuery occurrencesQuery)
		{
			OccurrencesQuery = occurrencesQuery;
		}

		public DailyCalendarQuery(DateTime from, DateTime to)
			: this(new OccurrencesQuery(from, to))
		{

		}

		public OccurrencesQuery OccurrencesQuery { get; private set; }
	}
}