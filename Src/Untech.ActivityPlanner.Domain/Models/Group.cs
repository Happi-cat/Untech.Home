using System.Runtime.Serialization;
using Untech.Practices.DataStorage;

namespace Untech.ActivityPlanner.Domain.Models
{
	[DataContract]
	public class Group : IAggregateRoot
	{
		public Group(int key, string name)
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
