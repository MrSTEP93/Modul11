using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using Modul11.Telebot.Configuration;


namespace Modul11.Telebot.Controllers
{
    internal class DefaultMessageController
    {
        private readonly ITelegramBotClient _telegramClient;

        private ILogger logger;

        public DefaultMessageController(ITelegramBotClient telegramBotClient)
        {
            _telegramClient = telegramBotClient;
            logger = new ConsoleLogger(false);
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            logger.Info($"Контроллер {GetType().Name} получил сообщение");
            await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Получено сообщение неподдерживаемого формата", cancellationToken: ct);
        }
    }
}
