namespace Untech.FinancePlanner.Domain.Cache
{
	public interface ICacheManager
	{
		T Get<T>(string key);

		void Set(string key, object value);

		void Drop(string key);
	}
}