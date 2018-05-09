namespace Untech.Home.Data
{
	public interface IConnectionStringFactory
	{
		string GetConnectionString(string dbName);
	}
}