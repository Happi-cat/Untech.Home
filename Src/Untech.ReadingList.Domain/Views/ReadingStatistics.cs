using System.Runtime.Serialization;

namespace Untech.ReadingList.Domain.Views
{
	[DataContract]
	public class ReadingStatistics
	{
		[DataMember]
		public int CompletedThisYear { get; set; }

		[DataMember]
		public int CompletedThisQuarter { get; set; }

		[DataMember]
		public int CompletedThisMonth { get; set; }
	}
}