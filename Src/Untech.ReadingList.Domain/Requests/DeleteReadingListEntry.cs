using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace Untech.ReadingList.Domain.Requests
{
	[DataContract]
	public class DeleteReadingListEntry : ICommand<bool>
	{
		public DeleteReadingListEntry(int key)
		{
			Key = key;
		}

		[DataMember]
		public int Key { get; private set; }
	}
}