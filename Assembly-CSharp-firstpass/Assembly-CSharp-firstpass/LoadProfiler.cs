// Decompiled with JetBrains decompiler
// Type: LoadProfiler
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Diagnostics;

public class LoadProfiler : ProfilerBase
{
  private static LoadProfiler instance;

  private LoadProfiler(string file_prefix)
    : base(file_prefix)
  {
  }

  public static LoadProfiler Instance
  {
    get
    {
      if (LoadProfiler.instance == null)
      {
        LoadProfiler.instance = new LoadProfiler("load_stats_");
        if (!Stopwatch.IsHighResolution)
          UnityEngine.Debug.LogWarning((object) ("Low resolution timer! [" + (object) Stopwatch.Frequency + "] ticks per second"));
      }
      return LoadProfiler.instance;
    }
  }

  private static void ProfilerSection(string region_name, string file = "unknown", uint line = 0) => LoadProfiler.Instance.Push(region_name, file, line);

  private static void EndProfilerSection() => LoadProfiler.Instance.Pop();

  [Conditional("ENABLE_LOAD_STATS")]
  public static void AddEvent(string event_name, string file = "unknown", uint line = 0)
  {
    if (!LoadProfiler.Instance.IsRecording() || LoadProfiler.Instance.proFile == null)
      return;
    LoadProfiler.Instance.ManifestThreadInfo().WriteLine("GAME", event_name, LoadProfiler.Instance.sw, "I", "},");
  }

  [Conditional("ENABLE_LOAD_STATS")]
  public static void BeginSample(string region_name) => LoadProfiler.Instance.Push(region_name, "unknown", 0U);

  [Conditional("ENABLE_LOAD_STATS")]
  public static void EndSample() => LoadProfiler.Instance.Pop();
}
