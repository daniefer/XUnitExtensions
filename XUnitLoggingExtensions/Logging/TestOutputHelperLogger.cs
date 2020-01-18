using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace XUnitLoggingExtensions.Logging
{
    public class TestOutputHelperLogger : ILogger
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly string _format;
        private readonly LogLevel _minLogLevel;
        private readonly IExternalScopeProvider _scopeProvider;

        public TestOutputHelperLogger(ITestOutputHelper testOutputHelper, IExternalScopeProvider scopeProvider, string logFormat, LogLevel minLogLevel = LogLevel.Information)
        {
            _testOutputHelper = testOutputHelper;
            _scopeProvider = scopeProvider;
            _format = logFormat ?? "[{0} {1}] - {2}";
            _minLogLevel = minLogLevel;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return _scopeProvider?.Push(state);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _minLogLevel <= logLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(_format, logLevel, eventId, formatter(state, exception));
            _scopeProvider?.ForEachScope((obj, sb) =>
            {
                if (obj is IEnumerable<KeyValuePair<string, object>> properties)
                {
                    foreach (var pair in properties)
                    {
                        sb.Append(pair.Key).Append(": ").AppendLine(pair.Value?.ToString());
                    }
                }
                else if (obj != null)
                {
                    sb.AppendLine(obj.ToString());
                }
            }, stringBuilder);
            _testOutputHelper.WriteLine(stringBuilder.ToString());
        }
    }
}
