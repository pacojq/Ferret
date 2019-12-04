using System.Runtime.CompilerServices;
using NLog;
using NLog.Config;
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
        private static Logger _ferretLogger;
        
        internal static void Initialize()
        {
            // Targets where to log to: File and Console
            //var logfile = new FileTarget("logfile") { FileName = "file.txt" };
            
            // Application logger
            ColoredConsoleTarget appConsole = new ColoredConsoleTarget("logconsole");
            appConsole.Header = Layout.FromString("${date} | Welcome to FerretEngine\n");
            appConsole.Layout = Layout.FromString("${date} | App    [${level:uppercase=true}]\t${message}");
            
            
            // Internal Logger
            ColoredConsoleTarget internalConsole = new ColoredConsoleTarget("internalConsole");
            internalConsole.Layout = Layout.FromString("${date} | Ferret [${level:uppercase=true}]\t${message}");
            
            
            
            LoggingConfiguration config = new NLog.Config.LoggingConfiguration();
#if DEBUG
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, appConsole, "Application");
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, internalConsole, "Ferret");
#else
            config.AddRule(LogLevel.Info, LogLevel.Fatal, appConsole, "Application");
            config.AddRule(LogLevel.Info, LogLevel.Fatal, internalConsole, "Ferret");
#endif
            //config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile); LOG TO FILE
            
            LogManager.Configuration = config;// Apply config 
            _logger = LogManager.GetLogger("Application");
            _ferretLogger = LogManager.GetLogger("Ferret");
        }
        
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Debug(string msg)
        {
            _logger.Debug(msg);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void FerretDebug(string msg)
        {
            _ferretLogger.Debug(msg);
        }
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Info(string msg)
        {
            _logger.Info(msg);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void FerretInfo(string msg)
        {
            _ferretLogger.Info(msg);
        }
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Warning(string msg)
        {
            _logger.Warn(msg);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void FerretWarning(string msg)
        {
            _ferretLogger.Warn(msg);
        }
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Error(string msg)
        {
            _logger.Error(msg);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void FerretError(string msg)
        {
            _ferretLogger.Error(msg);
        }
    }
}