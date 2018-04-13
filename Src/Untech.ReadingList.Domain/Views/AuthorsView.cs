using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Untech.ReadingList.Domain.Views
{
	[DataContract]
	public class AuthorsView
	{
		[DataMember]
		public IReadOnlyCollection<AuthorsViewItem> Auhtors { get; set; }
	}
}