// Decompiled with JetBrains decompiler
// Type: RanchedStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

public class RanchedStates : GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>
{
  private RanchedStates.RanchStates ranch;
  private GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State wavegoodbye;
  private GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State runaway;
  private GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.ranch;
    this.root.Exit("AbandonedRanchStation", (StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback) (smi => smi.AbandonedRanchStation()));
    this.ranch.EventTransition(GameHashes.RanchStationNoLongerAvailable, (GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State) null, (StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.Transition.ConditionCallback) null).DefaultState((GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State) this.ranch.cheer).Exit(new StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback(RanchedStates.ClearLayerOverride));
    this.ranch.cheer.DefaultState(this.ranch.cheer.pre).ToggleStatusItem((string) CREATURES.STATUSITEMS.EXCITED_TO_BE_RANCHED.NAME, (string) CREATURES.STATUSITEMS.EXCITED_TO_BE_RANCHED.TOOLTIP, category: Db.Get().StatusItemCategories.Main);
    this.ranch.cheer.pre.ScheduleGoTo(0.9f, (StateMachine.BaseState) this.ranch.cheer.cheer);
    this.ranch.cheer.cheer.Enter("FaceRancher", (StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback) (smi => smi.GetComponent<Facing>().Face(smi.GetRanchStation().transform.GetPosition()))).PlayAnim("excited_loop").OnAnimQueueComplete(this.ranch.cheer.pst);
    this.ranch.cheer.pst.ScheduleGoTo(0.2f, (StateMachine.BaseState) this.ranch.move);
    this.ranch.move.DefaultState(this.ranch.move.movetoranch).ToggleStatusItem((string) CREATURES.STATUSITEMS.GETTING_RANCHED.NAME, (string) CREATURES.STATUSITEMS.GETTING_RANCHED.TOOLTIP, category: Db.Get().StatusItemCategories.Main);
    this.ranch.move.movetoranch.Enter("Speedup", (StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback) (smi => smi.GetComponent<Navigator>().defaultSpeed = smi.originalSpeed * 1.25f)).MoveTo(new Func<RanchedStates.Instance, int>(RanchedStates.GetTargetRanchCell), this.ranch.move.getontable).Exit("RestoreSpeed", (StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback) (smi => smi.GetComponent<Navigator>().defaultSpeed = smi.originalSpeed));
    this.ranch.move.getontable.Enter(new StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback(RanchedStates.GetOnTable)).OnAnimQueueComplete(this.ranch.move.waitforranchertobeready);
    this.ranch.move.waitforranchertobeready.Enter("SetCreatureAtRanchingStation", (StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback) (smi => smi.GetRanchStation().Trigger(-1357116271))).EventTransition(GameHashes.RancherReadyAtRanchStation, this.ranch.ranching);
    this.ranch.ranching.Enter(new StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback(RanchedStates.PlayGroomingLoopAnim)).EventTransition(GameHashes.RanchingComplete, this.wavegoodbye).ToggleStatusItem((string) CREATURES.STATUSITEMS.GETTING_RANCHED.NAME, (string) CREATURES.STATUSITEMS.GETTING_RANCHED.TOOLTIP, category: Db.Get().StatusItemCategories.Main);
    this.wavegoodbye.Enter(new StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback(RanchedStates.PlayGroomingPstAnim)).OnAnimQueueComplete(this.runaway).ToggleStatusItem((string) CREATURES.STATUSITEMS.EXCITED_TO_BE_RANCHED.NAME, (string) CREATURES.STATUSITEMS.EXCITED_TO_BE_RANCHED.TOOLTIP, category: Db.Get().StatusItemCategories.Main);
    this.runaway.MoveTo(new Func<RanchedStates.Instance, int>(RanchedStates.GetRunawayCell), this.behaviourcomplete, this.behaviourcomplete).ToggleStatusItem((string) CREATURES.STATUSITEMS.IDLE.NAME, (string) CREATURES.STATUSITEMS.IDLE.TOOLTIP, category: Db.Get().StatusItemCategories.Main);
    this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsToGetRanched);
  }

  private static RanchStation.Instance GetRanchStation(RanchedStates.Instance smi) => smi.GetSMI<RanchableMonitor.Instance>().targetRanchStation;

  private static void ClearLayerOverride(RanchedStates.Instance smi) => smi.Get<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Creatures);

  private static void GetOnTable(RanchedStates.Instance smi)
  {
    Navigator navigator = smi.Get<Navigator>();
    if (navigator.IsValidNavType(NavType.Floor))
      navigator.SetCurrentNavType(NavType.Floor);
    smi.Get<Facing>().SetFacing(false);
    smi.Get<KBatchedAnimController>().Queue(RanchedStates.GetRanchStation(smi).def.ranchedPreAnim);
  }

  private static void PlayGroomingLoopAnim(RanchedStates.Instance smi) => smi.Get<KBatchedAnimController>().Queue(RanchedStates.GetRanchStation(smi).def.ranchedLoopAnim, KAnim.PlayMode.Loop);

  private static void PlayGroomingPstAnim(RanchedStates.Instance smi) => smi.Get<KBatchedAnimController>().Queue(RanchedStates.GetRanchStation(smi).def.ranchedPstAnim);

  private static int GetTargetRanchCell(RanchedStates.Instance smi) => RanchedStates.GetRanchStation(smi).GetTargetRanchCell();

  private static int GetRunawayCell(RanchedStates.Instance smi)
  {
    int cell = Grid.PosToCell(smi.transform.GetPosition());
    int i = Grid.OffsetCell(cell, 2, 0);
    if (Grid.Solid[i])
      i = Grid.OffsetCell(cell, -2, 0);
    return i;
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.GameInstance
  {
    public float originalSpeed;

    public Instance(Chore<RanchedStates.Instance> chore, RanchedStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      this.originalSpeed = this.GetComponent<Navigator>().defaultSpeed;
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.WantsToGetRanched);
    }

    public RanchStation.Instance GetRanchStation() => this.GetSMI<RanchableMonitor.Instance>().targetRanchStation;

    public void AbandonedRanchStation()
    {
      if (this.GetRanchStation() == null)
        return;
      this.GetRanchStation().Trigger(-364750427);
    }
  }

  public class RanchStates : GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State
  {
    public RanchedStates.RanchStates.CheerStates cheer;
    public RanchedStates.RanchStates.MoveStates move;
    public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State ranching;

    public class CheerStates : GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State
    {
      public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State pre;
      public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State cheer;
      public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State pst;
    }

    public class MoveStates : GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State
    {
      public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State movetoranch;
      public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State getontable;
      public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State waitforranchertobeready;
    }
  }
}
