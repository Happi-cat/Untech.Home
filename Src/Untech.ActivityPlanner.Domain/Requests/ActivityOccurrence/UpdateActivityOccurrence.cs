using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace Untech.ActivityPlanner.Domain.Requests.ActivityOccurrence
{
	[DataContract]
	public class UpdateActivityOccurrence : ICommand
	{
		public UpdateActivityOccurrence(int key)
		{
			Key = key;
		}

		[DataMember]
		public int Key { get; private set; }

		[DataMember]
		public string Note { get; set; }

		[DataMember]
		public bool Highlighted { get; set; }

		[DataMember]
		public bool Missed { get; set; }
	}
}