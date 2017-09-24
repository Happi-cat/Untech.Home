﻿using System;
using System.Collections.Generic;
using Untech.ActivityPlanner.Domain.Models;
using Untech.ActivityPlanner.Domain.Views;
using Untech.Practices.CQRS;

namespace Untech.ActivityPlanner.Domain.Requests
{
	public class DailyCalendarQuery : IQuery<DailyCalendar>
	{
		public DailyCalendarQuery(DateTime from, DateTime to)
		{
			if (From > To) throw new ArgumentException("From cannot be greater than To");

			From = from;
			To = to;
		}

		public DateTime From { get; private set; }

		public DateTime To { get; private set; }
	}
}
