using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace Untech.DailySurvey.Domain.Requests
{
	[DataContract]
	public class AnswerOnQuestion : ICommand
	{
		public AnswerOnQuestion(int questionKey, IReadOnlyCollection<string> selectedAnswers)
		{
			QuestionKey = questionKey;
			SelectedAnswers = selectedAnswers;
		}

		[DataMember]
		public int QuestionKey { get; private set; }

		[DataMember]
		public IReadOnlyCollection<string> SelectedAnswers { get; private set; }
	}
}