using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sticky.Application.ResponseUpdater.Services;
using System.IO;
using System.Threading.Tasks;

namespace Sticky.Application.ResponseUpdater;

class Program
{
    //You should always run at least 3 kinds of response updater one for each type of response.(Category,Product and Query)
    static void Main()
    {
        MainAsync().GetAwaiter().GetResult();

    }
    private static async Task MainAsync()
    {
        var setting = GetConfiguration();

        var responseUpdater = new ServiceBuilder(setting)
            .BuildCollection()
            .BuildServiceProvider()
            .GetService<IResponseUpdater>();

        await responseUpdater.DoAsync();

    }

    private static Setting GetConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
        var conf = builder.Build();
        return conf.Get<Setting>();
    }

}

