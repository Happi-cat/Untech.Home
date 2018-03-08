using System.Runtime.Serialization;
using Untech.ActivityPlanner.Domain.Models;

namespace Untech.ActivityPlanner.Domain.Views
{
	[DataContract]
	public class MonthlyCalendarMonthActivity
	{
		public MonthlyCalendarMonthActivity(int activityKey, int count)
		{
			ActivityKey = activityKey;
			Count = count;
		}

		[DataMember]
		public int ActivityKey { get; private set; }

		[DataMember]
		public int Count { get; private set; }
	}
}