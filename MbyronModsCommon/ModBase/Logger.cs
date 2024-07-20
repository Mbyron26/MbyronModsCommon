using ColossalFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using UnityEngine;

namespace MbyronModsCommon;
public class Logger : ILog {
    private static readonly object fileLock = new();
    private static string debugFilePath;

    public static string DebugFileName { get; } = AssemblyUtils.CurrentAssemblyName + ".log";
    public static string DebugFilePath => debugFilePath ??= Path.Combine(Application.dataPath, DebugFileName);
    public bool InternalLogging { get; set; }
    public bool ExternalLoggingFileCreated { get; private set; }
    private static Dictionary<string, ILog> Loggers { get; set; } = new();
    public string Name { get; set; }

    public Logger(string name, bool internalLogging = false) {
        Name = name;
        InternalLogging = internalLogging;
    }

    public static ILog GetLogger(string name, bool internalLogging = false) {
        if (name.IsNullOrWhiteSpace()) {
            name = "Temp";
        }
        if (Loggers.TryGetValue(name, out var log)) {
            return log;
        }
        var logger = new Logger(name, internalLogging);
        Loggers.Add(name, logger);
        return logger;
    }

    public void Debug(object message) => Log(LogType.Debug, message);
    public void Debug(Exception exception, object message) => Log(LogType.Debug, $"\n{message}");
    public void DebugFormat(string format, object p) => Log(LogType.Debug, string.Format(format, p));
    public void DebugFormat(string format, object p1, object p2) => Log(LogType.Debug, string.Format(format, p1, p2));
    public void DebugFormat(string format, params object[] p) => Log(LogType.Debug, string.Format(format, p));

    public void Info(object message) => Log(LogType.Info, message);
    public void Info(Exception exception, object message) => Log(LogType.Info, $"\n{message}");
    public void InfoFormat(string format, object p) => Log(LogType.Info, string.Format(format, p));
    public void InfoFormat(string format, object p1, object p2) => Log(LogType.Info, string.Format(format, p1, p2));
    public void InfoFormat(string format, params object[] p) => Log(LogType.Info, string.Format(format, p));

    public void Error(object message) => Log(LogType.Error, message);
    public void Error(Exception exception, object message) => Log(LogType.Error, $"\n{message}");
    public void ErrorFormat(string format, object p) => Log(LogType.Error, string.Format(format, p));
    public void ErrorFormat(string format, object p1, object p2) => Log(LogType.Error, string.Format(format, p1, p2));
    public void ErrorFormat(string format, params object[] p) => Log(LogType.Error, string.Format(format, p));

    public void Warn(object message) => Log(LogType.Warn, message);
    public void Warn(Exception exception, object message) => Log(LogType.Warn, $"\n{message}");
    public void WarnFormat(string format, object p) => Log(LogType.Warn, string.Format(format, p));
    public void WarnFormat(string format, object p1, object p2) => Log(LogType.Warn, string.Format(format, p1, p2));
    public void WarnFormat(string format, params object[] p) => Log(LogType.Warn, string.Format(format, p));

    public void Patch(object message) => Log(LogType.Patch, message);
    public void Patch(Exception exception, object message) => Log(LogType.Patch, $"\n{message}");
    public void Patch(PatcherType patchType, MethodBase original, MethodBase patch) => Log(LogType.Patch, $"[{patchType}] [{original.DeclaringType.FullName}.{original.Name}] patched by [{patch.DeclaringType.FullName}.{patch.Name}]");

    private string GetPrefixLog(LogType logType) => '[' + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ']' + $" [{logType.ToString().ToUpper()}] ";

    private void Log(LogType logType, object message) {
        if (InternalLogging) {
            UnityEngine.Debug.logger.Log($"[{AssemblyUtils.CurrentAssemblyName}] {GetPrefixLog(logType)}  {message}");
        }
        else {
            EnsureExternalFile();
            Monitor.Enter(fileLock);
            try {
                using FileStream debugFile = new(DebugFilePath, FileMode.Append);
                using StreamWriter sw = new(debugFile);
                sw.WriteLine($"{GetPrefixLog(logType)}  {message}");
            }
            finally {
                Monitor.Exit(fileLock);
            }
        }
    }

    private void EnsureExternalFile() {
        if (!ExternalLoggingFileCreated) {
            using FileStream debugFile = new(DebugFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
            using StreamWriter sw = new(debugFile);
            sw.WriteLine($"--- {AssemblyUtils.CurrentAssemblyName} {AssemblyUtils.CurrentAssemblyVersion} ---");
            ExternalLoggingFileCreated = true;
        }
    }
}

public enum LogType {
    Debug,
    Info,
    Warn,
    Error,
    Patch
}

public interface ILog {
    string Name { get; set; }
    bool InternalLogging { get; set; }

    void Debug(object message);
    void Debug(Exception exception, object message);
    void DebugFormat(string format, object p1);
    void DebugFormat(string format, object p1, object p2);
    void DebugFormat(string format, params object[] p);

    void Info(object message);
    void Info(Exception exception, object message);
    void InfoFormat(string format, object p);
    void InfoFormat(string format, object p1, object p2);
    void InfoFormat(string format, params object[] p);

    void Warn(object message);
    void Warn(Exception exception, object message);
    void WarnFormat(string format, object p);
    void WarnFormat(string format, object p1, object p2);
    void WarnFormat(string format, params object[] p);

    void Error(object message);
    void Error(Exception exception, object message);
    void ErrorFormat(string format, object p);
    void ErrorFormat(string format, object p1, object p2);
    void ErrorFormat(string format, params object[] p);

    void Patch(object message);
    void Patch(Exception exception, object message);
    void Patch(PatcherType patchType, MethodBase original, MethodBase patch);
}