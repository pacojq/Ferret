using System.Runtime.CompilerServices;
using NLog;
using NLog.Layouts;
using NLog.Targets;

namespace FerretEngine.Logging
{
    /// <summary>
    /// Logging class that uses NLog https://github.com/NLog/NLog
    /// </summary>
    public static class FeLog
    {
        private static Logger _logger;
        
        internal static void Initialize()
        {
            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            //var logfile = new FileTarget("logfile") { FileName = "file.txt" };
            var logconsole = new ColoredConsoleTarget("logconsole");
            
            //logfile.Layout = Layout.FromString("${longdate}| ${level:uppercase=true}\t${message}");
            logconsole.Header = Layout.FromString("${longdate} | Welcome to FerretEngine");
            logconsole.Layout = Layout.FromString("${date} | [${level:uppercase=true}]\t${message}");
            
            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            //config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);
            
            // Apply config           
            LogManager.Configuration = config;
			
            _logger = LogManager.GetCurrentClassLogger();
        }
        
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Debug(string msg)
        {
            _logger.Debug(msg);
        }
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Info(string msg)
        {
            _logger.Info(msg);
        }
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Warning(string msg)
        {
            _logger.Warn(msg);
        }
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Error(string msg)
        {
            _logger.Error(msg);
        }
    }
}