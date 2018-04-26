using System;
using System.Runtime.Serialization;
using Untech.Practices.CQRS;
using Untech.ThingsPlanner.Domain.Models;

namespace Untech.ThingsPlanner.Domain.Requests
{
	[DataContract]
	public class ConvertThing : ICommand<bool>
	{
		public ConvertThing(Guid key, ThingType type)
		{
			Key = key;
			Type = type;
		}

		[DataMember]
		public Guid Key { get; private set; }

		[DataMember]
		public ThingType Type { get; private set; }
	}
}