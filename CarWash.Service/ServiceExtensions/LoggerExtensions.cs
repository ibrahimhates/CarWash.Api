
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CarWash.Service.ServiceExtensions
{
    public static class LoggerExtensions
    {
        public static void ConfigureLogging()
        {
            try
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{environment}.json", optional: true)
                    .Build();

                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .Enrich.FromLogContext()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                    .Enrich.WithMachineName()
                    .Enrich.WithExceptionDetails()
                    .WriteTo.Console()
                    .WriteTo.Debug()
                    .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment))
                    .Enrich.WithProperty("Environment", environment)
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();
            }
            catch (Exception ex)
            {
                Log.Warning("Logger Exception: " + ex.Message);
            }
        }

        private static ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
        {
            environment ??= "dev";
            return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"{Assembly.GetEntryAssembly().GetName().Name.Replace("Yk.Redesign.", "").ToString().ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}",
                ModifyConnectionSettings = x =>
                {
                    x.BasicAuthentication(configuration["ElasticConfiguration:UserName"], configuration["ElasticConfiguration:Password"]);
                    x.ServerCertificateValidationCallback(
                        (o, certificate, arg3, arg4) => { return true; });
                    return x;
                },
                DeadLetterIndexName = "deadletter-{0:yyyy.MM}",

            };
        }
        public static void SendError<T>(this ILogger<T> _logger, Exception ex, [CallerMemberName] string methodName = "")
        {
            _logger.LogError($"{methodName} throw an exception. Exception Message: {ex.Message}. Inner Exception Message: {(ex.InnerException != null ? ex.InnerException.Message : "")}");
        }
        public static void SendError<T>(this ILogger<T> _logger, string message, Exception ex, [CallerMemberName] string methodName = "")
        {
            _logger.LogError($"{methodName}: {message}. Exception Message: {ex.Message}. Inner Exception Message: {(ex.InnerException != null ? ex.InnerException.Message : "")}");
        }
        public static void SendWarning<T>(this ILogger<T> _logger, object o, [CallerMemberName] string methodName = "")
        {
            _logger.LogWarning($"{methodName} warning. {System.Text.Json.JsonSerializer.Serialize(o)}");
        }
        public static void SendWarning<T>(this ILogger<T> _logger, string message, [CallerMemberName] string methodName = "")
        {
            _logger.LogWarning($"{methodName} warning. {message}");
        }
        public static void SendInformation<T>(this ILogger<T> _logger, object o, [CallerMemberName] string methodName = "")
        {
            _logger.LogInformation($"{methodName} just started. Request: {System.Text.Json.JsonSerializer.Serialize(o)}");
        }
        public static void SendInformation<T>(this ILogger<T> _logger, string message, [CallerMemberName] string methodName = "")
        {
            _logger.LogInformation($"{methodName}. {message}");
        }

    }
}
