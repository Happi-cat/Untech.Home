using System.Collections.Generic;
using Untech.Home.ActivityPlanner.Domain.Models;

namespace Untech.Home.ActivityPlanner.Domain.Views
{
	public class CategoryCalendar
	{
		public int CategoryId { get; set; }
		public string Name { get; set; }
		public string Remarks { get; set; }

		public ICollection<ActivityCalendar> Activities { get; set; }
	}

	public class ActivityCalendar
	{
		public int ActivityId { get; set; }
		public string Name { get; set; }
		public string Remarks { get; set; }

		public ICollection<ActivityDay> Days { get; set; }
	}

	public class ActivityDay
	{
		public Date When { get; set; }
		public bool IsOccurrence { get; set; }
		public string Remarks { get; set; }

		public ICollection<Reminder> Reminder { get; set; }
		public ICollection<RemindPromise> RemindPromises { get; set; }
	}
}
