using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Modul11.Telebot.Controllers;
using Modul11.Telebot.Services;
using Modul11.Telebot.Configuration;
using System.Runtime;

namespace Modul11.Telebot
{
    internal class Program
    {
        static ILogger log = new ConsoleLogger(false);
        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;
            log.Info("Мой телеграм-бот");
            // Объект, отвечающий за постоянный жизненный цикл приложения
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services)) // Задаем конфигурацию
                .UseConsoleLifetime() // Позволяет поддерживать приложение активным в консоли
                .Build(); // Собираем

            log.GoodEvent("Запуск сервиса");
            // Запускаем сервис
            await host.RunAsync();
            log.SomeEvent("Сервис остановлен");
        }

        static AppSettings BuildAppSettings()
        {
            return new AppSettings()
            {
                BotToken = "6970731421:AAGJ3U6SaaDH_2EA_kfxMfQ9D9UYHRH28cc",
                DownloadsFolder = "C:\\Users\\user\\Downloads\\BOT",
                AudioFileName = "audio",
                InputAudioFormat = "ogg",
                OutputAudioFormat = "wav",
                AudioBitrate = 48000,
            };
        }

        static void ConfigureServices(IServiceCollection services)
        {
            // Подключаем конфигурацию
            AppSettings appSettings = BuildAppSettings();
            services.AddSingleton(BuildAppSettings());
            // Подключаем контроллеры сообщений и кнопок
            services.AddTransient<DefaultMessageController>();
            services.AddTransient<VoiceMessageController>();
            services.AddTransient<TextMessageController>();
            services.AddTransient<InlineKeyboardController>();
            // Подключаем хранилище
            services.AddSingleton<IStorage, MemoryStorage>();
            // Подключаем обработчик файлов
            services.AddSingleton<IFileHandler, AudioFileHandler>();

            // Регистрируем объект TelegramBotClient c токеном подключения
            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(appSettings.BotToken));
            // Регистрируем постоянно активный сервис бота
            services.AddHostedService<Bot>();
        }
    }
}
