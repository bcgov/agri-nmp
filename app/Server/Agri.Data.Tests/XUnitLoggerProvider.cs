using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Agri.Data.Tests
{
    public class XUnitLoggerProvider : ILoggerProvider
    {
        private readonly ITestOutputHelper output;

        public XUnitLoggerProvider(ITestOutputHelper output)
        {
            this.output = output;
        }

        public void Dispose()
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new XUnitLogger(output);
        }

        private class XUnitLogger : ILogger
        {
            private readonly ITestOutputHelper output;

            public XUnitLogger(ITestOutputHelper output)
            {
                this.output = output;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                if (!IsEnabled(logLevel)) return;

                if (formatter == null) throw new ArgumentNullException(nameof(formatter));

                var message = formatter(state, exception);
                if (string.IsNullOrEmpty(message) && exception == null) return;

                var line = $"{logLevel}: {nameof(XUnitLogger)}: {message}";
                if (exception != null) line = $"{line}{Environment.NewLine}{exception.ToString()}";
                output.WriteLine(line);
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }
        }
    }
}