using System.Runtime.Serialization;
using Untech.ActivityPlanner.Domain.Models;

namespace Untech.ActivityPlanner.Domain.Views
{
	[DataContract]
	public class DailyCalendarDayActivity
	{
		public DailyCalendarDayActivity(Activity activity, ActivityOccurrence occurrence)
		{
			ActivityKey = activity.Key;

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