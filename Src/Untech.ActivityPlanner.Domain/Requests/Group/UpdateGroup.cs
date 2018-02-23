using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace Untech.ActivityPlanner.Domain.Requests.Group
{
	[DataContract]
	public class UpdateGroup : ICommand<Models.Group>
	{
		public UpdateGroup(int key, string name)
		{
			Key = key;
			Name = name;
		}

		[DataMember]
		public int Key { get; private set; }

		[DataMember]
		public string Name { get; private set; }
	}
}