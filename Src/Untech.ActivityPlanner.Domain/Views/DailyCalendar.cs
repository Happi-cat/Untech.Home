using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Untech.ActivityPlanner.Domain.Views
{
	[DataContract]
	public class DailyCalendar : ActivitiesView
	{
		[DataMember]
		public IReadOnlyCollection<DailyCalendarDay> Days { get; set; }
	}
}