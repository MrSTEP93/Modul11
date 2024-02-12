using System;
using static ConsoleHelper_50.Helper_50;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Telegram.Bot.Types;

namespace Modul11.Telebot
{
    public interface ILogger
    {
        void Info(string message);
        void SomeEvent(string message);
        void GoodEvent(string message);
        void Error(string message);
    }

    public class ConsoleLogger : ILogger
    {
        bool isDateNeed;

        private string PrintDate(ref string message)
        {
            if (isDateNeed)
            {
                message += DateTime.Now + ": ";
            }
            return message;
        }

        public ConsoleLogger(bool _isDateNeed)
        {
            this.isDateNeed = _isDateNeed;
        }

        public void Info(string message)
        {
            PrintDate(ref message);
            WriteLn(message, ConsoleColor.White);
        }

        public void SomeEvent(string message)
        {
            PrintDate(ref message);
            WriteLn(message, ConsoleColor.Blue);
        }

        public void GoodEvent(string message)
        {
            PrintDate(ref message);
            WriteLn(message, ConsoleColor.Green);
        }

        public void Error(string message)
        {
            PrintDate(ref message);
            WriteLn(message, ConsoleColor.Red);
        }
    }

    public class FileLogger : ILogger
    {
        private FileInfo logFile;

        public FileLogger(string logFilePath)
        {
            logFile = new FileInfo(logFilePath);
        }

        public void Info(string message)
        {
            throw new NotImplementedException();
        }

        public void SomeEvent(string message)
        {
            throw new NotImplementedException();
        }

        public void GoodEvent(string message)
        {
            throw new NotImplementedException();
        }


        public void Error(string message)
        {
            throw new NotImplementedException();
        }
    }

}
