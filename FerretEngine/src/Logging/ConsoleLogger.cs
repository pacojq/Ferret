using System;

namespace FerretEngine.Logging
{
	public class ConsoleLogger : ILogger
	{
		public void Log(string msg)
		{
			Console.WriteLine(msg);
		}
	}
}