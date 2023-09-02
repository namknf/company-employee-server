﻿namespace Contracts
{
    public interface ILoggerService
    {
        void LogInformation(string message);

        void LogWarning(string message);

        void LogError(string message);

        void LogDebug(string message);
    }
}
