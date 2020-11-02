// Decompiled with JetBrains decompiler
// Type: DebugUtil
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Diagnostics;
using System.Text;
using UnityEngine;

public static class DebugUtil
{
  private static StringBuilder s_errorMessageBuilder = new StringBuilder();
  private static Exception s_lastExceptionLogged;
  private static StringBuilder fullNameBuilder = new StringBuilder();

  public static void Assert(bool test) => Debug.Assert(test);

  public static void Assert(bool test, string message) => Debug.Assert(test, (object) message);

  public static void Assert(bool test, string message0, string message1)
  {
    if (test)
      return;
    DebugUtil.s_errorMessageBuilder.Length = 0;
    Debug.Assert(test, (object) DebugUtil.s_errorMessageBuilder.Append(message0).Append(" ").Append(message1).ToString());
  }

  public static void Assert(bool test, string message0, string message1, string message2)
  {
    if (test)
      return;
    DebugUtil.s_errorMessageBuilder.Length = 0;
    Debug.Assert(test, (object) DebugUtil.s_errorMessageBuilder.Append(message0).Append(" ").Append(message1).Append(" ").Append(message2).ToString());
  }

  public static string BuildString(object[] objs)
  {
    string str = "";
    if (objs.Length != 0)
    {
      str = objs[0] != null ? objs[0].ToString() : "null";
      for (int index = 1; index < objs.Length; ++index)
      {
        object obj = objs[index];
        str = str + " " + (obj != null ? obj.ToString() : "null");
      }
    }
    return str;
  }

  public static void DevAssert(bool test, string msg)
  {
    if (test)
      return;
    Debug.LogWarning((object) msg);
  }

  public static void DevAssertArgs(bool test, params object[] objs)
  {
    if (test)
      return;
    Debug.LogWarning((object) DebugUtil.BuildString(objs));
  }

  public static void DevAssertArgsWithStack(bool test, params object[] objs)
  {
    if (test)
      return;
    StackTrace stackTrace = new StackTrace(1, true);
    Debug.LogWarning((object) string.Format("{0}\n{1}", (object) DebugUtil.BuildString(objs), (object) stackTrace));
  }

  public static void DevLogError(UnityEngine.Object context, string msg) => Debug.LogWarningFormat(context, msg, (object[]) Array.Empty<object>());

  public static void DevLogError(string msg) => Debug.LogWarningFormat(msg, (object[]) Array.Empty<object>());

  public static void DevLogErrorFormat(UnityEngine.Object context, string format, params object[] args) => Debug.LogWarningFormat(context, format, args);

  public static void DevLogErrorFormat(string format, params object[] args) => Debug.LogWarningFormat(format, args);

  public static void LogArgs(params object[] objs) => Debug.Log((object) DebugUtil.BuildString(objs));

  public static void LogArgs(UnityEngine.Object context, params object[] objs) => Debug.Log((object) DebugUtil.BuildString(objs), context);

  public static void LogWarningArgs(params object[] objs) => Debug.LogWarning((object) DebugUtil.BuildString(objs));

  public static void LogWarningArgs(UnityEngine.Object context, params object[] objs) => Debug.LogWarning((object) DebugUtil.BuildString(objs), context);

  public static void LogErrorArgs(params object[] objs) => Debug.LogError((object) DebugUtil.BuildString(objs));

  public static void LogErrorArgs(UnityEngine.Object context, params object[] objs) => Debug.LogError((object) DebugUtil.BuildString(objs), context);

  public static void LogException(UnityEngine.Object context, string errorMessage, Exception e)
  {
    DebugUtil.s_lastExceptionLogged = e;
    DebugUtil.LogErrorArgs(context, (object) errorMessage, (object) ("\n" + e.ToString()));
  }

  public static Exception RetrieveLastExceptionLogged()
  {
    Exception lastExceptionLogged = DebugUtil.s_lastExceptionLogged;
    DebugUtil.s_lastExceptionLogged = (Exception) null;
    return lastExceptionLogged;
  }

  private static void RecursiveBuildFullName(GameObject obj)
  {
    if ((UnityEngine.Object) obj == (UnityEngine.Object) null)
      return;
    DebugUtil.RecursiveBuildFullName(obj.transform.parent.gameObject);
    DebugUtil.fullNameBuilder.Append("/").Append(obj.name);
  }

  private static StringBuilder BuildFullName(GameObject obj)
  {
    DebugUtil.fullNameBuilder.Length = 0;
    DebugUtil.RecursiveBuildFullName(obj);
    return DebugUtil.fullNameBuilder.Append(" (").Append(obj.GetInstanceID()).Append(")");
  }

  public static string FullName(GameObject obj) => DebugUtil.BuildFullName(obj).ToString();

  public static string FullName(Component cmp) => DebugUtil.BuildFullName(cmp.gameObject).Append(" (").Append((object) cmp.GetType()).Append(" ").Append(cmp.GetInstanceID().ToString()).Append(")").ToString();

  [Conditional("UNITY_EDITOR")]
  public static void LogIfSelected(GameObject obj, params object[] objs)
  {
  }

  [Conditional("ENABLE_DETAILED_PROFILING")]
  public static void ProfileBegin(string str)
  {
  }

  [Conditional("ENABLE_DETAILED_PROFILING")]
  public static void ProfileBegin(string str, UnityEngine.Object target)
  {
  }

  [Conditional("ENABLE_DETAILED_PROFILING")]
  public static void ProfileEnd()
  {
  }
}
