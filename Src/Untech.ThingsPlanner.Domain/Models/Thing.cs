using System.Runtime.Serialization;
using Untech.Practices.DataStorage;

namespace Untech.ThingsPlanner.Domain.Models
{
	[DataContract]
	public abstract partial class Thing : IAggregateRoot
	{
		protected Thing(ThingType type, int key, string title)
		{
			Type = type;
			Key = key;
			Title = title;
		}

		[DataMember]
		public int Key { get; private set; }

		[DataMember]
		public string Title { get; private set; }

		[DataMember]
		public ThingType Type { get; private set; }
	}
}