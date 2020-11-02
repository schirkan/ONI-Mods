// Decompiled with JetBrains decompiler
// Type: StressMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using Klei.CustomSettings;
using System;

public class StressMonitor : GameStateMachine<StressMonitor, StressMonitor.Instance>
{
  public GameStateMachine<StressMonitor, StressMonitor.Instance, IStateMachineTarget, object>.State satisfied;
  public StressMonitor.Stressed stressed;
  private const float StressThreshold_One = 60f;
  private const float StressThreshold_Two = 100f;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = true;
    default_state = (StateMachine.BaseState) this.satisfied;
    this.root.Update(nameof (StressMonitor), (System.Action<StressMonitor.Instance, float>) ((smi, dt) => smi.ReportStress(dt)));
    this.satisfied.Transition(this.stressed.tier1, (StateMachine<StressMonitor, StressMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => (double) smi.stress.value >= 60.0)).ToggleExpression(Db.Get().Expressions.Neutral);
    this.stressed.ToggleStatusItem(Db.Get().DuplicantStatusItems.Stressed).Transition(this.satisfied, (StateMachine<StressMonitor, StressMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => (double) smi.stress.value < 60.0)).ToggleReactable((Func<StressMonitor.Instance, Reactable>) (smi => smi.CreateConcernReactable())).TriggerOnEnter(GameHashes.Stressed);
    this.stressed.tier1.Transition(this.stressed.tier2, (StateMachine<StressMonitor, StressMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.HasHadEnough()));
    this.stressed.tier2.TriggerOnEnter(GameHashes.StressedHadEnough).Transition(this.stressed.tier1, (StateMachine<StressMonitor, StressMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.HasHadEnough()));
  }

  public class Stressed : GameStateMachine<StressMonitor, StressMonitor.Instance, IStateMachineTarget, object>.State
  {
    public GameStateMachine<StressMonitor, StressMonitor.Instance, IStateMachineTarget, object>.State tier1;
    public GameStateMachine<StressMonitor, StressMonitor.Instance, IStateMachineTarget, object>.State tier2;
  }

  public new class Instance : GameStateMachine<StressMonitor, StressMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public AmountInstance stress;
    private bool allowStressBreak = true;

    public Instance(IStateMachineTarget master)
      : base(master)
    {
      this.stress = Db.Get().Amounts.Stress.Lookup(this.gameObject);
      this.allowStressBreak = CustomGameSettings.Instance.QualitySettings[CustomGameSettingConfigs.StressBreaks.id].IsDefaultLevel(CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.StressBreaks).id);
    }

    public bool IsStressed() => this.IsInsideState((StateMachine.BaseState) this.sm.stressed);

    public bool HasHadEnough() => this.allowStressBreak && (double) this.stress.value >= 100.0;

    public void ReportStress(float dt)
    {
      for (int i = 0; i != this.stress.deltaAttribute.Modifiers.Count; ++i)
      {
        AttributeModifier modifier = this.stress.deltaAttribute.Modifiers[i];
        DebugUtil.DevAssert(!modifier.IsMultiplier, "Reporting stress for multipliers not supported yet.");
        ReportManager.Instance.ReportValue(ReportManager.ReportType.StressDelta, modifier.Value * dt, modifier.GetDescription(), this.gameObject.GetProperName());
      }
    }

    public Reactable CreateConcernReactable() => (Reactable) new EmoteReactable(this.master.gameObject, (HashedString) "StressConcern", Db.Get().ChoreTypes.Emote, (HashedString) "anim_react_concern_kanim", min_reactor_time: 30f).AddStep(new EmoteReactable.EmoteStep()
    {
      anim = (HashedString) "react"
    });
  }
}
