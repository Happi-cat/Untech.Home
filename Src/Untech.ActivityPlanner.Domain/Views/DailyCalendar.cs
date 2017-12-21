using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Untech.ActivityPlanner.Domain.Views
{
	[DataContract]
	public class DailyCalendar
	{
		public DailyCalendar(ActivitiesView view)
		{
			View = view;
		}

		[DataMember]
		public ActivitiesView View { get; private set; }

		[DataMember]
		public IReadOnlyCollection<DailyCalendarDay> Days { get; set; }
	}
}