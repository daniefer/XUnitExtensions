using Microsoft.Extensions.Logging;
using Xunit.Abstractions;
using XUnitLoggingExtensions.Logging;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LoggerFactoryExtensions
    {
        public static ILoggingBuilder AddTestOutputHelper(this ILoggingBuilder builder, ITestOutputHelper testOutputHelper)
        {
            return builder.AddProvider(new TestOutputHelperLoggingProvider(testOutputHelper));
        }
    }
}
