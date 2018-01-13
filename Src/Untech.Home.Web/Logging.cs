using Microsoft.Extensions.Logging;

namespace Untech.Home.Web
{
	public class Logging
	{
		private static ILoggerFactory s_factory;

		public static ILoggerFactory ConfigureLogger(ILoggerFactory factory) => factory
			.AddDebug(LogLevel.None)
			.AddConsole();

		public static ILoggerFactory LoggerFactory
		{
			get => s_factory ?? (s_factory = ConfigureLogger(new LoggerFactory()));
			set => s_factory = value;
		}
	}
}