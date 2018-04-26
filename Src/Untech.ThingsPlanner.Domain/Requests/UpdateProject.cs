using System;
using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace Untech.ThingsPlanner.Domain.Requests
{
	[DataContract]
	public class UpdateProject : ICommand<bool>
	{
		public UpdateProject(Guid key)
		{
			Key = key;
		}

		[DataMember]
		public Guid Key { get; private set; }

		[DataMember]
		public DateTime? Start { get; set; }

		[DataMember]
		public DateTime? End { get; set; }

		[DataMember]
		public string Expectation { get; }
	}
}