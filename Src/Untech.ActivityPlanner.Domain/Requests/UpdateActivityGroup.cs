using System;
using System.Runtime.Serialization;
using Untech.ActivityPlanner.Domain.Models;
using Untech.Practices.CQRS;

namespace Untech.ActivityPlanner.Domain.Requests
{
	[DataContract]
	public class UpdateActivityGroup : ICommand<ActivityGroup>
	{
		public UpdateActivityGroup(int id, string name)
		{
			Id = id;
			Name = name;
		}

		[DataMember]
		public int Id { get; private set; }

		[DataMember]
		public string Name { get; private set; }

		[DataMember]
		public string Remarks { get; set; }
	}

}
