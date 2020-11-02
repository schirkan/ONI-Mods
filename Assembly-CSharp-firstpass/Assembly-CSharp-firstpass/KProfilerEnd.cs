// Decompiled with JetBrains decompiler
// Type: KProfilerEnd
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

public class KProfilerEnd : MonoBehaviour
{
  private void Start()
  {
  }

  private void LateUpdate() => KProfiler.EndFrame();
}
