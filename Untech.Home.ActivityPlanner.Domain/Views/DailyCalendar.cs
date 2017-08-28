using System;
using System.Collections.Generic;
using Untech.Home.ActivityPlanner.Domain.Models;

namespace Untech.Home.ActivityPlanner.Domain.Views
{
	public class DailyCalendar
	{
		public DateTime From {get;set;}

		public DateTime To {get;set;}

		public ICollection<CategoryDailyCalendar> Categories { get; set; }
	}

	public class CategoryDailyCalendar {
		public int CategoryId {get;set;}

		public string Name {get;set;}

		public string Remarks {get;set;}

		public ICollection<ActivityDailyCalendar> Activities {get;set;}
	}

	public class ActivityDailyCalendar
	{
		public int ActivityId { get; set; }
		public string Name { get; set; }
		public string Remarks { get; set; }

		public ICollection<Occurrence> Occurrences { get; set; }
	}
}
