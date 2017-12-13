using System;
using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace Untech.ActivityPlanner.Domain.Requests
{
	[DataContract]
	public class UpdateActivityOccurrence : ICommand
	{
		public UpdateActivityOccurrence(int activityKey, DateTime when)
		{
			ActivityKey = activityKey;
			When = when;
		}
		
		[DataMember]
		public int ActivityKey { get; private set; }
		
		[DataMember]
		public DateTime When { get; private set; }

		[DataMember]
		public string Note { get; set; }

		[DataMember]
		public bool Highlighted { get; set; }
	}
}