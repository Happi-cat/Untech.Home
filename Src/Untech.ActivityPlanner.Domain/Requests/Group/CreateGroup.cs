using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace Untech.ActivityPlanner.Domain.Requests.Group
{
	[DataContract]
	public class CreateGroup : ICommand<Models.Group>
	{
		public CreateGroup(string name)
		{
			Name = name;
		}

		[DataMember]
		public string Name { get; private set; }
	}
}
