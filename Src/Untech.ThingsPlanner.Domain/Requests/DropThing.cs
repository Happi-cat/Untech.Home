using System;
using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace Untech.ThingsPlanner.Domain.Requests
{
	[DataContract]
	public class DropThing : ICommand<bool>
	{
		public DropThing(Guid key)
		{
			Key = key;
		}

		[DataMember]
		public Guid Key { get; private set; }
	}
}