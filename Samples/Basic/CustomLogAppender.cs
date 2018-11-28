using System;
using Dolittle.Logging;

namespace Basic
{
    public class CustomLogAppender : ILogAppender
    {
        public void Append(string filePath, int lineNumber, string member, LogLevel level, string message, Exception exception = null)
        {
            if (exception != null) Console.WriteLine($"({filePath} - {member}[{lineNumber}]) - {level} - {message} - {exception.Message} - {exception.StackTrace}");
            else Console.WriteLine($"({filePath} - {member}[{lineNumber}]) - {level} - {message}");
        }
    }
}
