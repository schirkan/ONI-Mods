// Decompiled with JetBrains decompiler
// Type: BalloonStandConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

public class BalloonStandConfig : IEntityConfig
{
  public static readonly string ID = "BalloonStand";
  private Chore.Precondition HasNoBalloon = new Chore.Precondition()
  {
    id = nameof (HasNoBalloon),
    description = "Duplicant doesn't have a balloon already",
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => !((UnityEngine.Object) context.consumerState.consumer == (UnityEngine.Object) null) && !context.consumerState.gameObject.GetComponent<Effects>().HasEffect("HasBalloon"))
  };

  public GameObject CreatePrefab()
  {
    GameObject entity = EntityTemplates.CreateEntity(BalloonStandConfig.ID, BalloonStandConfig.ID, false);
    KAnimFile[] kanimFileArray = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_balloon_receiver_kanim")
    };
    GetBalloonWorkable getBalloonWorkable = entity.AddOrGet<GetBalloonWorkable>();
    getBalloonWorkable.workTime = 2f;
    getBalloonWorkable.workLayer = Grid.SceneLayer.BuildingFront;
    getBalloonWorkable.overrideAnims = kanimFileArray;
    getBalloonWorkable.synchronizeAnims = false;
    return entity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
    GetBalloonWorkable component = inst.GetComponent<GetBalloonWorkable>();
    WorkChore<GetBalloonWorkable> workChore = new WorkChore<GetBalloonWorkable>(Db.Get().ChoreTypes.JoyReaction, (IStateMachineTarget) component, on_complete: new System.Action<Chore>(this.MakeNewBalloonChore), schedule_block: Db.Get().ScheduleBlockTypes.Recreation, priority_class: PriorityScreen.PriorityClass.high, ignore_building_assignment: true);
    workChore.AddPrecondition(this.HasNoBalloon, (object) workChore);
  }

  private void MakeNewBalloonChore(Chore chore)
  {
    GetBalloonWorkable component = chore.target.GetComponent<GetBalloonWorkable>();
    WorkChore<GetBalloonWorkable> workChore = new WorkChore<GetBalloonWorkable>(Db.Get().ChoreTypes.JoyReaction, (IStateMachineTarget) component, on_complete: new System.Action<Chore>(this.MakeNewBalloonChore), schedule_block: Db.Get().ScheduleBlockTypes.Recreation, priority_class: PriorityScreen.PriorityClass.high, ignore_building_assignment: true);
    workChore.AddPrecondition(this.HasNoBalloon, (object) workChore);
  }
}
