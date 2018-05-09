using Untech.ActivityPlanner.Domain.Models;

namespace Untech.ActivityPlanner.Domain.Notifications
{
	public abstract class ActivityOccurrenceNotification
	{
		protected ActivityOccurrenceNotification(Activity activity, ActivityOccurrence occurrence)
		{
			Activity = activity;
			Occurrence = occurrence;
		}

		public Activity Activity { get; }

		public ActivityOccurrence Occurrence { get; }
	}
}