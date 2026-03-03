using System;
using UnityEngine;
using Metica.Core;

namespace Metica.Unity
{
    /// <summary>
    /// Internal logger for Metica SDK with configurable log level and automatic tag prefixing.
    /// </summary>
    internal class Logger : ILog
    {
        private readonly string _tag;

        /// <summary>
        /// The current log level. Messages below this level are filtered out.
        /// Defaults to <see cref="LogLevel.Error"/>.
        /// </summary>
        public LogLevel CurrentLogLevel { get; set; } = LogLevel.Error;

        /// <summary>
        /// Creates a new logger instance with the specified tag.
        /// </summary>
        /// <param name="tag">Tag to prepend to all log messages (without brackets).</param>
        public Logger(string tag)
        {
            _tag = tag;
        }

        private string FormatMessage(Func<string> messageSupplier) => $"[{_tag}] {messageSupplier()}";

        public void I(Func<string> messageSupplier)
        {
            if (CurrentLogLevel >= LogLevel.Info)
            {
                Debug.Log(FormatMessage(messageSupplier));
            }
        }

        public void E(Func<string> messageSupplier, Exception error = null)
        {
            if (CurrentLogLevel >= LogLevel.Error)
            {
                Debug.LogError(FormatMessage(messageSupplier));
                if (error != null) Debug.LogException(error);
            }
        }

        public void W(Func<string> messageSupplier)
        {
            if (CurrentLogLevel >= LogLevel.Warning)
            {
                Debug.LogWarning(FormatMessage(messageSupplier));
            }
        }

        public void D(Func<string> messageSupplier)
        {
            if (CurrentLogLevel >= LogLevel.Debug)
            {
                Debug.Log(FormatMessage(messageSupplier));
            }
        }
    }
}
