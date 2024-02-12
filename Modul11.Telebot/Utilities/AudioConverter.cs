using FFMpegCore;
using Modul11.Telebot.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modul11.Telebot.Utilities
{
    public static class AudioConverter
    {
        public static void TryConvert(string inputFile, string outputFile)
        {
            // Задаём путь, где лежит вспомогательная программа - конвертер
            GlobalFFOptions.Configure(options => options.BinaryFolder = Path.Combine(DirectoryExtension.GetSolutionRoot(), "ffmpeg-win64", "bin"));

            // Вызываем Ffmpeg, передав требуемые аргументы.
            FFMpegArguments
              .FromFileInput(inputFile)
              .OutputToFile(outputFile, true, options => options
                .WithFastStart())
              .ProcessSynchronously();
        }
    }
}
