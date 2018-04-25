using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.DailySurvey.Domain.Models;

namespace Untech.DailySurvey.Domain.Views
{
	[DataContract]
	public class SurveyResultQuestion
	{
		private SurveyResultQuestion()
		{
		}

		public SurveyResultQuestion(Question question)
		{
			Question = question;
		}

		[DataMember] public Question Question { get; private set; }

		[DataMember] public IReadOnlyCollection<SurveyResultAnswer> Answers { get; set; }
	}
}