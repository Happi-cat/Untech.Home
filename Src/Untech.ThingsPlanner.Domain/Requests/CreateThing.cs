using System.Runtime.Serialization;
using Untech.Practices.CQRS;
using Untech.ThingsPlanner.Domain.Models;

namespace Untech.ThingsPlanner.Domain.Requests
{
	[DataContract]
	public class CreateIdea : ICommand<Thing.Idea>
	{
		public CreateIdea(string title)
		{
			Title = title;
		}

		[DataMember]
		public string Title { get; private set; }
	}
}