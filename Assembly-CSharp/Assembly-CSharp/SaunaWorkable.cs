// Decompiled with JetBrains decompiler
// Type: SaunaWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Klei.AI;
using TUNING;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/SaunaWorkable")]
public class SaunaWorkable : Workable, IWorkerPrioritizable
{
  [MyCmpReq]
  private Operational operational;
  public int basePriority;
  private Sauna sauna;

  private SaunaWorkable() => this.SetReportType(ReportManager.ReportType.PersonalTime);

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_sauna_kanim")
    };
    this.showProgressBar = true;
    this.resetProgressOnStop = true;
    this.synchronizeAnims = true;
    this.workLayer = Grid.SceneLayer.BuildingUse;
    this.SetWorkTime(30f);
    this.sauna = this.GetComponent<Sauna>();
  }

  protected override void OnStartWork(Worker worker)
  {
    base.OnStartWork(worker);
    this.operational.SetActive(true);
    worker.GetComponent<Effects>().Add("SaunaRelaxing", false);
  }

  protected override void OnCompleteWork(Worker worker)
  {
    Effects component = worker.GetComponent<Effects>();
    if (!string.IsNullOrEmpty(this.sauna.specificEffect))
      component.Add(this.sauna.specificEffect, true);
    if (!string.IsNullOrEmpty(this.sauna.trackingEffect))
      component.Add(this.sauna.trackingEffect, true);
    this.operational.SetActive(false);
  }

  protected override void OnStopWork(Worker worker)
  {
    this.operational.SetActive(false);
    worker.GetComponent<Effects>().Remove("SaunaRelaxing");
    Storage component = this.GetComponent<Storage>();
    SimUtil.DiseaseInfo disease_info;
    component.ConsumeAndGetDisease(SimHashes.Steam.CreateTag(), this.sauna.steamPerUseKG, out disease_info, out float _);
    component.AddLiquid(SimHashes.Water, this.sauna.steamPerUseKG, this.sauna.waterOutputTemp, disease_info.idx, disease_info.count, true, false);
  }

  public bool GetWorkerPriority(Worker worker, out int priority)
  {
    priority = this.basePriority;
    Effects component = worker.GetComponent<Effects>();
    if (!string.IsNullOrEmpty(this.sauna.trackingEffect) && component.HasEffect(this.sauna.trackingEffect))
    {
      priority = 0;
      return false;
    }
    if (!string.IsNullOrEmpty(this.sauna.specificEffect) && component.HasEffect(this.sauna.specificEffect))
      priority = RELAXATION.PRIORITY.RECENTLY_USED;
    return true;
  }
}
