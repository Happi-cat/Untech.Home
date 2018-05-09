using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Untech.ActivityPlanner.Domain.Views
{
	[DataContract]
	public class MonthlyCalendar
	{
		public MonthlyCalendar(ActivitiesView view)
		{
			View = view;
		}

		[DataMember]
		public ActivitiesView View { get; }

		[DataMember]
		public IReadOnlyCollection<MonthlyCalendarMonth> Months { get; set; }
	}
}