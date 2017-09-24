using System;
using System.Runtime.Serialization;
using Untech.ActivityPlanner.Domain.Models;
using Untech.Practices.CQRS;

namespace Untech.ActivityPlanner.Domain.Requests
{
	[DataContract]
	public class CreateActivityGroup : ICommand<ActivityGroup>
	{
		public CreateActivityGroup(string name)
		{
			Name = name;
		}

		[DataMember]
		public string Name { get; private set; }

		[DataMember]
		public string Remarks { get; set; }
	}

}
