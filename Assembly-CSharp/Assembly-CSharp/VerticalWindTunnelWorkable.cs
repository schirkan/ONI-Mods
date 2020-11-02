// Decompiled with JetBrains decompiler
// Type: VerticalWindTunnelWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using TUNING;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/VerticalWindTunnelWorkable")]
public class VerticalWindTunnelWorkable : Workable, IWorkerPrioritizable
{
  public VerticalWindTunnel windTunnel;
  public HashedString overrideAnim;
  public string[] preAnims;
  public string loopAnim;
  public string[] pstAnims;

  private VerticalWindTunnelWorkable() => this.SetReportType(ReportManager.ReportType.PersonalTime);

  public override Workable.AnimInfo GetAnim(Worker worker)
  {
    Workable.AnimInfo anim = base.GetAnim(worker);
    anim.smi = (StateMachine.Instance) new WindTunnelWorkerStateMachine.StatesInstance(worker, this);
    return anim;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.synchronizeAnims = false;
    this.showProgressBar = true;
    this.resetProgressOnStop = true;
    this.SetWorkTime(90f);
  }

  protected override void OnStartWork(Worker worker)
  {
    base.OnStartWork(worker);
    worker.GetComponent<Effects>().Add("VerticalWindTunnelFlying", false);
  }

  protected override void OnStopWork(Worker worker)
  {
    base.OnStopWork(worker);
    worker.GetComponent<Effects>().Remove("VerticalWindTunnelFlying");
  }

  protected override void OnCompleteWork(Worker worker)
  {
    Effects component = worker.GetComponent<Effects>();
    component.Add(this.windTunnel.trackingEffect, true);
    component.Add(this.windTunnel.specificEffect, true);
  }

  public bool GetWorkerPriority(Worker worker, out int priority)
  {
    priority = this.windTunnel.basePriority;
    Effects component = worker.GetComponent<Effects>();
    if (component.HasEffect(this.windTunnel.trackingEffect))
    {
      priority = 0;
      return false;
    }
    if (component.HasEffect(this.windTunnel.specificEffect))
      priority = RELAXATION.PRIORITY.RECENTLY_USED;
    return true;
  }
}
