using Untech.Practices.CQRS;

namespace Untech.Users.Domain.Requests
{
	public class FindUserByTelegramId : IQuery<User>
	{
		public int TelegramId { get; set; }
	}
}