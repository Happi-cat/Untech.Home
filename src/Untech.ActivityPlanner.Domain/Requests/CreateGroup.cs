using System.Runtime.Serialization;
using Untech.ActivityPlanner.Domain.Models;
using Untech.Practices.CQRS;

namespace Untech.ActivityPlanner.Domain.Requests
{
	[DataContract]
	public class CreateGroup : ICommand<Group>
	{
		public CreateGroup(string name)
		{
			Name = name;
		}

		[DataMember]
		public string Name { get; private set; }
	}
}
