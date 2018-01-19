using System;
using System.Runtime.Serialization;
using Untech.Practices.DataStorage;

namespace Untech.ActivityPlanner.Domain.Models
{
	[DataContract]
	public class Group : IAggregateRoot
	{
		private string _name;

		private Group()
		{
		}

		public Group(int key, string name)
		{
			Key = key;
			Name = name;
		}

		[DataMember]
		public int Key { get; private set; }

		[DataMember]
		public string Name
		{
			get => _name;
			set
			{
				if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));

				_name = value;
			}
		}
	}
}