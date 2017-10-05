using System;

namespace Untech.FinancePlanner.Domain.Storage
{
	public interface ICacheStorage
	{
		T Get<T>(string key);

		void Set(string key, object value);

		void Drop(string key);
	}
}