using System.Runtime.Serialization;

namespace Untech.ReadingList.Domain.Views
{
	[DataContract]
	public class ReadingStatistics
	{
		[DataMember]
		public int CompletedThisYearBooksCount { get; set; }

		[DataMember]
		public int CompletedThisQuarterBooksCount { get; set; }

		[DataMember]
		public int CompletedThisMonthBooksCount { get; set; }

		[DataMember]
		public int CompletedBooksCount { get; set; }

		[DataMember]
		public int ReadingBooksCount { get; set; }

		[DataMember]
		public int TotalBooksCount { get; set; }
	}
}