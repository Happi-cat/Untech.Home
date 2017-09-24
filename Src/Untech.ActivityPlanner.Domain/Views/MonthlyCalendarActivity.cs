using System;
using System.Runtime.Serialization;

namespace Untech.ActivityPlanner.Domain.Views
{
	[DataContract]
	public class MonthlyCalendarActivity
	{
		[DataMember]
		public int ActivityId { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string Remarks { get; set; }

		[DataMember]
		public int Year { get; set; }

		[DataMember]
		public int Month { get; set; }

		[DataMember]
		public int OccurrencesCount { get; set; }
	}
}