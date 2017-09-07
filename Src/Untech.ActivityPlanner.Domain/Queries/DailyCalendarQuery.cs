﻿using System;
using System.Collections.Generic;
using Untech.Home.ActivityPlanner.Domain.Models;
using Untech.Home.ActivityPlanner.Domain.Views;
using Untech.Practices.CQRS;

namespace Untech.Home.ActivityPlanner.Domain.Queries
{
	public class DailyCalendarQuery : IQuery<DailyCalendar>
	{
		public DateTime From { get; set; }
		public DateTime To { get; set; }
	}
}