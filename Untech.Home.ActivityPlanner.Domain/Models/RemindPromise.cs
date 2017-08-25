using System;

namespace Untech.Home.ActivityPlanner.Domain.Models
{
	public class RemindPromise
	{
		public int Id { get; set; }
		public DateTime When { get; set; }
		public string Name { get; set; }

		public IRemindRule Rule { get; set; }

		public Reminder GetReminder(Activity activity)
		{
			return Rule?.GetReminder(this, activity);
		}
	}
}
