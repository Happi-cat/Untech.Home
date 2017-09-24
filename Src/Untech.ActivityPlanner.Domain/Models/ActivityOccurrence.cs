using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.Practices;

namespace Untech.ActivityPlanner.Domain.Models
{
	[DataContract]
	public class ActivityOccurrence : ValueObject<ActivityOccurrence>
	{
		public ActivityOccurrence(DateTime thatDay)
		{
			When = thatDay.Date;
		}

		public static implicit operator ActivityOccurrence(DateTime thatDay)
		{
			return new ActivityOccurrence(thatDay);
		}

		public static implicit operator DateTime(ActivityOccurrence occurrence)
		{
			return occurrence.When;
		}

		[DataMember]
		public DateTime When { get; private set; }

		[DataMember]
		public bool IsPast => When.Date < DateTime.Today;

		[DataMember]
		public bool IsToday => When.Date == DateTime.Today;

		protected override IEnumerable<object> GetEquatableProperties()
		{
			yield return When;
		}
	}
}
