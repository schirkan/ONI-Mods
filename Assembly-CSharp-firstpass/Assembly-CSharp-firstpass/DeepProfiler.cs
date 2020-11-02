// Decompiled with JetBrains decompiler
// Type: DeepProfiler
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Diagnostics;

public class DeepProfiler
{
  private bool enableProfiling;

  public DeepProfiler(bool enable_profiling) => this.enableProfiling = enable_profiling;

  [Conditional("DEEP_PROFILE")]
  public void BeginSample(string message)
  {
    int num = this.enableProfiling ? 1 : 0;
  }

  [Conditional("DEEP_PROFILE")]
  public void EndSample()
  {
    int num = this.enableProfiling ? 1 : 0;
  }
}
