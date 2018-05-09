using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace Untech.ReadingList.Domain.Requests
{
	[DataContract]
	public class CompleteReadingBook : ICommand
	{
		public CompleteReadingBook(int readingListEntryKey)
		{
			ReadingListEntryKey = readingListEntryKey;
		}

		[DataMember]
		public int ReadingListEntryKey { get; private set; }
	}
}