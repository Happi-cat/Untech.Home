using System.Runtime.Serialization;
using Untech.Practices.CQRS;
using Untech.ReadingList.Domain.Views;

namespace Untech.ReadingList.Domain.Requests
{
	[DataContract]
	public class AuthorsQuery : IQuery<AuthorsView>
	{
	}
}