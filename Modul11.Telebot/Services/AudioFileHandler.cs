using Modul11.Telebot.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Modul11.Telebot.Utilities;

namespace Modul11.Telebot.Services
{
    public class AudioFileHandler : IFileHandler
    {
        private readonly AppSettings _appSettings;
        private readonly ITelegramBotClient _telegramBotClient;
        ILogger logger;

        public AudioFileHandler(ITelegramBotClient telegramBotClient, AppSettings appSettings)
        {
            _appSettings = appSettings;
            _telegramBotClient = telegramBotClient;
            logger = new ConsoleLogger(false);
        }

        public async Task Download(string fileId, CancellationToken ct)
        {
            // Генерируем полный путь файла из конфигурации
            string inputAudioFilePath = Path.Combine(_appSettings.DownloadsFolder, $"{_appSettings.AudioFileName}.{_appSettings.InputAudioFormat}");

            using (FileStream destinationStream = File.Create(inputAudioFilePath))
            {
                // Загружаем информацию о файле
                var file = await _telegramBotClient.GetFileAsync(fileId, ct);
                if (file.FilePath == null)
                    return;

                // Скачиваем файл
                await _telegramBotClient.DownloadFileAsync(file.FilePath, destinationStream, ct);
            }
        }

        public string Process(string languageCode)
        {
            string inputAudioPath = Path.Combine(_appSettings.DownloadsFolder, $"{_appSettings.AudioFileName}.{_appSettings.InputAudioFormat}");
            string outputAudioPath = Path.Combine(_appSettings.DownloadsFolder, $"{_appSettings.AudioFileName}.{_appSettings.OutputAudioFormat}");

            logger.Info("Начинаем конвертацию...");
            AudioConverter.TryConvert(inputAudioPath, outputAudioPath);
            logger.GoodEvent("Файл конвертирован");

            logger.Info("Начинаем распознавание...");
            var speechText = SpeechDetector.DetectSpeech(outputAudioPath, _appSettings.AudioBitrate, languageCode);
            logger.GoodEvent("Файл распознан.");
            return speechText;
        }
    }
}
