using System.Collections.Generic;
using Untech.Home.ActivityPlanner.Domain.Models;
using Untech.Practices.CQRS;

namespace Untech.Home.ActivityPlanner.Domain.Queries
{
	public class RemindersQuery : IQuery<IEnumerable<Reminder>> { }

	public class CategoryCalendarsQuery : IQuery<IEnumerable<CategoryCalendar>>
	{
		public Date From { get; set; }
		public Date To { get; set; }
	}
}
