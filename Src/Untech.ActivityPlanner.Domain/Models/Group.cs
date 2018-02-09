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
			set => _name = !string.IsNullOrWhiteSpace(value)
				? value
				: throw new ArgumentException("Argument cannot be null, empty or whitespace", nameof(value));
		}
	}
}