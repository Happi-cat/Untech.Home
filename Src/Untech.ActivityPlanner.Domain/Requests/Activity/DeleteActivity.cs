using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace Untech.ActivityPlanner.Domain.Requests.Activity
{
	[DataContract]
	public class DeleteActivity : ICommand<bool>
	{
		public DeleteActivity(int key)
		{
			Key = key;
		}

		[DataMember]
		public int Key { get; private set; }
	}
}