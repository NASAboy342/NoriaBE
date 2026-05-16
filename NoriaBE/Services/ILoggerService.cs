using System;

namespace NoriaBE.Services;

public interface ILoggerService
{
    void Info(string message);
    void Error(string message);
}
