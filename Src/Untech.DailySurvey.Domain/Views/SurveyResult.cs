using System.Collections.Generic;
using Untech.DailySurvey.Domain.Models;
using Untech.Home;

namespace Untech.DailySurvey.Domain.Views
{
	public class SurveyResult
	{
		public IReadOnlyCollection<SurveyResultQuestion> Questions { get; set; }
	}

	public class SurveyResultQuestion
	{
		public Question Question { get; set; }
		public IReadOnlyCollection<SurveyResultAnswer> Answers { get; set; }
	}

	public class SurveyResultAnswer : IHasDayInfo
	{
		public int Year { get; }
		public int Month { get; }
		public int Day { get; }
		public string Answer { get; set; }
	}
}