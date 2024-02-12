using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Modul11.Telebot.Configuration;
using Modul11.Telebot.Services;
using Telegram.Bot.Types.Enums;

namespace Modul11.Telebot.Controllers
{
    public class InlineKeyboardController
    {
        private readonly ITelegramBotClient _telegramClient;
        private ILogger logger;
        private readonly IStorage _memoryStorage;

        public InlineKeyboardController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
            logger = new ConsoleLogger(false);
        }

        public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
        {
            logger.Info($"Контроллер {GetType().Name} обнаружил нажатие на кнопку { callbackQuery.Data }");
            //await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id, $"Обнаружено нажатие на кнопку {callbackQuery.Data}", cancellationToken: ct);

            if (callbackQuery?.Data == null)
                return;

            // Обновление пользовательской сессии новыми данными
            _memoryStorage.GetSession(callbackQuery.From.Id).LanguageCode = callbackQuery.Data;

            // Генерим информационное сообщение
            string languageText = callbackQuery.Data switch
            {
                "ru" => " Русский",
                "en" => " Английский",
                _ => String.Empty
            };

            // Отправляем в ответ уведомление о выборе
            await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
                $"<b>Язык аудио - {languageText}.{Environment.NewLine}</b>" +
                $"{Environment.NewLine}Можно поменять в главном меню.", cancellationToken: ct, parseMode: ParseMode.Html);
        }
    }
}
