using System;
using System.Collections.Generic;
using Untech.ActivityPlanner.Domain.Models;
using Untech.Practices.CQRS;

namespace Untech.ActivityPlanner.Domain.Requests
{
	public class OccurrencesQuery : IQuery<IEnumerable<ActivityOccurrence>>
	{
		public OccurrencesQuery(DateTime from, DateTime to)
		{
			From = from;
			To = to;
		}

		public DateTime From { get; private set; }

		public DateTime To { get; private set; }
	}
}