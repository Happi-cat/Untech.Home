﻿using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Untech.ActivityPlanner.Domain.Views
{
	[DataContract]
	public class MonthlyCalendar : ActivitiesView
	{
		[DataMember]
		public IReadOnlyCollection<MonthlyCalendarMonth> Months { get; set; }
	}
}