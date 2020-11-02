// Decompiled with JetBrains decompiler
// Type: JoyBehaviourMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using System;
using TUNING;

public class JoyBehaviourMonitor : GameStateMachine<JoyBehaviourMonitor, JoyBehaviourMonitor.Instance>
{
  public StateMachine<JoyBehaviourMonitor, JoyBehaviourMonitor.Instance, IStateMachineTarget, object>.Signal exitEarly;
  public GameStateMachine<JoyBehaviourMonitor, JoyBehaviourMonitor.Instance, IStateMachineTarget, object>.State neutral;
  public GameStateMachine<JoyBehaviourMonitor, JoyBehaviourMonitor.Instance, IStateMachineTarget, object>.State overjoyed;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.neutral;
    this.serializable = true;
    this.root.TagTransition(GameTags.Dead, (GameStateMachine<JoyBehaviourMonitor, JoyBehaviourMonitor.Instance, IStateMachineTarget, object>.State) null);
    this.neutral.EventHandler(GameHashes.SleepFinished, (StateMachine<JoyBehaviourMonitor, JoyBehaviourMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi =>
    {
      if (!smi.ShouldBeOverjoyed())
        return;
      smi.GoToOverjoyed();
    }));
    this.overjoyed.Transition(this.neutral, (StateMachine<JoyBehaviourMonitor, JoyBehaviourMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => (double) GameClock.Instance.GetTime() >= (double) smi.transitionTime)).ToggleExpression((Func<JoyBehaviourMonitor.Instance, Expression>) (smi => smi.happyExpression)).ToggleAnims((Func<JoyBehaviourMonitor.Instance, HashedString>) (smi => (HashedString) smi.happyLocoAnim)).ToggleAnims((Func<JoyBehaviourMonitor.Instance, HashedString>) (smi => (HashedString) smi.happyLocoWalkAnim)).ToggleTag(GameTags.Overjoyed).OnSignal(this.exitEarly, this.neutral);
  }

  public new class Instance : GameStateMachine<JoyBehaviourMonitor, JoyBehaviourMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public string happyLocoAnim = "";
    public string happyLocoWalkAnim = "";
    public Expression happyExpression;
    [Serialize]
    public float transitionTime;
    private AttributeInstance expectationAttribute;
    private AttributeInstance qolAttribute;

    public Instance(
      IStateMachineTarget master,
      string happy_loco_anim,
      string happy_loco_walk_anim,
      Expression happy_expression)
      : base(master)
    {
      this.happyLocoAnim = happy_loco_anim;
      this.happyLocoWalkAnim = happy_loco_walk_anim;
      this.happyExpression = happy_expression;
      this.expectationAttribute = this.gameObject.GetAttributes().Add(Db.Get().Attributes.QualityOfLifeExpectation);
      this.qolAttribute = Db.Get().Attributes.QualityOfLife.Lookup(this.gameObject);
    }

    public bool ShouldBeOverjoyed()
    {
      float val = this.qolAttribute.GetTotalValue() - this.expectationAttribute.GetTotalValue();
      if ((double) val < (double) TRAITS.JOY_REACTIONS.MIN_MORALE_EXCESS)
        return false;
      float num = MathUtil.ReRange(val, TRAITS.JOY_REACTIONS.MIN_MORALE_EXCESS, TRAITS.JOY_REACTIONS.MAX_MORALE_EXCESS, TRAITS.JOY_REACTIONS.MIN_REACTION_CHANCE, TRAITS.JOY_REACTIONS.MAX_REACTION_CHANCE);
      return (double) UnityEngine.Random.Range(0.0f, 100f) <= (double) num;
    }

    public void GoToOverjoyed()
    {
      this.smi.transitionTime = GameClock.Instance.GetTime() + TRAITS.JOY_REACTIONS.JOY_REACTION_DURATION;
      this.smi.GoTo((StateMachine.BaseState) this.smi.sm.overjoyed);
    }
  }
}
