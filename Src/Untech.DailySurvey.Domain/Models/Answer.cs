using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.Practices.DataStorage;

namespace Untech.DailySurvey.Domain.Models
{
	[DataContract]
	public class Answer : IAggregateRoot
	{
		public Answer(int key, int questionKey, DateTime when, IReadOnlyCollection<string> selectedOptions)
		{
			Key = key;
			QuestionKey = questionKey;
			When = when;
			SelectedOptions = selectedOptions;
		}

		[DataMember] public int Key { get; private set; }

		[DataMember] public int QuestionKey { get; private set; }

		[DataMember] public DateTime When { get; private set; }

		[DataMember] public IReadOnlyCollection<string> SelectedOptions { get; private set; }
	}
}