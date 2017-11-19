using System.Runtime.Serialization;
using Untech.Practices;

namespace Untech.FinancePlanner.Domain.ViewModels
{
	[DataContract]
	public class MonthlyFinancialReportDayEntry
	{
		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string Remarks { get; set; }

		[DataMember]
		public Money Actual { get; set; }

		[DataMember]
		public Money Forecasted { get; set; }
	}
}