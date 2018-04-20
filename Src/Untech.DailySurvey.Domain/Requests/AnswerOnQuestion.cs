using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace Untech.DailySurvey.Domain.Requests
{
	[DataContract]
	public class AnswerOnQuestion : ICommand
	{
		public AnswerOnQuestion(int questionKey, string selectedAnswer)
		{
			QuestionKey = questionKey;
			SelectedAnswer = selectedAnswer;
		}

		[DataMember]
		public int QuestionKey { get; private set; }

		[DataMember]
		public string SelectedAnswer { get; private set; }
	}
}