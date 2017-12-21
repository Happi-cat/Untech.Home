using System.Runtime.Serialization;
using Untech.ActivityPlanner.Domain.Models;

namespace Untech.ActivityPlanner.Domain.Views
{
	[DataContract]
	public class DailyCalendarDayActivity
	{
		public DailyCalendarDayActivity(int activityKey, ActivityOccurrence occurrence)
		{
			ActivityKey = activityKey;

			Note = occurrence.Note;
			Higlighted = occurrence.Highlighted;
		}

		[DataMember]
		public int ActivityKey { get; private set; }

		[DataMember]
		public string Note { get; private set; }

		[DataMember]
		public bool Higlighted { get; private set; }
	}
}