using Microsoft.Extensions.DependencyInjection;

using Serilog;
using Serilog.Core;
using Serilog.Events;

using Xunit.Abstractions;

namespace Client.Tests;

public static class ServiceCollectionLoggingExtensions
{
    public static IServiceCollection AddXunitLogger(this IServiceCollection services, ITestOutputHelper outputHelper)
    {
        Logger serilogLogger = new LoggerConfiguration()
                               .MinimumLevel.Verbose()
                               .WriteTo.TestOutput(outputHelper, LogEventLevel.Verbose)
                               .CreateLogger();

        services.AddSingleton(new LoggerFactory().AddSerilog(serilogLogger, dispose: true));
        services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

        return services;
    }
}
