﻿using System.Runtime.Serialization;
using Untech.Practices.DataStorage;

namespace Untech.Users.Domain.Models
{
	[DataContract]
	public class User : IAggregateRoot
	{
		[DataMember] public int Key { get; private set; }

		[DataMember] public int? TelegramId { get; set; }
	}
}