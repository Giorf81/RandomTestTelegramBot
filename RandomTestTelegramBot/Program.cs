using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyTelegramBot.Controllers;
using MyTelegramBot.Services;
using RandomTestTelegramBot;
using RandomTestTelegramBot.Controllers;
using Telegram.Bot;

namespace RandomTestTelegramBot
{
    static class Program
    {
        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services))
                .UseConsoleLifetime()
                .Build();

            Console.WriteLine("Starting Service");
            await host.RunAsync();
            Console.WriteLine("Service stopped");
        }

        static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient("7123537005:AAHEIibv9IsIYrleWHAodTMw90UesSJC2oU"));
            services.AddHostedService<Bot>();
            services.AddTransient<AnyMessageContoller>();
            services.AddTransient<InlineKeyboardController>();
            services.AddSingleton<IStorage, MemoryStorage>();
        }
    }
}
