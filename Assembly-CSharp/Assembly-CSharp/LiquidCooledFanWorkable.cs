// Decompiled with JetBrains decompiler
// Type: LiquidCooledFanWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/LiquidCooledFanWorkable")]
public class LiquidCooledFanWorkable : Workable
{
  [MyCmpGet]
  private Operational operational;

  private LiquidCooledFanWorkable() => this.showProgressBar = false;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.workerStatusItem = (StatusItem) null;
  }

  protected override void OnSpawn()
  {
    GameScheduler.Instance.Schedule("InsulationTutorial", 2f, (System.Action<object>) (obj => Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Insulation)), (object) null, (SchedulerGroup) null);
    base.OnSpawn();
  }

  protected override void OnStartWork(Worker worker) => this.operational.SetActive(true);

  protected override void OnStopWork(Worker worker) => this.operational.SetActive(false);

  protected override void OnCompleteWork(Worker worker) => this.operational.SetActive(false);
}
