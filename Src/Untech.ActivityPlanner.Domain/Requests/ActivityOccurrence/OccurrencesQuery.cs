using System;
using System.Collections.Generic;
using Untech.Practices.CQRS;

namespace Untech.ActivityPlanner.Domain.Requests.ActivityOccurrence
{
	public class OccurrencesQuery : IQuery<IEnumerable<Models.ActivityOccurrence>>
	{
		public OccurrencesQuery(DateTime from, DateTime to)
		{
			From = from;
			To = to;
		}

		public OccurrencesQuery(DateTime from, TimeSpan range)
		{
			From = from;
			To = from + range;
		}

		public DateTime From { get; }

		public DateTime To { get; }
	}
}