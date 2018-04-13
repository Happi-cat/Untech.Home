using System;
using System.Runtime.Serialization;
using Untech.Practices.DataStorage;

namespace Untech.DailySurvey.Domain.Models
{
	[DataContract]
	public class Answer : IAggregateRoot
	{
		public Answer(int key, int questionKey, DateTime when, string value)
		{
			Key = key;
			QuestionKey = questionKey;
			When = when;
			Value = value;
		}

		[DataMember]
		public int Key { get; private set; }

		[DataMember]
		public int QuestionKey { get; private set; }

		[DataMember]
		public DateTime When { get; private set; }

		[DataMember]
		public string Value { get; private set; }
	}
}