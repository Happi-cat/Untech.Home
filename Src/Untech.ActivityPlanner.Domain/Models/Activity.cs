using System.Collections.Generic;

namespace Untech.Home.ActivityPlanner.Domain.Models
{
	public class Activity
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Remarks { get; set; }
		public int CategoryId { get; set; }
		public ICollection<Occurrence> Occurrences { get; set; }
		public ICollection<RemindPromise> RemindPromises { get; set; }
		public ICollection<Reminder> Reminders { get; set; }

		public IEnumerable<Reminder> GetReminders()
		{
			foreach (var promise in RemindPromises)
			{
				var reminder = promise.GetReminder(this);
				if (reminder != null)
				{
					yield return reminder;
				}
			}
		}
	}
}
