using System.Runtime.Serialization;
using Untech.Practices.CQRS;
using Untech.Users.Domain.Models;

namespace Untech.Users.Domain.Requests
{
	[DataContract]
	public class FindUserByTelegramId : IQuery<User>
	{
		public FindUserByTelegramId(int telegramId)
		{
			TelegramId = telegramId;
		}

		[DataMember]
		public int TelegramId { get; private set; }
	}
}