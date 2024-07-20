namespace MbyronModsCommon.API;
using System;
using System.Reflection;
using UnityEngine;

public class Logger {
    private static string Name { get; } = Assembly.GetExecutingAssembly().GetName().Name;

    public static void Error(string tag, object message) => UnityEngine.Debug.logger.LogError(GetTag(LogType.Error), $"{tag} | {message}");
    public static void Error(object message) => UnityEngine.Debug.logger.LogError(GetTag(LogType.Error), $" {message}");
    public static void Warning(object message) => UnityEngine.Debug.logger.LogWarning(GetTag(LogType.Warning), $"{message}");
    public static void Warning(string tag, object message) => UnityEngine.Debug.logger.LogWarning(GetTag(LogType.Warning), $"{tag} | {message}");
    public static void Log(object message) => UnityEngine.Debug.logger.Log(GetTag(LogType.Log), $"{message}");
    public static void Exception(string tag, Exception exception) => UnityEngine.Debug.logger.Log(GetTag(LogType.Exception), $"{tag} | {exception}");
    private static string GetTag(LogType logType) => $"{Name} | [{logType}]";
}
