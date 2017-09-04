using System.Linq;

namespace Untech.Home.ActivityPlanner.Domain.Models
{
	public class RemindAfterOccurrencesRule : IRemindRule
	{
		public int OccurrencesCount { get; set; }

		public RemindRuleType Type => RemindRuleType.AfterOccurrences;

		public Reminder GetReminder(RemindPromise promise, Activity activity)
		{
			var occurrence = activity.Occurrences
				.Where(n => n.When >= promise.When)
				.Skip(OccurrencesCount)
				.FirstOrDefault();

			return (occurrence == null)
				? null
				: new Reminder(promise, occurrence.When);
		}
	}
}
