using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.DailySurvey.Domain.Models;
using Untech.Practices.CQRS;

namespace Untech.DailySurvey.Domain.Requests
{
	[DataContract]
	public class AnswersQuery : IQuery<IEnumerable<Answer>>
	{
		public AnswersQuery(DateTime from, DateTime to)
		{
			From = @from;
			To = to;
		}

		[DataMember] public DateTime From { get; private set; }

		[DataMember] public DateTime To { get; private set; }
	}
}