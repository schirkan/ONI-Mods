﻿// Decompiled with JetBrains decompiler
// Type: PhonoboxWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using TUNING;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/PhonoboxWorkable")]
public class PhonoboxWorkable : Workable, IWorkerPrioritizable
{
  public Phonobox owner;
  public int basePriority = RELAXATION.PRIORITY.TIER3;
  public string specificEffect = "Danced";
  public string trackingEffect = "RecentlyDanced";
  public KAnimFile[][] workerOverrideAnims = new KAnimFile[3][]
  {
    new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_phonobox_danceone_kanim")
    },
    new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_phonobox_dancetwo_kanim")
    },
    new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_phonobox_dancethree_kanim")
    }
  };

  private PhonoboxWorkable() => this.SetReportType(ReportManager.ReportType.PersonalTime);

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.synchronizeAnims = false;
    this.showProgressBar = true;
    this.resetProgressOnStop = true;
    this.SetWorkTime(15f);
  }

  protected override void OnCompleteWork(Worker worker)
  {
    Effects component = worker.GetComponent<Effects>();
    if (!string.IsNullOrEmpty(this.trackingEffect))
      component.Add(this.trackingEffect, true);
    if (string.IsNullOrEmpty(this.specificEffect))
      return;
    component.Add(this.specificEffect, true);
  }

  public bool GetWorkerPriority(Worker worker, out int priority)
  {
    priority = this.basePriority;
    Effects component = worker.GetComponent<Effects>();
    if (!string.IsNullOrEmpty(this.trackingEffect) && component.HasEffect(this.trackingEffect))
    {
      priority = 0;
      return false;
    }
    if (!string.IsNullOrEmpty(this.specificEffect) && component.HasEffect(this.specificEffect))
      priority = RELAXATION.PRIORITY.RECENTLY_USED;
    return true;
  }

  protected override void OnStartWork(Worker worker)
  {
    this.owner.AddWorker(worker);
    worker.GetComponent<Effects>().Add("Dancing", false);
  }

  protected override void OnStopWork(Worker worker)
  {
    this.owner.RemoveWorker(worker);
    worker.GetComponent<Effects>().Remove("Dancing");
  }

  public override Workable.AnimInfo GetAnim(Worker worker)
  {
    this.overrideAnims = this.workerOverrideAnims[Random.Range(0, this.workerOverrideAnims.Length)];
    return base.GetAnim(worker);
  }
}
