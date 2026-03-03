using System;

namespace Metica.Core
{
    public enum LogLevel
    {
        Off,
        Error,
        Warning,
        Info,
        Debug,
    }

    public interface ILog
    {
        /// <summary>
        /// The current log level can be changed at runtime.
        /// </summary>
        LogLevel CurrentLogLevel { get; set; }
        void D(Func<string> messageSupplier);
        void E(Func<string> messageSupplier, Exception error = null);
        void I(Func<string> messageSupplier);
        void W(Func<string> messageSupplier);
    }
}
