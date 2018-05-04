using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.Practices.CQRS;
using Untech.ReadingList.Domain.Models;

namespace Untech.ReadingList.Domain.Requests
{
	[DataContract]
	public class ReadingListQuery : IQuery<IEnumerable<ReadingListEntry>>
	{
		[DataMember]
		public ReadingListEntryStatus? Status { get; set; }
	}
}