using System.Runtime.Serialization;
using Untech.Practices.CQRS;
using Untech.Users.Domain.Models;

namespace Untech.Users.Domain.Requests
{
	[DataContract]
	public class CreateUser : ICommand<User>
	{
		[DataMember]
		public int? TelegramId { get; set; }
	}
}