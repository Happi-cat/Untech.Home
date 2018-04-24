﻿using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.DailySurvey.Domain.Models;
using Untech.Practices.CQRS;

namespace Untech.DailySurvey.Domain.Requests
{
	[DataContract]
	public class CreateQuestion : ICommand<Question>
	{
		public CreateQuestion(string title)
		{
			Title = title;
		}

		[DataMember]
		public string Title { get; private set; }

		[DataMember]
		public QuestionPeriodicity Periodicity { get; set; }

		[DataMember]
		public QuestionType Type { get; set; }

		[DataMember]
		public IReadOnlyCollection<string> Options { get; set; }
	}
}