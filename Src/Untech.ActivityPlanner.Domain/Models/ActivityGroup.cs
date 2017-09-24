using System;
using System.Runtime.Serialization;

namespace Untech.ActivityPlanner.Domain.Models
{
	[DataContract]
	public class ActivityGroup
	{
		public ActivityGroup(int key, string name)
		{
			Key = key;
			Name = name;
		}

		[DataMember]
		public int Key { get; private set; }

		[DataMember]
		public string Name { get; private set; }

		[DataMember]
		public string Remarks { get; set; }
	}
}
