using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.Practices.CQRS;
using Untech.ReadingList.Domain.Models;

namespace Untech.ReadingList.Domain.Requests
{
	[DataContract]
	public class AuthorsBooksQuery : IQuery<IEnumerable<ReadingListEntry>>
	{
		public AuthorsBooksQuery(string author)
		{
			Author = author;
		}

		[DataMember]
		public string Author { get; }
	}
}