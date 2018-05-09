namespace Untech.Home
{
	public interface IHasDayInfo : IHasMonthInfo
	{
		int Day { get; }
	}
}