// Decompiled with JetBrains decompiler
// Type: KSerialization.DebugLog
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Diagnostics;

namespace KSerialization
{
  internal static class DebugLog
  {
    private const DebugLog.Level OutputLevel = DebugLog.Level.Error;

    [Conditional("DEBUG_LOG")]
    public static void Output(DebugLog.Level msg_level, string msg)
    {
      switch (msg_level)
      {
        case DebugLog.Level.Error:
          Debug.LogError((object) msg);
          break;
        case DebugLog.Level.Warning:
          Debug.LogWarning((object) msg);
          break;
        case DebugLog.Level.Info:
          Debug.Log((object) msg);
          break;
      }
    }

    public enum Level
    {
      Error,
      Warning,
      Info,
    }
  }
}
