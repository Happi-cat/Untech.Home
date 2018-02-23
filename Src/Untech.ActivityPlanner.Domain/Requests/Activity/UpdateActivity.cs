using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace Untech.ActivityPlanner.Domain.Requests.Activity
{
	[DataContract]
	public class UpdateActivity : ICommand<Models.Activity>
	{
		public UpdateActivity(int key, string name)
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
