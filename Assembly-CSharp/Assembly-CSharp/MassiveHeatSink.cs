﻿// Decompiled with JetBrains decompiler
// Type: MassiveHeatSink
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

public class MassiveHeatSink : StateMachineComponent<MassiveHeatSink.StatesInstance>
{
  [MyCmpReq]
  private Operational operational;
  [MyCmpReq]
  private ElementConverter elementConverter;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  public class States : GameStateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink>
  {
    public GameStateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.State disabled;
    public GameStateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.State idle;
    public GameStateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.State active;

    private string AwaitingFuelResolveString(string str, object obj)
    {
      ElementConverter elementConverter = ((StateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.GenericInstance) obj).master.elementConverter;
      string str1 = elementConverter.consumedElements[0].tag.ProperName();
      string formattedMass = GameUtil.GetFormattedMass(elementConverter.consumedElements[0].massConsumptionRate, GameUtil.TimeSlice.PerSecond);
      str = string.Format(str, (object) str1, (object) formattedMass);
      return str;
    }

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.disabled;
      this.disabled.EventTransition(GameHashes.OperationalChanged, this.idle, (StateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.Transition.ConditionCallback) (smi => smi.master.operational.IsOperational));
      this.idle.EventTransition(GameHashes.OperationalChanged, this.disabled, (StateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.Transition.ConditionCallback) (smi => !smi.master.operational.IsOperational)).ToggleStatusItem((string) BUILDING.STATUSITEMS.AWAITINGFUEL.NAME, (string) BUILDING.STATUSITEMS.AWAITINGFUEL.TOOLTIP, icon_type: StatusItem.IconType.Exclamation, notification_type: NotificationType.BadMinor, resolve_string_callback: new Func<string, MassiveHeatSink.StatesInstance, string>(this.AwaitingFuelResolveString)).EventTransition(GameHashes.OnStorageChange, this.active, (StateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.Transition.ConditionCallback) (smi => smi.master.elementConverter.HasEnoughMassToStartConverting()));
      this.active.EventTransition(GameHashes.OperationalChanged, this.disabled, (StateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.Transition.ConditionCallback) (smi => !smi.master.operational.IsOperational)).EventTransition(GameHashes.OnStorageChange, this.idle, (StateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.Transition.ConditionCallback) (smi => !smi.master.elementConverter.HasEnoughMassToStartConverting())).Enter((StateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.State.Callback) (smi => smi.master.operational.SetActive(true))).Exit((StateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.State.Callback) (smi => smi.master.operational.SetActive(false)));
    }
  }

  public class StatesInstance : GameStateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.GameInstance
  {
    public StatesInstance(MassiveHeatSink master)
      : base(master)
    {
    }
  }
}