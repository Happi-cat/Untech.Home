using System;
using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace Untech.ActivityPlanner.Domain.Requests
{
	[DataContract]
	public class ToggleActivityOccurrence : ICommand
	{
		public ToggleActivityOccurrence(int activityKey, DateTime when)
		{
			ActivityKey = activityKey;
			When = when.Date;
		}

		[DataMember]
		public int ActivityKey { get; private set; }

		[DataMember]
		public DateTime When { get; private set; }
	}
}
