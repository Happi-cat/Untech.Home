using System.Runtime.Serialization;

namespace Untech.ReadingList.Domain.Views
{
	[DataContract]
	public class AuthorsViewItem
	{
		public AuthorsViewItem(string author)
		{
			Author = author;
		}

		[DataMember]
		public string Author { get; private set; }

		[DataMember]
		public int CompletedBooksCount { get; set; }

		[DataMember]
		public int ReadingBooksCount { get; set; }

		[DataMember]
		public int TotalBooksCount { get; set; }
	}
}