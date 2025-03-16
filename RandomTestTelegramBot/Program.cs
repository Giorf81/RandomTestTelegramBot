using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RandomTestTelegramBot;
using Telegram.Bot;

namespace RandomTestTelegramBot
{
    internal class Program
    {
        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            var host = new HostBuilder().ConfigureServices((hostContext, services) => ConfigureServices(services)).UseConsoleLifetime().Build();

            Console.WriteLine("Сервис запущен!");

            await host.RunAsync();
            Console.WriteLine("Сервис остановлен!");
        }

        static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient("7123537005:AAEsa7x7NAdP_sQMfRwHvsOcoK5IZmI8T8s"));
            services.AddHostedService<Bot>();
        }
    }
}
