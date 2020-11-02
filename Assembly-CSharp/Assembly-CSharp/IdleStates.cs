﻿// Decompiled with JetBrains decompiler
// Type: IdleStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

public class IdleStates : GameStateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>
{
  private GameStateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>.State loop;
  private GameStateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>.State move;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.loop;
    this.root.Exit("StopNavigator", (StateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>.State.Callback) (smi => smi.GetComponent<Navigator>().Stop())).ToggleStatusItem((string) CREATURES.STATUSITEMS.IDLE.NAME, (string) CREATURES.STATUSITEMS.IDLE.TOOLTIP, category: Db.Get().StatusItemCategories.Main).ToggleTag(GameTags.Idle);
    this.loop.Enter(new StateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>.State.Callback(this.PlayIdle)).ToggleScheduleCallback("IdleMove", (Func<IdleStates.Instance, float>) (smi => (float) UnityEngine.Random.Range(3, 10)), (System.Action<IdleStates.Instance>) (smi => smi.GoTo((StateMachine.BaseState) this.move)));
    this.move.Enter(new StateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>.State.Callback(this.MoveToNewCell)).EventTransition(GameHashes.DestinationReached, this.loop).EventTransition(GameHashes.NavigationFailed, this.loop);
  }

  public void MoveToNewCell(IdleStates.Instance smi)
  {
    Navigator component = smi.GetComponent<Navigator>();
    IdleStates.MoveCellQuery moveCellQuery = new IdleStates.MoveCellQuery(component.CurrentNavType);
    moveCellQuery.allowLiquid = smi.gameObject.HasTag(GameTags.Amphibious);
    component.RunQuery((PathFinderQuery) moveCellQuery);
    component.GoTo(moveCellQuery.GetResultCell());
  }

  public void PlayIdle(IdleStates.Instance smi)
  {
    KAnimControllerBase component1 = smi.GetComponent<KAnimControllerBase>();
    Navigator component2 = smi.GetComponent<Navigator>();
    NavType nav_type = component2.CurrentNavType;
    if (smi.GetComponent<Facing>().GetFacing())
      nav_type = NavGrid.MirrorNavType(nav_type);
    if (smi.def.customIdleAnim != null)
    {
      HashedString invalid = HashedString.Invalid;
      HashedString anim_name = smi.def.customIdleAnim(smi, ref invalid);
      if (anim_name != HashedString.Invalid)
      {
        if (invalid != HashedString.Invalid)
          component1.Play(invalid);
        component1.Queue(anim_name, KAnim.PlayMode.Loop);
        return;
      }
    }
    HashedString idleAnim = component2.NavGrid.GetIdleAnim(nav_type);
    component1.Play(idleAnim, KAnim.PlayMode.Loop);
  }

  public class Def : StateMachine.BaseDef
  {
    public IdleStates.Def.IdleAnimCallback customIdleAnim;

    public delegate HashedString IdleAnimCallback(
      IdleStates.Instance smi,
      ref HashedString pre_anim);
  }

  public new class Instance : GameStateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>.GameInstance
  {
    public Instance(Chore<IdleStates.Instance> chore, IdleStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
    }
  }

  public class MoveCellQuery : PathFinderQuery
  {
    private NavType navType;
    private int targetCell = Grid.InvalidCell;
    private int maxIterations;

    public bool allowLiquid { get; set; }

    public MoveCellQuery(NavType navType)
    {
      this.navType = navType;
      this.maxIterations = UnityEngine.Random.Range(5, 25);
    }

    public override bool IsMatch(int cell, int parent_cell, int cost)
    {
      if (!Grid.IsValidCell(cell))
        return false;
      bool flag1 = this.navType != NavType.Swim;
      bool flag2 = this.navType == NavType.Swim || this.allowLiquid;
      bool flag3 = Grid.IsSubstantialLiquid(cell);
      if (flag3 && !flag2 || !flag3 && !flag1)
        return false;
      this.targetCell = cell;
      return --this.maxIterations <= 0;
    }

    public override int GetResultCell() => this.targetCell;
  }
}
