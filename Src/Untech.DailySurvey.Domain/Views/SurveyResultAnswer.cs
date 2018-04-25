using System.Runtime.Serialization;
using Untech.Home;

namespace Untech.DailySurvey.Domain.Views
{
	[DataContract]
	public class SurveyResultAnswer : IHasDayInfo
	{
		[DataMember] public int Year { get; }

		[DataMember] public int Month { get; }

		[DataMember] public int Day { get; }

		[DataMember] public string Answer { get; set; }
	}
}