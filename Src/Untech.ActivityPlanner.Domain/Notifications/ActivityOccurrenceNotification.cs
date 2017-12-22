using Untech.ActivityPlanner.Domain.Models;

namespace Untech.ActivityPlanner.Domain.Notifications
{
	public class ActivityOccurrenceNotification
	{
		public ActivityOccurrenceNotification(Activity activity, ActivityOccurrence occurrence)
		{
			Activity = activity;
			Occurrence = occurrence;
		}

		public Activity Activity { get; private set; }

		public ActivityOccurrence Occurrence { get; private set; }
	}
}