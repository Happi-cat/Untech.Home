using System;
using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace Untech.ThingsPlanner.Domain.Requests
{
	[DataContract]
	public class DeleteProjectItem : ICommand<bool>
	{
		public DeleteProjectItem(Guid projectKey, Guid key)
		{
			ProjectKey = projectKey;
			Key = key;
		}

		[DataMember]
		public Guid ProjectKey { get; private set; }

		[DataMember]
		public Guid Key { get; private set; }
	}
}