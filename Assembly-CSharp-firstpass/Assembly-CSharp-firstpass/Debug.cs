// Decompiled with JetBrains decompiler
// Type: Debug
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

public static class Debug
{
  private static bool s_loggingDisabled;

  private static string TimeStamp() => DateTime.UtcNow.ToString("[HH:mm:ss.fff] [") + (object) Thread.CurrentThread.ManagedThreadId + "] ";

  private static void WriteTimeStamped(params object[] objs) => Console.WriteLine(Debug.TimeStamp() + DebugUtil.BuildString(objs));

  public static bool isDebugBuild => UnityEngine.Debug.isDebugBuild;

  public static bool developerConsoleVisible
  {
    get => UnityEngine.Debug.developerConsoleVisible;
    set => UnityEngine.Debug.developerConsoleVisible = value;
  }

  public static void Break()
  {
  }

  public static void LogException(Exception exception)
  {
    if (Debug.s_loggingDisabled)
      return;
    UnityEngine.Debug.LogException(exception);
  }

  public static void Log(object obj)
  {
    if (Debug.s_loggingDisabled)
      return;
    Debug.WriteTimeStamped((object) "[INFO]", obj);
  }

  public static void Log(object obj, UnityEngine.Object context)
  {
    if (Debug.s_loggingDisabled)
      return;
    Debug.WriteTimeStamped((object) "[INFO]", context != (UnityEngine.Object) null ? (object) context.name : (object) "null", obj);
  }

  public static void LogFormat(string format, params object[] args)
  {
    if (Debug.s_loggingDisabled)
      return;
    Debug.WriteTimeStamped((object) "[INFO]", (object) string.Format(format, args));
  }

  public static void LogFormat(UnityEngine.Object context, string format, params object[] args)
  {
    if (Debug.s_loggingDisabled)
      return;
    Debug.WriteTimeStamped((object) "[INFO]", context != (UnityEngine.Object) null ? (object) context.name : (object) "null", (object) string.Format(format, args));
  }

  public static void LogWarning(object obj)
  {
    if (Debug.s_loggingDisabled)
      return;
    Debug.WriteTimeStamped((object) "[WARNING]", obj);
  }

  public static void LogWarning(object obj, UnityEngine.Object context)
  {
    if (Debug.s_loggingDisabled)
      return;
    Debug.WriteTimeStamped((object) "[WARNING]", context != (UnityEngine.Object) null ? (object) context.name : (object) "null", obj);
  }

  public static void LogWarningFormat(string format, params object[] args)
  {
    if (Debug.s_loggingDisabled)
      return;
    Debug.WriteTimeStamped((object) "[WARNING]", (object) string.Format(format, args));
  }

  public static void LogWarningFormat(UnityEngine.Object context, string format, params object[] args)
  {
    if (Debug.s_loggingDisabled)
      return;
    Debug.WriteTimeStamped((object) "[WARNING]", context != (UnityEngine.Object) null ? (object) context.name : (object) "null", (object) string.Format(format, args));
  }

  public static void LogError(object obj)
  {
    if (Debug.s_loggingDisabled)
      return;
    Debug.WriteTimeStamped((object) "[ERROR]", obj);
    UnityEngine.Debug.LogError(obj);
  }

  public static void LogError(object obj, UnityEngine.Object context)
  {
    if (Debug.s_loggingDisabled)
      return;
    Debug.WriteTimeStamped((object) "[ERROR]", context != (UnityEngine.Object) null ? (object) context.name : (object) "null", obj);
    UnityEngine.Debug.LogError(obj, context);
  }

  public static void LogErrorFormat(string format, params object[] args)
  {
    if (Debug.s_loggingDisabled)
      return;
    Debug.WriteTimeStamped((object) "[ERROR]", (object) string.Format(format, args));
    UnityEngine.Debug.LogErrorFormat(format, args);
  }

  public static void LogErrorFormat(UnityEngine.Object context, string format, params object[] args)
  {
    if (Debug.s_loggingDisabled)
      return;
    Debug.WriteTimeStamped((object) "[ERROR]", context != (UnityEngine.Object) null ? (object) context.name : (object) "null", (object) string.Format(format, args));
    UnityEngine.Debug.LogErrorFormat(context, format, args);
  }

  public static void Assert(bool condition)
  {
    if (condition)
      return;
    Debug.LogError((object) "Assert failed");
    Debug.Break();
  }

  public static void Assert(bool condition, object message)
  {
    if (condition)
      return;
    Debug.LogError((object) ("Assert failed: " + message));
    Debug.Break();
  }

  public static void Assert(bool condition, object message, UnityEngine.Object context)
  {
    if (condition)
      return;
    Debug.LogError((object) ("Assert failed: " + message), context);
    Debug.Break();
  }

  public static void DisableLogging() => Debug.s_loggingDisabled = true;

  [Conditional("UNITY_EDITOR")]
  public static void DrawLine(
    Vector3 start,
    Vector3 end,
    Color color = default (Color),
    float duration = 0.0f,
    bool depthTest = true)
  {
    UnityEngine.Debug.DrawLine(start, end, color, duration, depthTest);
  }

  [Conditional("UNITY_EDITOR")]
  public static void DrawRay(
    Vector3 start,
    Vector3 dir,
    Color color = default (Color),
    float duration = 0.0f,
    bool depthTest = true)
  {
    UnityEngine.Debug.DrawRay(start, dir, color, duration, depthTest);
  }
}
