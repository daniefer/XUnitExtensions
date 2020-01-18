using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace XUnitLoggingExtensions.Logging
{
    [SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "Not needed.")]
    public class TestOutputHelperLoggingProvider : ILoggerProvider, ISupportExternalScope
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private IExternalScopeProvider _scopeProvider;

        public TestOutputHelperLoggingProvider(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new TestOutputHelperLogger(_testOutputHelper, _scopeProvider, null);
        }

        [SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "Not needed.")]
        public void Dispose()
        {
        }

        public void SetScopeProvider(IExternalScopeProvider scopeProvider)
        {
            _scopeProvider = scopeProvider;
        }
    }
}
