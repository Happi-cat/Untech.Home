using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Untech.ActivityPlanner.Domain.Views
{
	[DataContract]
	public class DailyCalendarGroup
	{
		[DataMember]
		public int GroupId { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string Remarks { get; set; }

		[DataMember]
		public ICollection<DailyCalendarActivity> Activities { get; set; }
	}
}
