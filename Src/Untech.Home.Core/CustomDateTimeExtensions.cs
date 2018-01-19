using System;

namespace Untech.Home
{
	public static class CustomDateTimeExtensions
	{
		public static DateTime AsMonthDate(this DateTime date) => new DateTime(date.Year, date.Month, 1);

		public static DateTime AsMonthDate(this IHasMonthInfo info) => new DateTime(info.Year, info.Month, 1);

		public static DateTime AsDayDate(this IHasDayInfo info) => new DateTime(info.Year, info.Month, info.Day);
	}
}