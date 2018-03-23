using System;
using Untech.Practices.DataStorage;

namespace Untech.ThingsPlanner.Domain.Model
{
	public class Thing : IAggregateRoot
	{
		public int Key { get; }

		public string Title { get; }

	}

}