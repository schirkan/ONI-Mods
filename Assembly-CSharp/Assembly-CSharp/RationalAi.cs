// Decompiled with JetBrains decompiler
// Type: RationalAi
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

public class RationalAi : GameStateMachine<RationalAi, RationalAi.Instance>
{
  public GameStateMachine<RationalAi, RationalAi.Instance, IStateMachineTarget, object>.State alive;
  public GameStateMachine<RationalAi, RationalAi.Instance, IStateMachineTarget, object>.State dead;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.root.ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new DeathMonitor.Instance(smi.master, new DeathMonitor.Def()))).Enter((StateMachine<RationalAi, RationalAi.Instance, IStateMachineTarget, object>.State.Callback) (smi =>
    {
      if (smi.HasTag(GameTags.Dead))
        smi.GoTo((StateMachine.BaseState) this.dead);
      else
        smi.GoTo((StateMachine.BaseState) this.alive);
    }));
    this.alive.TagTransition(GameTags.Dead, this.dead).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new ThoughtGraph.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new StaminaMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new StressMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new EmoteMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new SneezeMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new DecorMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new IncapacitationMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new IdleMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new RationMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new CalorieMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new DoctorMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new SicknessMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new GermExposureMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new BreathMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new RoomMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new TemperatureMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new ExternalTemperatureMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new BladderMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new SteppedInMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new LightMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new RedAlertMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new CringeMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new HygieneMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new FallMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new ThreatMonitor.Instance(smi.master, new ThreatMonitor.Def()))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new WoundMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new TiredMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new MoveToLocationMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new ReactionMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new SuitWearer.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new TubeTraveller.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new MingleMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new MournMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new SpeechMonitor.Instance(smi.master, new SpeechMonitor.Def()))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new BlinkMonitor.Instance(smi.master, new BlinkMonitor.Def()))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new ConversationMonitor.Instance(smi.master, new ConversationMonitor.Def())));
    this.dead.ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new FallWhenDeadMonitor.Instance(smi.master))).ToggleBrain("dead").Enter("RefreshUserMenu", (StateMachine<RationalAi, RationalAi.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.RefreshUserMenu())).Enter("DropStorage", (StateMachine<RationalAi, RationalAi.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.GetComponent<Storage>().DropAll()));
  }

  public new class Instance : GameStateMachine<RationalAi, RationalAi.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Instance(IStateMachineTarget master)
      : base(master)
    {
      ChoreConsumer component = this.GetComponent<ChoreConsumer>();
      component.AddUrge(Db.Get().Urges.EmoteHighPriority);
      component.AddUrge(Db.Get().Urges.EmoteIdle);
    }

    public void RefreshUserMenu() => Game.Instance.userMenu.Refresh(this.master.gameObject);
  }
}
