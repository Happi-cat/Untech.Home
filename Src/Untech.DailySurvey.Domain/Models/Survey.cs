using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.Practices.DataStorage;

namespace Untech.DailySurvey.Domain.Models
{
	[DataContract]
	public class SurveyQuestion : IAggregateRoot
	{
		public SurveyQuestion(int key, string question)
		{
			Key = key;
			Question = question;
		}

		[DataMember]
		public int Key { get; private set; }

		[DataMember]
		public string Question { get; private set; }

		[DataMember]
		public bool IsClosedQuestion { get; set; }

		[DataMember]
		public bool IsActual { get; set; }

		[DataMember]
		public IReadOnlyCollection<string> Options { get; set; }
	}

	public class SurveyQuestionResult : IAggregateRoot
	{
		public int Key { get; private set; }

		public int QuestionKey { get; private set; }

		public DateTime When { get; private set; }

		public string Answer { get; set; }
	}

	public class QuestionResult
	{
		public int QuestionKey { get; set; }

		public string Answer { get; set; }
	}
}