using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Untech.DailySurvey.Domain.Views
{
	[DataContract]
	public class SurveyResult
	{
		[DataMember]
		public IReadOnlyCollection<SurveyResultQuestion> Questions { get; set; }
	}
}