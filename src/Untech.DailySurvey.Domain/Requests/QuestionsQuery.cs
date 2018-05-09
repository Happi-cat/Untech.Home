using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.DailySurvey.Domain.Models;
using Untech.Practices.CQRS;

namespace Untech.DailySurvey.Domain.Requests
{
	[DataContract]
	public class QuestionsQuery : IQuery<IEnumerable<Question>>
	{
		[DataMember]
		public bool IncludeObsolete { get; set; }
	}
}