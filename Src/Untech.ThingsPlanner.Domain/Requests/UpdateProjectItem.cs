using System;
using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace Untech.ThingsPlanner.Domain.Requests
{
	[DataContract]
	public class UpdateProjectItem : ICommand<bool>
	{
		public UpdateProjectItem(Guid projectKey, Guid key, string title, bool done)
		{
			ProjectKey = projectKey;
			Key = key;
			Title = title;
			Done = done;
		}

		[DataMember]
		public Guid ProjectKey { get; private set; }

		[DataMember]
		public Guid Key { get; private set; }

		[DataMember]
		public string Title { get; private set; }

		[DataMember]
		public bool Done { get; private set; }
	}
}