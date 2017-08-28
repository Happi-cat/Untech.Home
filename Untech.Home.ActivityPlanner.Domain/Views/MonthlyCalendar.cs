using System;
using System.Collections.Generic;

namespace Untech.Home.ActivityPlanner.Domain.Views
{
	public class MonthlyCalendar
	{
		public DateTime From {get;set;}

		public DateTime To {get;set;}

		public ICollection<CategoryMonthlyCalendar> Categories { get; set; }
	}

	public class CategoryMonthlyCalendar {
		public int CategoryId {get;set;}

		public string Name {get;set;}

		public string Remarks {get;set;}

		public ICollection<ActivityMonthlyCalendar> Activities {get;set;}
	}

	public class ActivityMonthlyCalendar {
		public int ActivityId { get; set; }
		public string Name { get; set; }
		public string Remarks { get; set; }

		public DateTime Month {get;set;}

		public int OccurrencesCount {get;set;}
	}
}