using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Microsoft.Extensions.Logging;
using DotNet.Blog.Migrations.ConsoleApp;

// log
Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File(
                Path.Combine("Logs/log-.txt"),
                LogEventLevel.Information,
                rollingInterval: RollingInterval.Day,
                fileSizeLimitBytes: 10 * 1024 * 1024,
                shared: true,
                outputTemplate: @"{source}{Timestamp} [{Level:u3}][{TraceId}][{SpanId}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

await CreateHostBuilder(args).RunConsoleAsync();

static IHostBuilder CreateHostBuilder(string[] args) =>
      Host.CreateDefaultBuilder(args)
          //.ConfigureLogging((context, logging) => logging.ClearProviders().AddSerilog())
          .ConfigureServices((hostContext, services) =>
          {
              services.AddHostedService<DbMigratorHostedService>();
          });