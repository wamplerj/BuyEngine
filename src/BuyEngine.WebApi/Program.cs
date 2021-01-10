using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace BuyEngine.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                LogManager.Configuration = ConfigureNLog();
                CreateHostBuilder(args).Build().Run();
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }

        public static LoggingConfiguration ConfigureNLog()
        {
            var loggingConfig = new LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new FileTarget("logfile")
            {
                FileName = "../../../../logs/BuyEngine.WebApi/log.txt",
                ArchiveFileName = "log-{#}.txt",
                ArchiveNumbering = ArchiveNumberingMode.Date,
                ArchiveDateFormat = "yyyyMMdd-HH",
                Layout = "${longDate}|${level:uppercase=true}|${logger}|${callsite}|${message}"
            };
            var logconsole = new ConsoleTarget("logconsole");

            // Rules for mapping loggers to targets            
            loggingConfig.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            loggingConfig.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            return loggingConfig;
        }
    }
}