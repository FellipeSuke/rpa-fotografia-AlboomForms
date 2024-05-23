using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rpa_fotografia.Utils
{
    public static class Logger
    {
        private static readonly string logDirectory = Path.Combine(AppContext.BaseDirectory, "log");

        static Logger()
        {
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
        }

        public static void Log(string message)
        {
            string logFilePath = Path.Combine(logDirectory, $"log_{DateTime.Now:yyyyMMdd}.txt");
            string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";

            try
            {
                File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
                Console.WriteLine(logMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write log: {ex.Message}");
            }
        }
    }
}

