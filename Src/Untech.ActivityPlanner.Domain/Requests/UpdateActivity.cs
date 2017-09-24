using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace Untech.ActivityPlanner.Domain.Requests
{
	[DataContract]
	public class UpdateActivity : ICommand
	{
		public UpdateActivity(int id, string name)
		{
			Id = id;
			Name = name;
		}

		[DataMember]
		public int Id { get; private set; }

		[DataMember]
		public string Name { get; private set; }

		[DataMember]
		public string Remarks { get; set; }
	}
}
