using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace SportsStore.Services
{
    public class FileLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(categoryName);
        }

        public void Dispose() { }

        private class FileLogger : ILogger
        {
            private readonly string _categoryName;

            public FileLogger(string categoryName)
            {
                _categoryName = categoryName;
            }

            public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

            public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel.Warning;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
            {
                if (!IsEnabled(logLevel)) return;

                var message = formatter(state, exception);
                var logEntry = $"[{DateTime.Now}] [{logLevel}] [{_categoryName}] {message}";
                if (exception != null)
                {
                    logEntry += $"\nException: {exception.Message}\nStack Trace: {exception.StackTrace}";
                }
                logEntry += "\n\n";

                try
                {
                    // Write to error_log.txt in the application base directory
                    File.AppendAllText("error_log.txt", logEntry);
                }
                catch
                {
                    // Ignore writing failures to prevent crashing
                }
            }
        }
    }
}
