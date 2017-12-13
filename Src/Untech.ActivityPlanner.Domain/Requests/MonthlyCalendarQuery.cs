using System;
using Untech.Practices.CQRS;

namespace Untech.ActivityPlanner.Domain.Requests
{
	public class MonthlyCalendarQuery : IQuery<MonthlyCalendarQuery>
	{
		public MonthlyCalendarQuery(DateTime monthFrom, DateTime monthTo)
		{
			MonthFrom = monthFrom;
			MonthTo = monthTo;
		}

		public DateTime MonthFrom { get; }

		public DateTime MonthTo { get; }
	}
}