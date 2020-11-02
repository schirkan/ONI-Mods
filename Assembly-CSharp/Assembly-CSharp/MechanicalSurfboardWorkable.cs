// Decompiled with JetBrains decompiler
// Type: MechanicalSurfboardWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Klei.AI;
using TUNING;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/MechanicalSurfboardWorkable")]
public class MechanicalSurfboardWorkable : Workable, IWorkerPrioritizable
{
  [MyCmpReq]
  private Operational operational;
  public int basePriority;
  private MechanicalSurfboard surfboard;

  private MechanicalSurfboardWorkable() => this.SetReportType(ReportManager.ReportType.PersonalTime);

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.showProgressBar = true;
    this.resetProgressOnStop = true;
    this.synchronizeAnims = true;
    this.SetWorkTime(30f);
    this.surfboard = this.GetComponent<MechanicalSurfboard>();
  }

  protected override void OnStartWork(Worker worker)
  {
    this.operational.SetActive(true);
    worker.GetComponent<Effects>().Add("MechanicalSurfing", false);
  }

  public override Workable.AnimInfo GetAnim(Worker worker)
  {
    Workable.AnimInfo animInfo = new Workable.AnimInfo();
    AttributeInstance attributeInstance = worker.GetAttributes().Get(Db.Get().Attributes.Athletics);
    if ((double) attributeInstance.GetTotalValue() <= 7.0)
      animInfo.overrideAnims = new KAnimFile[1]
      {
        Assets.GetAnim((HashedString) this.surfboard.interactAnims[0])
      };
    else if ((double) attributeInstance.GetTotalValue() <= 15.0)
      animInfo.overrideAnims = new KAnimFile[1]
      {
        Assets.GetAnim((HashedString) this.surfboard.interactAnims[1])
      };
    else
      animInfo.overrideAnims = new KAnimFile[1]
      {
        Assets.GetAnim((HashedString) this.surfboard.interactAnims[2])
      };
    return animInfo;
  }

  protected override bool OnWorkTick(Worker worker, float dt)
  {
    Building component1 = this.GetComponent<Building>();
    MechanicalSurfboard component2 = this.GetComponent<MechanicalSurfboard>();
    int widthInCells = component1.Def.WidthInCells;
    int x = Random.Range(-(widthInCells - 1) / 2, widthInCells / 2);
    float num = component2.waterSpillRateKG * dt;
    SimUtil.DiseaseInfo disease_info;
    float aggregate_temperature;
    this.GetComponent<Storage>().ConsumeAndGetDisease(SimHashes.Water.CreateTag(), num, out disease_info, out aggregate_temperature);
    FallingWater.instance.AddParticle(Grid.OffsetCell(Grid.PosToCell(this.gameObject), new CellOffset(x, 0)), (byte) ElementLoader.GetElementIndex(SimHashes.Water), num, aggregate_temperature, disease_info.idx, disease_info.count, true);
    return false;
  }

  protected override void OnCompleteWork(Worker worker)
  {
    Effects component = worker.GetComponent<Effects>();
    if (!string.IsNullOrEmpty(this.surfboard.specificEffect))
      component.Add(this.surfboard.specificEffect, true);
    if (string.IsNullOrEmpty(this.surfboard.trackingEffect))
      return;
    component.Add(this.surfboard.trackingEffect, true);
  }

  protected override void OnStopWork(Worker worker)
  {
    this.operational.SetActive(false);
    worker.GetComponent<Effects>().Remove("MechanicalSurfing");
  }

  public bool GetWorkerPriority(Worker worker, out int priority)
  {
    priority = this.basePriority;
    Effects component = worker.GetComponent<Effects>();
    if (!string.IsNullOrEmpty(this.surfboard.trackingEffect) && component.HasEffect(this.surfboard.trackingEffect))
    {
      priority = 0;
      return false;
    }
    if (!string.IsNullOrEmpty(this.surfboard.specificEffect) && component.HasEffect(this.surfboard.specificEffect))
      priority = RELAXATION.PRIORITY.RECENTLY_USED;
    return true;
  }
}
