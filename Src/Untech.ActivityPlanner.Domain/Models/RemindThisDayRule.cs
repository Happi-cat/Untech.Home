namespace Untech.Home.ActivityPlanner.Domain.Models
{
	public class RemindThisDayRule : IRemindRule
	{
		public RemindRuleType Type => RemindRuleType.ThisDay;

		public Reminder GetReminder(RemindPromise promise, Activity activity)
		{
			return new Reminder(promise, promise.When);
		}
	}
}
