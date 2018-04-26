using System;
using System.Runtime.Serialization;
using Untech.Practices.CQRS;
using Untech.ThingsPlanner.Domain.Models;

namespace Untech.ThingsPlanner.Domain.Requests
{
	[DataContract]
	public class AddProjectItem : ICommand<bool>
	{
		public AddProjectItem(Guid projectKey, Thing.Project.ProjectItemType type, string title)
		{
			ProjectKey = projectKey;
			Type = type;
			Title = title;
		}

		[DataMember]
		public Guid ProjectKey { get; private set; }

		[DataMember]
		public Thing.Project.ProjectItemType Type { get; private set; }

		[DataMember]
		public string Title { get; private set; }
	}
}