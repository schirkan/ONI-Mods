// Decompiled with JetBrains decompiler
// Type: KComponentSpawn
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

public class KComponentSpawn : MonoBehaviour, ISim200ms, ISim33ms
{
  public static KComponentSpawn instance;
  public KComponents comps;

  private void FixedUpdate()
  {
    KComponentCleanUp.SetInCleanUpPhase(false);
    this.comps.Spawn();
  }

  private void Update()
  {
    KComponentCleanUp.SetInCleanUpPhase(false);
    this.comps.Spawn();
    this.comps.RenderEveryTick(Time.deltaTime);
  }

  public void Sim33ms(float dt) => this.comps.Sim33ms(dt);

  public void Sim200ms(float dt) => this.comps.Sim200ms(dt);

  private void OnDestroy() => this.comps.Shutdown();
}
