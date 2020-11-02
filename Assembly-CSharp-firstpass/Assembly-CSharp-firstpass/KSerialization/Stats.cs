// Decompiled with JetBrains decompiler
// Type: KSerialization.Stats
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace KSerialization
{
  public static class Stats
  {
    private static Dictionary<Type, Stats.StatInfo> serializationStats = new Dictionary<Type, Stats.StatInfo>();
    private static Dictionary<Type, Stats.StatInfo> deserializationStats = new Dictionary<Type, Stats.StatInfo>();

    [Conditional("ENABLE_KSERIALIZER_STATS")]
    public static void Clear()
    {
      Stats.serializationStats.Clear();
      Stats.deserializationStats.Clear();
    }

    [Conditional("ENABLE_KSERIALIZER_STATS")]
    public static void Write(Type type, long num_bytes)
    {
      Stats.StatInfo statInfo;
      Stats.serializationStats.TryGetValue(type, out statInfo);
      ++statInfo.numOccurrences;
      statInfo.numBytes += num_bytes;
      Stats.serializationStats[type] = statInfo;
    }

    [Conditional("ENABLE_KSERIALIZER_STATS")]
    public static void Read(Type type, long num_bytes)
    {
      Stats.StatInfo statInfo;
      Stats.deserializationStats.TryGetValue(type, out statInfo);
      ++statInfo.numOccurrences;
      statInfo.numBytes += num_bytes;
      Stats.deserializationStats[type] = statInfo;
    }

    public static void Print()
    {
      int count1 = Stats.serializationStats.Count;
      int count2 = Stats.deserializationStats.Count;
    }

    [Conditional("ENABLE_KSERIALIZER_STATS")]
    private static void Print(string header, Dictionary<Type, Stats.StatInfo> stats)
    {
      string str = header + "\n";
      foreach (KeyValuePair<Type, Stats.StatInfo> stat in stats)
        str = str + stat.Key.ToString() + "," + (object) stat.Value.numOccurrences + "," + (object) stat.Value.numBytes + "\n";
      DebugUtil.LogArgs((object) str);
    }

    private struct StatInfo
    {
      public int numOccurrences;
      public long numBytes;
    }
  }
}
