using System;
using Untech.ActivityPlanner.Domain.Views;

namespace Untech.ActivityPlanner.Domain.Services
{
	internal static class CustomDateTimeExtensions
	{
		public static DateTime AsMonth(this DateTime date) => new DateTime(date.Year, date.Month, 1);

		public static DateTime AsMonth(this IHasMonthInfo info) => new DateTime(info.Year, info.Month, 1);
	}
}