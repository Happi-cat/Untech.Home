using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.ActivityPlanner.Domain.Models;

namespace Untech.ActivityPlanner.Domain.Views
{
	[DataContract]
	public class DailyCalendarActivity
	{
		[DataMember]
		public int ActivityId { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string Remarks { get; set; }

		[DataMember]
		public ICollection<ActivityOccurrence> Occurrences { get; set; }

		[DataMember]
		public ICollection<Note> Notes { get; set; }

		[DataMember]
		public ICollection<Reminder> Reminders { get; set; }
	}
}
