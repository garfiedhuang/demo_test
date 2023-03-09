using NLog;
using NLog.Web;

namespace Hkust.Cus.WebApi;

internal static class Program
{
    internal static async Task Main(string[] args)
    {
        var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
        logger.Debug($"init {nameof(Program.Main)}");
        try
        {
            var webApiAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            var serviceInfo = Common.WebApi.ServiceInfo.CreateInstance(webApiAssembly);

            var app = WebApplication
                .CreateBuilder(args)
                .ConfigureHkustDefault(serviceInfo)
                .Build();

            app.UseHkust();

            await app
                .ChangeThreadPoolSettings()
                .UseRegistrationCenter()
                .RunAsync();
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Stopped program because of exception");
            throw;
        }
        finally
        {
            LogManager.Shutdown();
        }
    }
}