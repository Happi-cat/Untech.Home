using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.DailySurvey.Domain.Models;

namespace Untech.DailySurvey.Domain.Views
{
	[DataContract]
	public class SurveyResultQuestion
	{
		[DataMember]
		public Question Question { get; set; }

		[DataMember]
		public IReadOnlyCollection<SurveyResultAnswer> Answers { get; set; }
	}
}