using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using Modul11.Telebot.Configuration;
using Modul11.Telebot.Services;
using Modul11.Telebot.Models;

namespace Modul11.Telebot.Controllers
{
    public class VoiceMessageController
    {
        private readonly AppSettings _appSettings;
        private readonly ITelegramBotClient _telegramClient;
        private readonly IFileHandler _audioFileHandler;
        private readonly IStorage _memoryStorage;
        private ILogger logger;

        public VoiceMessageController(AppSettings appSettings, ITelegramBotClient telegramBotClient, IFileHandler audioFileHandler, IStorage memoryStorage)
        {
            _appSettings = appSettings;
            _telegramClient = telegramBotClient;
            _audioFileHandler = audioFileHandler;
            logger = new ConsoleLogger(false);
            _memoryStorage = memoryStorage;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            logger.Info($"Контроллер {GetType().Name} получил голосовое сообщение");

            var fileId = message.Voice?.FileId;
            if (fileId == null)
                return;

            await _audioFileHandler.Download(fileId, ct);
            await _telegramClient.SendTextMessageAsync(message.Chat.Id, "Голосовое сообщение получено, приступаем к распознаванию", cancellationToken: ct);
            string userLanguageCode = _memoryStorage.GetSession(message.Chat.Id).LanguageCode;

            var result = _audioFileHandler.Process(userLanguageCode); // Запустим обработку
            logger.Info($"Результат распознавания голосового сообщения:");
            if (string.IsNullOrEmpty(result))
            {
                result = "Ошибка распознавания текста";
                logger.Error(result);
            } else
            {
                logger.SomeEvent(result);
            }
            await _telegramClient.SendTextMessageAsync(message.Chat.Id, result, cancellationToken: ct);
        }
    }
}
