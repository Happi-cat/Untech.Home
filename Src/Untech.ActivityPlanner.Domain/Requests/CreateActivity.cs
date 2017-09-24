using System.Runtime.Serialization;
using Untech.ActivityPlanner.Domain.Models;
using Untech.Practices.CQRS;

namespace Untech.ActivityPlanner.Domain.Requests
{
	[DataContract]
	public class CreateActivity : ICommand<Activity>
	{
		public CreateActivity(int groupId, string name)
		{
			GroupId = groupId;
			Name = name;
		}

		[DataMember]
		public string Name { get; private set; }

		[DataMember]
		public string Remarks { get; set; }

		[DataMember]
		public int GroupId { get; private set; }
	}
}
