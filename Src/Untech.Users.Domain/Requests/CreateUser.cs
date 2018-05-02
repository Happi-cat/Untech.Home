using Untech.Practices.CQRS;

namespace Untech.Users.Domain.Requests
{
	public class CreateUser : ICommand<User>
	{
		public int? TelegramId { get; set; }
	}
}