using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Hosting;
using Serilog.Parsing;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FDEV.Rules.Demo.Core.Logging.AspNetCore
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly DiagnosticContext _diagnosticContext;
        private readonly MessageTemplate _messageTemplate;
        private readonly Action<IDiagnosticContext, HttpContext> _enrichDiagnosticContext;
        private readonly Func<HttpContext, double, Exception, LogEventLevel> _getLevel;
        private readonly ILogger _logger;
        private static readonly LogEventProperty[] NoProperties = new LogEventProperty[0];

        public RequestLoggingMiddleware(RequestDelegate next, DiagnosticContext diagnosticContext, RequestLoggingOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _diagnosticContext = diagnosticContext ?? throw new ArgumentNullException(nameof(diagnosticContext));
            _getLevel = options.GetLevel;
            _enrichDiagnosticContext = options.EnrichDiagnosticContext;
            _messageTemplate = new MessageTemplateParser().Parse(options.MessageTemplate);
            _logger = options.Logger?.ForContext<RequestLoggingMiddleware>();
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

            var start = Stopwatch.GetTimestamp();
            var collector = _diagnosticContext.BeginCollection();
            try
            {
                await _next(httpContext);
                var elapsedMs = GetElapsedMilliseconds(start, Stopwatch.GetTimestamp());
                var statusCode = httpContext.Response.StatusCode;
                LogCompletion(httpContext, collector, statusCode, elapsedMs, null);
            }
            catch (Exception ex)
                // Never caught, because `LogCompletion()` returns false. This ensures e.g. the developer exception page is still
                // shown, although it does also mean we see a duplicate "unhandled exception" event from ASP.NET Core.
                when (LogCompletion(httpContext, collector, 500, GetElapsedMilliseconds(start, Stopwatch.GetTimestamp()), ex))
            {
            }
            finally
            {
                collector.Dispose();
            }
        }

        private bool LogCompletion(HttpContext httpContext, DiagnosticContextCollector collector, int statusCode, double elapsedMs, Exception ex)
        {
            var logger = _logger ?? Log.ForContext<RequestLoggingMiddleware>();
            var level = _getLevel(httpContext, elapsedMs, ex);
            if (!logger.IsEnabled(level)) return false;

            _enrichDiagnosticContext?.Invoke(_diagnosticContext, httpContext);

            if (!collector.TryComplete(out var collectedProperties))
            {
                collectedProperties = NoProperties;
            }

            var properties = collectedProperties.Concat(new[]
            {
                new LogEventProperty("RequestMethod", new ScalarValue(httpContext.Request.Method)),
                new LogEventProperty("RequestPath", new ScalarValue(GetPath(httpContext))),
                new LogEventProperty("StatusCode", new ScalarValue(statusCode)),
                new LogEventProperty("Elapsed", new ScalarValue(elapsedMs))
            });

            var evt = new LogEvent(DateTimeOffset.Now, level, ex, _messageTemplate, properties);
            logger.Write(evt);
            return false;
        }

        private static double GetElapsedMilliseconds(long start, long stop) => (stop - start) * 1000 / (double)Stopwatch.Frequency;

        private static string GetPath(HttpContext httpContext)
        {
            // In some cases, like when running integration tests with WebApplicationFactory<T> the RawTarget
            // returns an empty string instead of null, in that case we can't use ?? as fallback.
            var requestPath = httpContext.Features.Get<IHttpRequestFeature>()?.RawTarget;
            return !string.IsNullOrEmpty(requestPath) ? requestPath : httpContext.Request.Path.ToString();
        }
    }
}
