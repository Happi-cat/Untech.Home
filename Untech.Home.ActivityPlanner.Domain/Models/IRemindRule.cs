namespace Untech.Home.ActivityPlanner.Domain.Models
{
	public interface IRemindRule
	{
		RemindRuleType Type { get; }
		Reminder GetReminder(RemindPromise promise, Activity activity);
	}
}
