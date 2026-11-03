using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalDataAccessLayer.Global_Classes
{
    public static class clsLogger
    {
        private static ILogger? _logger;

        public static void Initialize(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("CarRentalAPI.DataAccess");
        }

        public static void LogError(Exception ex, string message)
        {
            _logger?.LogError(ex, message);
        }

        public static void LogWarning(string message)
        {
            _logger?.LogWarning(message);
        }

        public static void LogInformation(string message)
        {
            _logger?.LogInformation(message);
        }
    }
}
