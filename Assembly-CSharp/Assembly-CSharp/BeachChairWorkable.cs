﻿// Decompiled with JetBrains decompiler
// Type: BeachChairWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using TUNING;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/BeachChairWorkable")]
public class BeachChairWorkable : Workable, IWorkerPrioritizable
{
  [MyCmpReq]
  private Operational operational;
  private float timeLit;
  public string soundPath = GlobalAssets.GetSound("BeachChair_music_lp");
  public HashedString BEACH_CHAIR_LIT_PARAMETER = (HashedString) "beachChair_lit";
  public int basePriority;
  private BeachChair beachChair;

  private BeachChairWorkable() => this.SetReportType(ReportManager.ReportType.PersonalTime);

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_beach_chair_kanim")
    };
    this.workAnims = (HashedString[]) null;
    this.workingPstComplete = (HashedString[]) null;
    this.workingPstFailed = (HashedString[]) null;
    this.showProgressBar = true;
    this.resetProgressOnStop = true;
    this.synchronizeAnims = false;
    this.lightEfficiencyBonus = false;
    this.SetWorkTime(150f);
    this.beachChair = this.GetComponent<BeachChair>();
  }

  protected override void OnStartWork(Worker worker)
  {
    this.timeLit = 0.0f;
    this.beachChair.SetWorker(worker);
    this.operational.SetActive(true);
    worker.GetComponent<Effects>().Add("BeachChairRelaxing", false);
  }

  protected override bool OnWorkTick(Worker worker, float dt)
  {
    int cell = Grid.PosToCell(this.gameObject);
    bool v = (double) Grid.LightIntensity[cell] >= 9999.0;
    this.beachChair.SetLit(v);
    if (v)
    {
      this.GetComponent<LoopingSounds>().SetParameter(this.soundPath, this.BEACH_CHAIR_LIT_PARAMETER, 1f);
      this.timeLit += dt;
    }
    else
      this.GetComponent<LoopingSounds>().SetParameter(this.soundPath, this.BEACH_CHAIR_LIT_PARAMETER, 0.0f);
    return false;
  }

  protected override void OnCompleteWork(Worker worker)
  {
    Effects component = worker.GetComponent<Effects>();
    if ((double) this.timeLit / (double) this.workTime >= 0.75)
    {
      component.Add(this.beachChair.specificEffectLit, true);
      component.Remove(this.beachChair.specificEffectUnlit);
    }
    else
    {
      component.Add(this.beachChair.specificEffectUnlit, true);
      component.Remove(this.beachChair.specificEffectLit);
    }
    component.Add(this.beachChair.trackingEffect, true);
  }

  protected override void OnStopWork(Worker worker)
  {
    this.operational.SetActive(false);
    worker.GetComponent<Effects>().Remove("BeachChairRelaxing");
  }

  public bool GetWorkerPriority(Worker worker, out int priority)
  {
    priority = this.basePriority;
    Effects component = worker.GetComponent<Effects>();
    if (component.HasEffect(this.beachChair.trackingEffect))
    {
      priority = 0;
      return false;
    }
    if (component.HasEffect(this.beachChair.specificEffectLit) || component.HasEffect(this.beachChair.specificEffectUnlit))
      priority = RELAXATION.PRIORITY.RECENTLY_USED;
    return true;
  }
}
