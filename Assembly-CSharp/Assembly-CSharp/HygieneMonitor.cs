﻿// Decompiled with JetBrains decompiler
// Type: HygieneMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;

public class HygieneMonitor : GameStateMachine<HygieneMonitor, HygieneMonitor.Instance>
{
  public StateMachine<HygieneMonitor, HygieneMonitor.Instance, IStateMachineTarget, object>.FloatParameter dirtiness;
  public GameStateMachine<HygieneMonitor, HygieneMonitor.Instance, IStateMachineTarget, object>.State clean;
  public GameStateMachine<HygieneMonitor, HygieneMonitor.Instance, IStateMachineTarget, object>.State needsshower;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.needsshower;
    this.serializable = true;
    this.clean.EventTransition(GameHashes.EffectRemoved, this.needsshower, (StateMachine<HygieneMonitor, HygieneMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.NeedsShower()));
    this.needsshower.EventTransition(GameHashes.EffectAdded, this.clean, (StateMachine<HygieneMonitor, HygieneMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.NeedsShower())).ToggleUrge(Db.Get().Urges.Shower).Enter((StateMachine<HygieneMonitor, HygieneMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.SetDirtiness(1f)));
  }

  public new class Instance : GameStateMachine<HygieneMonitor, HygieneMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    private Effects effects;

    public Instance(IStateMachineTarget master)
      : base(master)
      => this.effects = master.GetComponent<Effects>();

    public float GetDirtiness() => this.sm.dirtiness.Get(this);

    public void SetDirtiness(float dirtiness)
    {
      double num = (double) this.sm.dirtiness.Set(dirtiness, this);
    }

    public bool NeedsShower() => !this.effects.HasEffect(Shower.SHOWER_EFFECT);
  }
}
