using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace Untech.DailySurvey.Domain.Requests
{
	[DataContract]
	public class AnswerOnQuestion : ICommand
	{
		public AnswerOnQuestion(int questionKey, IReadOnlyCollection<string> selectedOptions)
		{
			QuestionKey = questionKey;
			SelectedOptions = selectedOptions;
		}

		[DataMember] public int QuestionKey { get; private set; }

		[DataMember] public IReadOnlyCollection<string> SelectedOptions { get; private set; }
	}
}