// Decompiled with JetBrains decompiler
// Type: EggProtector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class EggProtector : GameStateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>
{
  public StateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>.BoolParameter needsToMoveCloser;
  public StateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>.BoolParameter hasEggToGuard;
  public GameStateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>.State idle;
  public EggProtector.GuardingStates guarding;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.idle;
    this.idle.ParamTransition<bool>((StateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>.Parameter<bool>) this.hasEggToGuard, this.guarding.idle, GameStateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>.IsTrue).EventHandler(GameHashes.LayEgg, (StateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>.State.Callback) (smi => smi.FindEggToGuard())).Update((System.Action<EggProtector.Instance, float>) ((smi, dt) => smi.FindEggToGuard()), UpdateRate.SIM_4000ms);
    this.guarding.Enter((StateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>.State.Callback) (smi =>
    {
      smi.gameObject.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim((HashedString) "pincher_kanim"), postfix: "_heat");
      smi.gameObject.AddOrGet<FactionAlignment>().SwitchAlignment(FactionManager.FactionID.Hostile);
    })).Exit((StateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>.State.Callback) (smi =>
    {
      smi.gameObject.AddOrGet<SymbolOverrideController>().RemoveBuildOverride(Assets.GetAnim((HashedString) "pincher_kanim").GetData());
      smi.gameObject.AddOrGet<FactionAlignment>().SwitchAlignment(FactionManager.FactionID.Pest);
    })).ParamTransition<bool>((StateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>.Parameter<bool>) this.hasEggToGuard, this.idle, GameStateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>.IsFalse).Update((System.Action<EggProtector.Instance, float>) ((smi, dt) => smi.CanProtectEgg()), UpdateRate.SIM_1000ms);
    this.guarding.idle.ParamTransition<bool>((StateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>.Parameter<bool>) this.needsToMoveCloser, this.guarding.return_to_egg, GameStateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>.IsTrue);
    this.guarding.return_to_egg.MoveTo((Func<EggProtector.Instance, int>) (smi => smi.GetEggPos()), update_cell: true).ParamTransition<bool>((StateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>.Parameter<bool>) this.needsToMoveCloser, this.guarding.idle, GameStateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>.IsFalse);
  }

  public class Def : StateMachine.BaseDef
  {
    public Tag protectorTag;
    public bool shouldProtect;

    public Def(Tag tag, bool shouldProtect)
    {
      this.protectorTag = tag;
      this.shouldProtect = shouldProtect;
    }
  }

  public new class Instance : GameStateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>.GameInstance
  {
    public GameObject eggToGuard;

    public Instance(Chore<EggProtector.Instance> chore, EggProtector.Def def)
      : base((IStateMachineTarget) chore, def)
      => this.gameObject.GetSMI<EntityThreatMonitor.Instance>().allyTag = def.protectorTag;

    public void CheckDistanceToEgg()
    {
      int navigationCost = this.smi.GetComponent<Navigator>().GetNavigationCost(Grid.PosToCell(this.eggToGuard));
      if (navigationCost > 20)
      {
        this.sm.needsToMoveCloser.Set(true, this.smi);
      }
      else
      {
        if (navigationCost >= 0)
          return;
        this.sm.needsToMoveCloser.Set(false, this.smi);
      }
    }

    public void CanProtectEgg()
    {
      bool flag = true;
      if ((UnityEngine.Object) this.eggToGuard == (UnityEngine.Object) null)
        flag = false;
      Navigator component = this.smi.GetComponent<Navigator>();
      if (flag)
      {
        int num = 150;
        int navigationCost = component.GetNavigationCost(Grid.PosToCell(this.eggToGuard));
        if (navigationCost == -1 || navigationCost >= num)
          flag = false;
      }
      if (flag)
        return;
      this.SetEggToGuard((GameObject) null);
    }

    public void FindEggToGuard()
    {
      if (!this.def.shouldProtect)
        return;
      GameObject egg = (GameObject) null;
      int num = 100;
      Navigator component = this.smi.GetComponent<Navigator>();
      foreach (Pickupable pickupable in Components.Pickupables)
      {
        if (pickupable.HasTag("CrabEgg".ToTag()) && (double) Vector2.Distance((Vector2) this.smi.transform.position, (Vector2) pickupable.transform.position) <= 25.0)
        {
          int navigationCost = component.GetNavigationCost(Grid.PosToCell((KMonoBehaviour) pickupable));
          if (navigationCost != -1 && navigationCost < num)
          {
            egg = pickupable.gameObject;
            num = navigationCost;
          }
        }
      }
      this.SetEggToGuard(egg);
    }

    public void SetEggToGuard(GameObject egg)
    {
      this.eggToGuard = egg;
      this.gameObject.GetSMI<EntityThreatMonitor.Instance>().entityToProtect = egg;
      this.sm.hasEggToGuard.Set((UnityEngine.Object) egg != (UnityEngine.Object) null, this.smi);
    }

    public int GetEggPos() => Grid.PosToCell(this.eggToGuard);
  }

  public class GuardingStates : GameStateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>.State
  {
    public GameStateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>.State idle;
    public GameStateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>.State return_to_egg;
  }
}
