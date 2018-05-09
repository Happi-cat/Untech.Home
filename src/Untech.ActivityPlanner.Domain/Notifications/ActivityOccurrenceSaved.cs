using Untech.ActivityPlanner.Domain.Models;
using Untech.Practices.CQRS;

namespace Untech.ActivityPlanner.Domain.Notifications
{
	public class ActivityOccurrenceSaved : ActivityOccurrenceNotification, INotification
	{
		public ActivityOccurrenceSaved(Activity activity, ActivityOccurrence occurrence)
			: base(activity, occurrence)
		{
		}
	}
}