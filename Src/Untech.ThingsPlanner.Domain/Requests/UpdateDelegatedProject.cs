using System;
using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace Untech.ThingsPlanner.Domain.Requests
{
	[DataContract]
	public class UpdateDelegatedProject : ICommand<bool>
	{
		public UpdateDelegatedProject(Guid key)
		{
			Key = key;
		}

		[DataMember]
		public Guid Key { get; private set; }

		[DataMember]
		public DateTime? FollowUp { get; set; }

		[DataMember]
		public string Person { get; set; }
	}
}