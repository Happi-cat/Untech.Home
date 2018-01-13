using System;
using System.Runtime.Serialization;
using Untech.Practices.DataStorage;

namespace Untech.ActivityPlanner.Domain.Models
{
	[DataContract]
	public class Activity : IAggregateRoot
	{
		private Activity()
		{

		}

		public Activity(int key, int groupKey, string name)
		{
			Key = key;
			GroupKey = groupKey;
			Name = name;
		}

		[DataMember]
		public int Key { get; private set; }

		[DataMember]
		public string Name { get; private set; }

		[DataMember]
		public int GroupKey { get; private set; }

		public void ChangeName(string name)
		{
			if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

			Name = name;
		}
	}
}
