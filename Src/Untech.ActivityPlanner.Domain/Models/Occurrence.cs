using System;

namespace Untech.Home.ActivityPlanner.Domain.Models
{
	public class Occurrence
	{
		public DateTime When { get; set; }
		public bool Outdated => When.Date < DateTime.Today;
		public string Remarks { get; set; }
	}
}
