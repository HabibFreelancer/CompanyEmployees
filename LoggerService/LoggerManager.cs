using Contracts.Infrastructure;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerService
{
    public class LoggerManager<T> : ILoggerManager<T>
    {
        private readonly ILogger<T> _logger;
        public LoggerManager(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<T>();
        }
        public void LogDebug(string message, params object[] args) => _logger.LogDebug(message, args);
        public void LogError(string message, params object[] args) => _logger.LogError(message, args);
        public void LogInfo(string message, params object[] args) => _logger.LogInformation(message, args);
        public void LogWarn(string message, params object[] args) => _logger.LogWarning(message, args);
    }

}
