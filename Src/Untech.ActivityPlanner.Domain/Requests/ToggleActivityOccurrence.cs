using System;
using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace Untech.ActivityPlanner.Domain.Requests
{
	[DataContract]
	public class ToggleActivityOccurrence : ICommand
	{
		public ToggleActivityOccurrence(int activityId, DateTime thatDay)
		{
			ActivityId = activityId;
			When = thatDay.Date;
		}

		[DataMember]
		public int ActivityId { get; private set; }

		[DataMember]
		public DateTime When { get; private set; }
	}
}
