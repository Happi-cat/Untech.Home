using System.Runtime.Serialization;
using Untech.Practices.DataStorage;

namespace Untech.ActivityPlanner.Domain.Models
{
	[DataContract]
	public class Activity : IAggregateRoot
	{
		public Activity(int key, int groupId, string name)
		{
			Key = key;
			GroupId = groupId;
			Name = name;
		}

		[DataMember]
		public int Key { get; private set; }

		[DataMember]
		public string Name { get; private set; }

		[DataMember]
		public int GroupId { get; private set; }
	}
}
