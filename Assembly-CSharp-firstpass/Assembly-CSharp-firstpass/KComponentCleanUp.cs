// Decompiled with JetBrains decompiler
// Type: KComponentCleanUp
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

public class KComponentCleanUp : MonoBehaviour
{
  public static KComponentCleanUp instance;
  private static bool inCleanUpPhase;
  private KComponents comps;

  public static void SetInCleanUpPhase(bool in_cleanup_phase) => KComponentCleanUp.inCleanUpPhase = in_cleanup_phase;

  public static bool InCleanUpPhase => KComponentCleanUp.inCleanUpPhase;

  private void Awake()
  {
    KComponentCleanUp.instance = this;
    this.comps = this.GetComponent<KComponentSpawn>().comps;
  }

  private void FixedUpdate() => KComponentCleanUp.SetInCleanUpPhase(true);

  private void Update()
  {
    KComponentCleanUp.SetInCleanUpPhase(true);
    this.comps.CleanUp();
  }
}
