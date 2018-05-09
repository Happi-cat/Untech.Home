using System.Runtime.Serialization;
using Untech.DailySurvey.Domain.Views;
using Untech.Practices.CQRS;

namespace Untech.DailySurvey.Domain.Requests
{
	[DataContract]
	public class SurveyResultQuery : IQuery<SurveyResult>
	{
		public SurveyResultQuery()
		{
			NumberOfLastDays = 7;
		}

		[DataMember]
		public int NumberOfLastDays { get; set; }
	}
}