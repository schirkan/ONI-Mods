// Decompiled with JetBrains decompiler
// Type: EspressoMachineWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Klei.AI;
using TUNING;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/EspressoMachineWorkable")]
public class EspressoMachineWorkable : Workable, IWorkerPrioritizable
{
  [MyCmpReq]
  private Operational operational;
  public int basePriority = RELAXATION.PRIORITY.TIER5;
  private static string specificEffect = "Espresso";
  private static string trackingEffect = "RecentlyRecDrink";

  private EspressoMachineWorkable() => this.SetReportType(ReportManager.ReportType.PersonalTime);

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_espresso_machine_kanim")
    };
    this.showProgressBar = true;
    this.resetProgressOnStop = true;
    this.synchronizeAnims = false;
    this.SetWorkTime(30f);
  }

  protected override void OnStartWork(Worker worker) => this.operational.SetActive(true);

  protected override void OnCompleteWork(Worker worker)
  {
    Storage component1 = this.GetComponent<Storage>();
    SimUtil.DiseaseInfo disease_info1;
    float aggregate_temperature;
    component1.ConsumeAndGetDisease(GameTags.Water, EspressoMachine.WATER_MASS_PER_USE, out disease_info1, out aggregate_temperature);
    SimUtil.DiseaseInfo disease_info2;
    component1.ConsumeAndGetDisease(EspressoMachine.INGREDIENT_TAG, EspressoMachine.INGREDIENT_MASS_PER_USE, out disease_info2, out aggregate_temperature);
    GermExposureMonitor.Instance smi = worker.GetSMI<GermExposureMonitor.Instance>();
    if (smi != null)
    {
      smi.TryInjectDisease(disease_info1.idx, disease_info1.count, GameTags.Water, Sickness.InfectionVector.Digestion);
      smi.TryInjectDisease(disease_info2.idx, disease_info2.count, EspressoMachine.INGREDIENT_TAG, Sickness.InfectionVector.Digestion);
    }
    Effects component2 = worker.GetComponent<Effects>();
    if (!string.IsNullOrEmpty(EspressoMachineWorkable.specificEffect))
      component2.Add(EspressoMachineWorkable.specificEffect, true);
    if (string.IsNullOrEmpty(EspressoMachineWorkable.trackingEffect))
      return;
    component2.Add(EspressoMachineWorkable.trackingEffect, true);
  }

  protected override void OnStopWork(Worker worker) => this.operational.SetActive(false);

  public bool GetWorkerPriority(Worker worker, out int priority)
  {
    priority = this.basePriority;
    Effects component = worker.GetComponent<Effects>();
    if (!string.IsNullOrEmpty(EspressoMachineWorkable.trackingEffect) && component.HasEffect(EspressoMachineWorkable.trackingEffect))
    {
      priority = 0;
      return false;
    }
    if (!string.IsNullOrEmpty(EspressoMachineWorkable.specificEffect) && component.HasEffect(EspressoMachineWorkable.specificEffect))
      priority = RELAXATION.PRIORITY.RECENTLY_USED;
    return true;
  }
}
