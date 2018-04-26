using System;
using System.Runtime.Serialization;

namespace Untech.ThingsPlanner.Domain.Models
{
	public abstract partial class Thing
	{
		[DataContract]
		public class DelegatedProject : Thing
		{
			public DelegatedProject(int key, string title) : base(ThingType.DelegatedProject, key, title)
			{
			}

			[DataMember]
			public DateTime? FollowUp { get; set; }

			[DataMember]
			public string Person { get; set; }
		}
	}
}