using System.Collections.Generic;
using Untech.Practices.CQRS;
using Untech.ThingsPlanner.Domain.Models;

namespace Untech.ThingsPlanner.Domain.Requests
{
	public class ThingQuery : IQuery<Thing>
	{

	}

	public class ThingsQuery : IQuery<IEnumerable<Thing>>
	{
		
	}
}