using NLog.Web;

namespace IntelliCRMAPIService
{
    public class Program
    {
        public static void Main(string[] args)
        {
           var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                logger.Error($"Error occurred--{ex.Message}");
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;

                    config.AddEnvironmentVariables();
                    config.SetBasePath(env.ContentRootPath)
                          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

                    if (args != null)
                    {
                        config.AddCommandLine(args);
                    }
                }).ConfigureLogging(logging => {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
                }).UseNLog();
    }
}


