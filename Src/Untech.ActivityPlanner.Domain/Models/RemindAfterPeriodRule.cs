using System;
using System.Collections.Generic;

namespace Untech.Home.ActivityPlanner.Domain.Models
{

	public class RemindAfterPeriodRule : IRemindRule
	{
		public TimeSpan Period { get; set; }

		public RemindRuleType Type => RemindRuleType.AfterPeriod;

		public Reminder GetReminder(RemindPromise promise, Activity activity)
		{
			return new Reminder(promise, promise.When + Period);
		}
	}
}
