﻿// Decompiled with JetBrains decompiler
// Type: TreeClimbStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

public class TreeClimbStates : GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>
{
  public GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.ApproachSubState<Uprootable> moving;
  public TreeClimbStates.ClimbState climbing;
  public GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State behaviourcomplete;
  public StateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.TargetParameter target;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.moving;
    this.root.Enter(new StateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State.Callback(TreeClimbStates.SetTarget)).Enter((StateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State.Callback) (smi =>
    {
      if (TreeClimbStates.ReserveClimbable(smi))
        return;
      smi.GoTo((StateMachine.BaseState) this.behaviourcomplete);
    })).Exit(new StateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State.Callback(TreeClimbStates.UnreserveClimbable)).ToggleStatusItem((string) CREATURES.STATUSITEMS.RUMMAGINGSEED.NAME, (string) CREATURES.STATUSITEMS.RUMMAGINGSEED.TOOLTIP, category: Db.Get().StatusItemCategories.Main);
    this.moving.MoveTo(new Func<TreeClimbStates.Instance, int>(TreeClimbStates.GetClimbableCell), (GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State) this.climbing, this.behaviourcomplete, false);
    this.climbing.DefaultState(this.climbing.pre);
    this.climbing.pre.PlayAnim("rummage_pre").OnAnimQueueComplete(this.climbing.loop);
    this.climbing.loop.QueueAnim("rummage_loop", true).ScheduleGoTo(3.5f, (StateMachine.BaseState) this.climbing.pst).Update(new System.Action<TreeClimbStates.Instance, float>(TreeClimbStates.Rummage), UpdateRate.SIM_1000ms);
    this.climbing.pst.QueueAnim("rummage_pst").OnAnimQueueComplete(this.behaviourcomplete);
    this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsToClimbTree);
  }

  private static void SetTarget(TreeClimbStates.Instance smi) => smi.sm.target.Set(smi.GetSMI<ClimbableTreeMonitor.Instance>().climbTarget, smi);

  private static bool ReserveClimbable(TreeClimbStates.Instance smi)
  {
    GameObject go = smi.sm.target.Get(smi);
    if (!((UnityEngine.Object) go != (UnityEngine.Object) null) || go.HasTag(GameTags.Creatures.ReservedByCreature))
      return false;
    go.AddTag(GameTags.Creatures.ReservedByCreature);
    return true;
  }

  private static void UnreserveClimbable(TreeClimbStates.Instance smi)
  {
    GameObject go = smi.sm.target.Get(smi);
    if (!((UnityEngine.Object) go != (UnityEngine.Object) null))
      return;
    go.RemoveTag(GameTags.Creatures.ReservedByCreature);
  }

  private static void Rummage(TreeClimbStates.Instance smi, float dt)
  {
    GameObject gameObject1 = smi.sm.target.Get(smi);
    if (!((UnityEngine.Object) gameObject1 != (UnityEngine.Object) null))
      return;
    BuddingTrunk component1 = gameObject1.GetComponent<BuddingTrunk>();
    if ((bool) (UnityEngine.Object) component1)
    {
      component1.ExtractExtraSeed();
    }
    else
    {
      Storage component2 = gameObject1.GetComponent<Storage>();
      if (!(bool) (UnityEngine.Object) component2 || component2.items.Count <= 0)
        return;
      int index = UnityEngine.Random.Range(0, component2.items.Count - 1);
      GameObject gameObject2 = component2.items[index];
      Pickupable pu = (bool) (UnityEngine.Object) gameObject2 ? gameObject2.GetComponent<Pickupable>() : (Pickupable) null;
      if (!(bool) (UnityEngine.Object) pu || (double) pu.UnreservedAmount <= 0.00999999977648258)
        return;
      smi.Toss(pu);
    }
  }

  private static int GetClimbableCell(TreeClimbStates.Instance smi) => Grid.PosToCell(smi.sm.target.Get(smi));

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.GameInstance
  {
    private Storage storage;
    private static readonly Vector2 VEL_MIN = new Vector2(-1f, 2f);
    private static readonly Vector2 VEL_MAX = new Vector2(1f, 4f);

    public Instance(Chore<TreeClimbStates.Instance> chore, TreeClimbStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.WantsToClimbTree);
      this.storage = this.GetComponent<Storage>();
    }

    public void Toss(Pickupable pu)
    {
      Pickupable pickupable = pu.Take(Mathf.Min(1f, pu.UnreservedAmount));
      if (!((UnityEngine.Object) pickupable != (UnityEngine.Object) null))
        return;
      this.storage.Store(pickupable.gameObject, true);
      this.storage.Drop(pickupable.gameObject);
      this.Throw(pickupable.gameObject);
    }

    private void Throw(GameObject ore_go)
    {
      Vector3 position = this.transform.GetPosition();
      position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
      int cell = Grid.PosToCell(position);
      int num = Grid.CellAbove(cell);
      Vector2 initial_velocity;
      if (Grid.IsValidCell(cell) && Grid.Solid[cell] || Grid.IsValidCell(num) && Grid.Solid[num])
      {
        initial_velocity = Vector2.zero;
      }
      else
      {
        position.y += 0.5f;
        initial_velocity = new Vector2(UnityEngine.Random.Range(TreeClimbStates.Instance.VEL_MIN.x, TreeClimbStates.Instance.VEL_MAX.x), UnityEngine.Random.Range(TreeClimbStates.Instance.VEL_MIN.y, TreeClimbStates.Instance.VEL_MAX.y));
      }
      ore_go.transform.SetPosition(position);
      if (GameComps.Fallers.Has((object) ore_go))
        GameComps.Fallers.Remove(ore_go);
      GameComps.Fallers.Add(ore_go, initial_velocity);
    }
  }

  public class ClimbState : GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State
  {
    public GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State pre;
    public GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State loop;
    public GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State pst;
  }
}
