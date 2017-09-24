using System;
using System.Runtime.Serialization;

namespace Untech.ActivityPlanner.Domain.Models
{
	[DataContract]
	public class Note
	{
		public Note(DateTime thatDay)
		{
			When = thatDay.Date;
		}

		[DataMember]
		public DateTime When { get; private set; }

		[DataMember]
		public string Remarks { get; set; }
	}
}