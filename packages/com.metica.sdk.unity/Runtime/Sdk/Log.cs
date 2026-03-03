using System;

using Metica.Core;

/// <summary>
/// Facade class for logging.
/// This requires a registered implementation of <see cref="ILog"/>
/// Example: Registry.Register<ILog>(new MyLogger());
/// For retro compatibility, use
/// <code>
/// using MeticaLogger = Log;
/// </code>
/// </summary>
namespace Metica
{
public static class Log
{
    private static readonly Lazy<ILog> _logger = new Lazy<ILog>(() => Registry.Resolve<ILog>());
    private static ILog Logger => _logger.Value;

    public static LogLevel CurrentLogLevel
    {
        get => Logger.CurrentLogLevel;
        set => Logger.CurrentLogLevel = value;
    }

    public static void Info(Func<string> messageSupplier) => Logger.I(messageSupplier);

    public static void Error(Func<string> messageSupplier, Exception error = null) =>
        Logger.E(messageSupplier, error);

    public static void Warning(Func<string> messageSupplier) => Logger.W(messageSupplier);
    public static void Debug(Func<string> messageSupplier) => Logger.D(messageSupplier);

    // Obsolete aliases - TODO : remove

    [Obsolete("PLease use 'Log.Info'")]
    public static void LogInfo(Func<string> messageSupplier) => Logger.I(messageSupplier);

    [Obsolete("PLease use 'Log.Error'")]
    public static void LogError(Func<string> messageSupplier, Exception error = null) =>
        Logger.E(messageSupplier, error);

    [Obsolete("PLease use 'Log.Warning'")]
    public static void LogWarn(Func<string> messageSupplier) => Logger.W(messageSupplier);

    [Obsolete("PLease use 'Log.Debug'")]
    public static void LogDebug(Func<string> messageSupplier) => Logger.D(messageSupplier);
}
}
