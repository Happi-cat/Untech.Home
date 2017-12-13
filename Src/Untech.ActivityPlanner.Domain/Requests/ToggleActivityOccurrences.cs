using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace Untech.ActivityPlanner.Domain.Requests
{
	[DataContract]
	public class ToggleActivityOccurrences : ICommand
	{
		public ToggleActivityOccurrences(int activityKey)
		{
			ActivityKey = activityKey;
		}

		[DataMember]
		public int ActivityKey { get; private set; }

		[DataMember]
		public IReadOnlyCollection<DateTime> Added { get; set; }

		[DataMember]
		public IReadOnlyCollection<DateTime> Deleted { get; set; }
	}
}
