using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace Untech.ActivityPlanner.Domain.Requests.Group
{
	[DataContract]
	public class DeleteGroup : ICommand<bool>
	{
		public DeleteGroup(int key)
		{
			Key = key;
		}

		[DataMember]
		public int Key { get; private set; }
	}
}