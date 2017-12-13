using System;
using Untech.ActivityPlanner.Domain.Views;
using Untech.Practices.CQRS;

namespace Untech.ActivityPlanner.Domain.Requests
{
	public class DailyCalendarQuery  : IQuery<DailyCalendar>
	{
		public DailyCalendarQuery(DateTime dayFrom, DateTime dayTo)
		{
			DayFrom = dayFrom;
			DayTo = dayTo;
		}

		public DateTime DayFrom { get; private set; }

		public DateTime DayTo { get; private set; }
	}
}