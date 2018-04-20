using System.Runtime.Serialization;
using Untech.DailySurvey.Domain.Models;
using Untech.Practices.CQRS;

namespace Untech.DailySurvey.Domain.Requests
{
	[DataContract]
	public class MarkQuestionAsObsolete : ICommand<Question>
	{
		public MarkQuestionAsObsolete(int key)
		{

		}
	}
}