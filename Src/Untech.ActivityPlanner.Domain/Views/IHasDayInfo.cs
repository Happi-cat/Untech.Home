namespace Untech.ActivityPlanner.Domain.Views
{
	public interface IHasDayInfo : IHasMonthInfo
	{
		int Day { get; }
	}
}