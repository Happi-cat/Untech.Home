using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.Practices.DataStorage;

namespace Untech.DailySurvey.Domain.Models
{
	[DataContract]
	public class Question : IAggregateRoot
	{
		public Question(int key, string text)
		{
			Key = key;
			Text = text;
		}

		[DataMember] public int Key { get; private set; }

		[DataMember] public string Text { get; private set; }

		[DataMember] public QuestionPeriodicity Periodicity { get; set; }

		[DataMember] public QuestionType Type { get; set; }

		[DataMember] public bool IsObsolete { get; set; }

		[DataMember] public IReadOnlyCollection<string> Options { get; set; }
	}
}