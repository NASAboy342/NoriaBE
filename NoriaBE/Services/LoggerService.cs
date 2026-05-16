using NLog;

namespace NoriaBE.Services;

public class LoggerService : ILoggerService
{
    private static readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger();

    public void Info(string message)
    {
        _logger.Info(message);
    }

    public void Error(string message)
    {
        _logger.Error(message);
    }
}
