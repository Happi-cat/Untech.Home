using System;
using System.Runtime.Serialization;
using Untech.Practices.CQRS;
using Untech.ThingsPlanner.Domain.Models;

namespace Untech.ThingsPlanner.Domain.Requests
{
	[DataContract]
	public class ChangeProjectItemType : ICommand<bool>
	{
		public ChangeProjectItemType(Guid projectKey, Guid key, Thing.Project.ProjectItemType type)
		{
			ProjectKey = projectKey;
			Key = key;
			Type = type;
		}

		[DataMember]
		public Guid ProjectKey { get; private set; }

		[DataMember]
		public Guid Key { get; private set; }

		[DataMember]
		public Thing.Project.ProjectItemType Type { get; private set; }
	}
}