// Decompiled with JetBrains decompiler
// Type: KProfilerBegin
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

public class KProfilerBegin : MonoBehaviour
{
  public static int begin_counter;

  private void Start()
  {
    Debug.Log((object) "KProfiler: Start");
    KProfiler.BeginThread("Main", "Game");
  }

  private void Update() => KProfiler.BeginFrame();
}
