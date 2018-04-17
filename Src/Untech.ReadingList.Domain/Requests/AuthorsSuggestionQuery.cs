using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace Untech.ReadingList.Domain.Requests
{
	[DataContract]
	public class AuthorsSuggestionQuery : IQuery<IEnumerable<string>>
	{
		public AuthorsSuggestionQuery(string searchString)
		{
			SearchString = searchString;
		}
		
		[DataMember]
		public string SearchString { get; }
	}
}