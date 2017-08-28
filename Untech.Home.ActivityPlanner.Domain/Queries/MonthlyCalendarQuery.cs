using System;
using System.Collections.Generic;
using Untech.Home.ActivityPlanner.Domain.Views;
using Untech.Practices.CQRS;

namespace Untech.Home.ActivityPlanner.Domain.Queries
{
	public class MonthlyCalendarQuery : IQuery<MonthlyCalendar>
	{
		public DateTime From { get; set; }

		public DateTime To { get; set; }
	}
}
