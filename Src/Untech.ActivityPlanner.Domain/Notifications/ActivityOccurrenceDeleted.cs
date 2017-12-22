using Untech.ActivityPlanner.Domain.Models;
using Untech.Practices.CQRS;

namespace Untech.ActivityPlanner.Domain.Notifications
{
	public class ActivityOccurrenceDeleted: ActivityOccurrenceNotification, INotification
	{
		public ActivityOccurrenceDeleted(Activity activity, ActivityOccurrence occurrence)
			: base(activity, occurrence)
		{
		}
	}
}