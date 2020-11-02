// Decompiled with JetBrains decompiler
// Type: AstronautTrainingCenter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/AstronautTrainingCenter")]
public class AstronautTrainingCenter : Workable
{
  public float daysToMasterRole;
  private Chore chore;
  public Chore.Precondition IsNotMarkedForDeconstruction = new Chore.Precondition()
  {
    id = nameof (IsNotMarkedForDeconstruction),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_MARKED_FOR_DECONSTRUCTION,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      Deconstructable deconstructable = data as Deconstructable;
      return (Object) deconstructable == (Object) null || !deconstructable.IsMarkedForDeconstruction();
    })
  };

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.chore = this.CreateChore();
  }

  private Chore CreateChore() => (Chore) new WorkChore<AstronautTrainingCenter>(Db.Get().ChoreTypes.Train, (IStateMachineTarget) this, allow_in_red_alert: false);

  protected override void OnStartWork(Worker worker)
  {
    base.OnStartWork(worker);
    this.GetComponent<Operational>().SetActive(true);
  }

  protected override bool OnWorkTick(Worker worker, float dt)
  {
    int num = (Object) worker == (Object) null ? 1 : 0;
    return true;
  }

  protected override void OnCompleteWork(Worker worker)
  {
    base.OnCompleteWork(worker);
    if (this.chore != null && !this.chore.isComplete)
      this.chore.Cancel("completed but not complete??");
    this.chore = this.CreateChore();
  }

  protected override void OnStopWork(Worker worker)
  {
    base.OnStopWork(worker);
    this.GetComponent<Operational>().SetActive(false);
  }

  public override float GetPercentComplete()
  {
    int num = (Object) this.worker == (Object) null ? 1 : 0;
    return 0.0f;
  }
}
