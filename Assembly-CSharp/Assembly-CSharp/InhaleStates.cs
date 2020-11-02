// Decompiled with JetBrains decompiler
// Type: InhaleStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

public class InhaleStates : GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>
{
  public GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State goingtoeat;
  public InhaleStates.InhalingStates inhaling;
  public GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State behaviourcomplete;
  public StateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.IntParameter targetCell;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.goingtoeat;
    this.root.Enter("SetTarget", (StateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State.Callback) (smi => this.targetCell.Set(smi.GetSMI<GasAndLiquidConsumerMonitor.Instance>().targetCell, smi)));
    this.goingtoeat.MoveTo((Func<InhaleStates.Instance, int>) (smi => this.targetCell.Get(smi)), (GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State) this.inhaling).ToggleStatusItem((string) CREATURES.STATUSITEMS.LOOKINGFORFOOD.NAME, (string) CREATURES.STATUSITEMS.LOOKINGFORFOOD.TOOLTIP, category: Db.Get().StatusItemCategories.Main);
    this.inhaling.DefaultState(this.inhaling.pre).ToggleStatusItem((string) CREATURES.STATUSITEMS.INHALING.NAME, (string) CREATURES.STATUSITEMS.INHALING.TOOLTIP, category: Db.Get().StatusItemCategories.Main);
    this.inhaling.pre.PlayAnim("inhale_pre").QueueAnim("inhale_loop", true).Update("Consume", (System.Action<InhaleStates.Instance, float>) ((smi, dt) => smi.GetSMI<GasAndLiquidConsumerMonitor.Instance>().Consume(dt))).EventTransition(GameHashes.ElementNoLongerAvailable, this.inhaling.pst).Enter("StartInhaleSound", (StateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State.Callback) (smi => smi.StartInhaleSound())).Exit("StopInhaleSound", (StateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State.Callback) (smi => smi.StopInhaleSound())).ScheduleGoTo((Func<InhaleStates.Instance, float>) (smi => smi.def.inhaleTime), (StateMachine.BaseState) this.inhaling.pst);
    this.inhaling.pst.Transition(this.inhaling.full, new StateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.Transition.ConditionCallback(InhaleStates.IsFull)).Transition(this.behaviourcomplete, GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.Not(new StateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.Transition.ConditionCallback(InhaleStates.IsFull)));
    this.inhaling.full.QueueAnim("inhale_pst").OnAnimQueueComplete(this.behaviourcomplete);
    this.behaviourcomplete.PlayAnim("idle_loop", KAnim.PlayMode.Loop).BehaviourComplete(GameTags.Creatures.WantsToEat);
  }

  private static bool IsFull(InhaleStates.Instance smi)
  {
    CreatureCalorieMonitor.Instance smi1 = smi.GetSMI<CreatureCalorieMonitor.Instance>();
    return smi1 != null && (double) smi1.stomach.GetFullness() >= 1.0;
  }

  public class Def : StateMachine.BaseDef
  {
    public string inhaleSound;
    public float inhaleTime = 3f;
  }

  public new class Instance : GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.GameInstance
  {
    public string inhaleSound;

    public Instance(Chore<InhaleStates.Instance> chore, InhaleStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.WantsToEat);
      this.inhaleSound = GlobalAssets.GetSound(def.inhaleSound);
    }

    public void StartInhaleSound()
    {
      LoopingSounds component = this.GetComponent<LoopingSounds>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      component.StartSound(this.smi.inhaleSound);
    }

    public void StopInhaleSound()
    {
      LoopingSounds component = this.GetComponent<LoopingSounds>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      component.StopSound(this.smi.inhaleSound);
    }
  }

  public class InhalingStates : GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State
  {
    public GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State pre;
    public GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State pst;
    public GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State full;
  }
}
