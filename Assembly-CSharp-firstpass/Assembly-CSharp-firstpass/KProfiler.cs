// Decompiled with JetBrains decompiler
// Type: KProfiler
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;

public static class KProfiler
{
  private static bool enabled = false;
  public static int counter = 0;
  public static Thread main_thread;
  public static KProfilerEndpoint AppEndpoint = (KProfilerEndpoint) new KProfilerPluginEndpoint();
  public static KProfilerEndpoint UnityEndpoint = new KProfilerEndpoint();
  public static KProfilerEndpoint ChromeEndpoint = new KProfilerEndpoint();
  private static string pattern = "<link=\"(.+)\">(.+)<\\/link>";
  private static Regex re = new Regex(KProfiler.pattern);

  public static bool IsEnabled() => KProfiler.enabled;

  public static void Enable() => KProfiler.enabled = true;

  public static void Disable()
  {
  }

  public static void BeginThread(string name, string group)
  {
  }

  public static void BeginFrame()
  {
  }

  public static void EndFrame()
  {
  }

  public static int BeginSampleI(string region_name, string group = "Game")
  {
    int counter = KProfiler.counter;
    ++KProfiler.counter;
    return counter;
  }

  public static int EndSampleI(string region_name = null)
  {
    --KProfiler.counter;
    return KProfiler.counter;
  }

  public static string SanitizeName(string name) => KProfiler.re.Replace(name, "${1}");

  [Conditional("ENABLE_KPROFILER")]
  public static void Ping(string display, string group, double value)
  {
  }

  [Conditional("ENABLE_KPROFILER")]
  public static void BeginAsync(string display, string group = "Game")
  {
  }

  [Conditional("ENABLE_KPROFILER")]
  public static void EndAsync(string display)
  {
  }

  [Conditional("ENABLE_KPROFILER")]
  public static void BeginSample(string region_name, string group = "Game") => KProfiler.BeginSampleI(region_name, group);

  [Conditional("ENABLE_KPROFILER")]
  public static void EndSample(string region_name = null) => KProfiler.EndSampleI(region_name);

  [Conditional("ENABLE_KPROFILER")]
  public static void EndSample(string region_name, int count) => KProfiler.EndSampleI(region_name);

  [Conditional("ENABLE_KPROFILER")]
  public static void BeginSample(string region_name, int count)
  {
  }

  [Conditional("ENABLE_KPROFILER")]
  public static void BeginSample(string region_name, string group, int count) => KProfiler.BeginSampleI(region_name, group);

  public static int BeginSampleI(string region_name, UnityEngine.Object profiler_obj)
  {
    int counter = KProfiler.counter;
    ++KProfiler.counter;
    return counter;
  }

  [Conditional("ENABLE_KPROFILER")]
  public static void BeginSample(string region_name, UnityEngine.Object profiler_obj) => KProfiler.BeginSampleI(region_name, profiler_obj);

  [Conditional("ENABLE_KPROFILER")]
  public static void AddEvent(string event_name)
  {
  }

  [Conditional("ENABLE_KPROFILER")]
  public static void AddCounter(
    string event_name,
    List<KeyValuePair<string, int>> series_name_counts)
  {
    foreach (KeyValuePair<string, int> seriesNameCount in series_name_counts)
      KProfiler.EndSampleI();
  }

  [Conditional("ENABLE_KPROFILER")]
  public static void AddCounter(string event_name, string series_name, int count) => KProfiler.EndSampleI(series_name);

  [Conditional("ENABLE_KPROFILER")]
  public static void AddCounter(string event_name, int count)
  {
  }

  [Conditional("ENABLE_KPROFILER")]
  public static void BeginThreadProfiling(string threadGroupName, string threadName)
  {
  }

  [Conditional("ENABLE_KPROFILER")]
  public static void EndThreadProfiling()
  {
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct Region : IDisposable
  {
    public Region(string region_name, UnityEngine.Object profiler_obj = null)
    {
    }

    public void Dispose()
    {
    }
  }
}
