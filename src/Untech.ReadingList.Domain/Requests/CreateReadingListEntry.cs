using System.Runtime.Serialization;
using Untech.Practices.CQRS;
using Untech.ReadingList.Domain.Models;

namespace Untech.ReadingList.Domain.Requests
{
	[DataContract]
	public class CreateReadingListEntry : ICommand<ReadingListEntry>
	{
		public CreateReadingListEntry(string author, string title)
		{
			Author = author;
			Title = title;
		}

		[DataMember]
		public string Author { get; private set; }

		[DataMember]
		public string Title { get; private set; }
	}
}