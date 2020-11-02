// Decompiled with JetBrains decompiler
// Type: KProfilerEndpoint
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Diagnostics;

public class KProfilerEndpoint
{
  [Conditional("ENABLE_KPROFILER")]
  public virtual void Begin(string name, string group)
  {
  }

  [Conditional("ENABLE_KPROFILER")]
  public virtual void End(string name)
  {
  }

  [Conditional("ENABLE_KPROFILER")]
  public virtual void BeginFrame()
  {
  }

  [Conditional("ENABLE_KPROFILER")]
  public virtual void Ping(string display, string group, double value)
  {
  }

  [Conditional("ENABLE_KPROFILER")]
  public virtual void BeginAsync(string display, string group)
  {
  }

  [Conditional("ENABLE_KPROFILER")]
  public virtual void EndAsync(string display)
  {
  }

  [Conditional("ENABLE_KPROFILER")]
  public virtual void EndFrame()
  {
  }
}
