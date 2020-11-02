﻿// Decompiled with JetBrains decompiler
// Type: Turbine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using KSerialization;
using System;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/Turbine")]
public class Turbine : KMonoBehaviour
{
  public SimHashes srcElem;
  public float requiredMassFlowDifferential = 3f;
  public float activePercent = 0.75f;
  public float minEmitMass;
  public float minActiveTemperature = 400f;
  public float emitTemperature = 300f;
  public float maxRPM;
  public float rpmAcceleration;
  public float rpmDeceleration;
  public float minGenerationRPM;
  public float pumpKGRate;
  private static readonly HashedString TINT_SYMBOL = new HashedString("meter_fill");
  [Serialize]
  private float storedMass;
  [Serialize]
  private float storedTemperature;
  [Serialize]
  private byte diseaseIdx = byte.MaxValue;
  [Serialize]
  private int diseaseCount;
  [MyCmpGet]
  private Generator generator;
  [Serialize]
  private float currentRPM;
  private int[] srcCells;
  private int[] destCells;
  private Turbine.Instance smi;
  private static StatusItem inputBlockedStatusItem;
  private static StatusItem outputBlockedStatusItem;
  private static StatusItem insufficientMassStatusItem;
  private static StatusItem insufficientTemperatureStatusItem;
  private static StatusItem activeStatusItem;
  private static StatusItem spinningUpStatusItem;
  private const Sim.Cell.Properties floorCellProperties = Sim.Cell.Properties.GasImpermeable | Sim.Cell.Properties.LiquidImpermeable | Sim.Cell.Properties.SolidImpermeable | Sim.Cell.Properties.Opaque;
  private MeterController meter;
  private HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.Handle simEmitCBHandle = HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.InvalidHandle;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.simEmitCBHandle = Game.Instance.massEmitCallbackManager.Add(new System.Action<Sim.MassEmittedCallback, object>(Turbine.OnSimEmittedCallback), (object) this, "TurbineEmit");
    BuildingDef def = this.GetComponent<BuildingComplete>().Def;
    this.srcCells = new int[def.WidthInCells];
    this.destCells = new int[def.WidthInCells];
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    for (int index = 0; index < def.WidthInCells; ++index)
    {
      int x = index - (def.WidthInCells - 1) / 2;
      this.srcCells[index] = Grid.OffsetCell(cell, new CellOffset(x, -1));
      this.destCells[index] = Grid.OffsetCell(cell, new CellOffset(x, def.HeightInCells - 1));
      int num = Grid.OffsetCell(cell, new CellOffset(x, 0));
      SimMessages.SetCellProperties(num, (byte) 39);
      Grid.Foundation[num] = true;
      Grid.SetSolid(num, true, CellEventLogger.Instance.SimCellOccupierForceSolid);
      Grid.RenderedByWorld[num] = false;
      World.Instance.OnSolidChanged(num);
      GameScenePartitioner.Instance.TriggerEvent(num, GameScenePartitioner.Instance.solidChangedLayer, (object) null);
    }
    this.smi = new Turbine.Instance(this);
    this.smi.StartSM();
    this.CreateMeter();
  }

  private void CreateMeter()
  {
    this.meter = new MeterController((KAnimControllerBase) this.gameObject.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[3]
    {
      "meter_OL",
      "meter_frame",
      "meter_fill"
    });
    this.smi.UpdateMeter();
  }

  protected override void OnCleanUp()
  {
    if (this.smi != null)
      this.smi.StopSM("cleanup");
    BuildingDef def = this.GetComponent<BuildingComplete>().Def;
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    for (int index = 0; index < def.WidthInCells; ++index)
    {
      int x = index - (def.WidthInCells - 1) / 2;
      int num = Grid.OffsetCell(cell, new CellOffset(x, 0));
      SimMessages.ClearCellProperties(num, (byte) 39);
      Grid.Foundation[num] = false;
      Grid.SetSolid(num, false, CellEventLogger.Instance.SimCellOccupierForceSolid);
      Grid.RenderedByWorld[num] = true;
      World.Instance.OnSolidChanged(num);
      GameScenePartitioner.Instance.TriggerEvent(num, GameScenePartitioner.Instance.solidChangedLayer, (object) null);
    }
    Game.Instance.massEmitCallbackManager.Release(this.simEmitCBHandle, nameof (Turbine));
    this.simEmitCBHandle.Clear();
    base.OnCleanUp();
  }

  private void Pump(float dt)
  {
    float num1 = this.pumpKGRate * dt / (float) this.srcCells.Length;
    foreach (int srcCell in this.srcCells)
    {
      HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = Game.Instance.massConsumedCallbackManager.Add(new System.Action<Sim.MassConsumedCallback, object>(Turbine.OnSimConsumeCallback), (object) this, "TurbineConsume");
      int srcElem = (int) this.srcElem;
      double num2 = (double) num1;
      int index = handle.index;
      SimMessages.ConsumeMass(srcCell, (SimHashes) srcElem, (float) num2, (byte) 1, index);
    }
  }

  private static void OnSimConsumeCallback(Sim.MassConsumedCallback mass_cb_info, object data) => ((Turbine) data).OnSimConsume(mass_cb_info);

  private void OnSimConsume(Sim.MassConsumedCallback mass_cb_info)
  {
    if ((double) mass_cb_info.mass <= 0.0)
      return;
    this.storedTemperature = SimUtil.CalculateFinalTemperature(this.storedMass, this.storedTemperature, mass_cb_info.mass, mass_cb_info.temperature);
    this.storedMass += mass_cb_info.mass;
    SimUtil.DiseaseInfo finalDiseaseInfo = SimUtil.CalculateFinalDiseaseInfo(this.diseaseIdx, this.diseaseCount, mass_cb_info.diseaseIdx, mass_cb_info.diseaseCount);
    this.diseaseIdx = finalDiseaseInfo.idx;
    this.diseaseCount = finalDiseaseInfo.count;
    if ((double) this.storedMass <= (double) this.minEmitMass || !this.simEmitCBHandle.IsValid())
      return;
    float mass = this.storedMass / (float) this.destCells.Length;
    int disease_count = this.diseaseCount / this.destCells.Length;
    Game.Instance.massEmitCallbackManager.GetItem(this.simEmitCBHandle);
    foreach (int destCell in this.destCells)
      SimMessages.EmitMass(destCell, mass_cb_info.elemIdx, mass, this.emitTemperature, this.diseaseIdx, disease_count, this.simEmitCBHandle.index);
    this.storedMass = 0.0f;
    this.storedTemperature = 0.0f;
    this.diseaseIdx = byte.MaxValue;
    this.diseaseCount = 0;
  }

  private static void OnSimEmittedCallback(Sim.MassEmittedCallback info, object data) => ((Turbine) data).OnSimEmitted(info);

  private void OnSimEmitted(Sim.MassEmittedCallback info)
  {
    if (info.suceeded == (byte) 1)
      return;
    this.storedTemperature = SimUtil.CalculateFinalTemperature(this.storedMass, this.storedTemperature, info.mass, info.temperature);
    this.storedMass += info.mass;
    if (info.diseaseIdx == byte.MaxValue)
      return;
    SimUtil.DiseaseInfo diseaseInfo = new SimUtil.DiseaseInfo();
    diseaseInfo.idx = this.diseaseIdx;
    diseaseInfo.count = this.diseaseCount;
    SimUtil.DiseaseInfo a = diseaseInfo;
    diseaseInfo = new SimUtil.DiseaseInfo();
    diseaseInfo.idx = info.diseaseIdx;
    diseaseInfo.count = info.diseaseCount;
    SimUtil.DiseaseInfo b = diseaseInfo;
    SimUtil.DiseaseInfo finalDiseaseInfo = SimUtil.CalculateFinalDiseaseInfo(a, b);
    this.diseaseIdx = finalDiseaseInfo.idx;
    this.diseaseCount = finalDiseaseInfo.count;
  }

  public static void InitializeStatusItems()
  {
    Turbine.inputBlockedStatusItem = new StatusItem("TURBINE_BLOCKED_INPUT", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
    Turbine.outputBlockedStatusItem = new StatusItem("TURBINE_BLOCKED_OUTPUT", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
    Turbine.spinningUpStatusItem = new StatusItem("TURBINE_SPINNING_UP", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID);
    Turbine.activeStatusItem = new StatusItem("TURBINE_ACTIVE", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID);
    Turbine.activeStatusItem.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      Turbine turbine = (Turbine) data;
      str = string.Format(str, (object) (int) turbine.currentRPM);
      return str;
    });
    Turbine.insufficientMassStatusItem = new StatusItem("TURBINE_INSUFFICIENT_MASS", "BUILDING", "status_item_resource_unavailable", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Power.ID);
    Turbine.insufficientMassStatusItem.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
    {
      Turbine turbine = (Turbine) data;
      str = str.Replace("{MASS}", GameUtil.GetFormattedMass(turbine.requiredMassFlowDifferential));
      str = str.Replace("{SRC_ELEMENT}", ElementLoader.FindElementByHash(turbine.srcElem).name);
      return str;
    });
    Turbine.insufficientTemperatureStatusItem = new StatusItem("TURBINE_INSUFFICIENT_TEMPERATURE", "BUILDING", "status_item_plant_temperature", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Power.ID);
    Turbine.insufficientTemperatureStatusItem.resolveStringCallback = new Func<string, object, string>(Turbine.ResolveStrings);
    Turbine.insufficientTemperatureStatusItem.resolveTooltipCallback = new Func<string, object, string>(Turbine.ResolveStrings);
  }

  private static string ResolveStrings(string str, object data)
  {
    Turbine turbine = (Turbine) data;
    str = str.Replace("{SRC_ELEMENT}", ElementLoader.FindElementByHash(turbine.srcElem).name);
    str = str.Replace("{ACTIVE_TEMPERATURE}", GameUtil.GetFormattedTemperature(turbine.minActiveTemperature));
    return str;
  }

  public class States : GameStateMachine<Turbine.States, Turbine.Instance, Turbine>
  {
    public GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State inoperational;
    public Turbine.States.OperationalStates operational;
    private static readonly HashedString[] ACTIVE_ANIMS = new HashedString[2]
    {
      (HashedString) "working_pre",
      (HashedString) "working_loop"
    };

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      Turbine.InitializeStatusItems();
      default_state = (StateMachine.BaseState) this.operational;
      this.serializable = true;
      this.inoperational.EventTransition(GameHashes.OperationalChanged, this.operational.spinningUp, (StateMachine<Turbine.States, Turbine.Instance, Turbine, object>.Transition.ConditionCallback) (smi => smi.master.GetComponent<Operational>().IsOperational)).QueueAnim("off").Enter((StateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State.Callback) (smi =>
      {
        smi.master.currentRPM = 0.0f;
        smi.UpdateMeter();
      }));
      this.operational.DefaultState(this.operational.spinningUp).EventTransition(GameHashes.OperationalChanged, this.inoperational, (StateMachine<Turbine.States, Turbine.Instance, Turbine, object>.Transition.ConditionCallback) (smi => !smi.master.GetComponent<Operational>().IsOperational)).Update("UpdateOperational", (System.Action<Turbine.Instance, float>) ((smi, dt) => smi.UpdateState(dt))).Exit((StateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State.Callback) (smi => smi.DisableStatusItems()));
      this.operational.idle.QueueAnim("on");
      this.operational.spinningUp.ToggleStatusItem((Func<Turbine.Instance, StatusItem>) (smi => Turbine.spinningUpStatusItem), (Func<Turbine.Instance, object>) (smi => (object) smi.master)).QueueAnim("buildup", true);
      this.operational.active.Update("UpdateActive", (System.Action<Turbine.Instance, float>) ((smi, dt) => smi.master.Pump(dt))).ToggleStatusItem((Func<Turbine.Instance, StatusItem>) (smi => Turbine.activeStatusItem), (Func<Turbine.Instance, object>) (smi => (object) smi.master)).Enter((StateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State.Callback) (smi =>
      {
        smi.GetComponent<KAnimControllerBase>().Play(Turbine.States.ACTIVE_ANIMS, KAnim.PlayMode.Loop);
        smi.GetComponent<Operational>().SetActive(true);
      })).Exit((StateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State.Callback) (smi =>
      {
        smi.master.GetComponent<Generator>().ResetJoules();
        smi.GetComponent<Operational>().SetActive(false);
      }));
    }

    public class OperationalStates : GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State
    {
      public GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State idle;
      public GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State spinningUp;
      public GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State active;
    }
  }

  public class Instance : GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.GameInstance
  {
    public bool isInputBlocked;
    public bool isOutputBlocked;
    public bool insufficientMass;
    public bool insufficientTemperature;
    private Guid inputBlockedHandle = Guid.Empty;
    private Guid outputBlockedHandle = Guid.Empty;
    private Guid insufficientMassHandle = Guid.Empty;
    private Guid insufficientTemperatureHandle = Guid.Empty;

    public Instance(Turbine master)
      : base(master)
    {
    }

    public void UpdateState(float dt)
    {
      float num = this.CanSteamFlow(ref this.insufficientMass, ref this.insufficientTemperature) ? this.master.rpmAcceleration : -this.master.rpmDeceleration;
      this.master.currentRPM = Mathf.Clamp(this.master.currentRPM + dt * num, 0.0f, this.master.maxRPM);
      this.UpdateMeter();
      this.UpdateStatusItems();
      StateMachine.BaseState currentState = this.smi.GetCurrentState();
      if ((double) this.master.currentRPM >= (double) this.master.minGenerationRPM)
      {
        if (currentState != this.sm.operational.active)
          this.smi.GoTo((StateMachine.BaseState) this.sm.operational.active);
        this.smi.master.generator.GenerateJoules(this.smi.master.generator.WattageRating * dt);
      }
      else if ((double) this.master.currentRPM > 0.0)
      {
        if (currentState == this.sm.operational.spinningUp)
          return;
        this.smi.GoTo((StateMachine.BaseState) this.sm.operational.spinningUp);
      }
      else
      {
        if (currentState == this.sm.operational.idle)
          return;
        this.smi.GoTo((StateMachine.BaseState) this.sm.operational.idle);
      }
    }

    public void UpdateMeter()
    {
      if (this.master.meter == null)
        return;
      float percent_full = Mathf.Clamp01(this.master.currentRPM / this.master.maxRPM);
      this.master.meter.SetPositionPercent(percent_full);
      this.master.meter.SetSymbolTint((KAnimHashedString) Turbine.TINT_SYMBOL, (Color32) ((double) percent_full >= (double) this.master.activePercent ? Color.green : Color.red));
    }

    private bool CanSteamFlow(ref bool insufficient_mass, ref bool insufficient_temperature)
    {
      float a1 = 0.0f;
      float a2 = 0.0f;
      float a3 = float.PositiveInfinity;
      this.isInputBlocked = false;
      for (int index = 0; index < this.master.srcCells.Length; ++index)
      {
        int srcCell = this.master.srcCells[index];
        float b1 = Grid.Mass[srcCell];
        if (Grid.Element[srcCell].id == this.master.srcElem)
          a1 = Mathf.Max(a1, b1);
        float b2 = Grid.Temperature[srcCell];
        a2 = Mathf.Max(a2, b2);
        byte num = Grid.ElementIdx[srcCell];
        Element element = ElementLoader.elements[(int) num];
        if (element.IsLiquid || element.IsSolid)
          this.isInputBlocked = true;
      }
      this.isOutputBlocked = false;
      for (int index = 0; index < this.master.destCells.Length; ++index)
      {
        int destCell = this.master.destCells[index];
        float b = Grid.Mass[destCell];
        a3 = Mathf.Min(a3, b);
        byte num = Grid.ElementIdx[destCell];
        Element element = ElementLoader.elements[(int) num];
        if (element.IsLiquid || element.IsSolid)
          this.isOutputBlocked = true;
      }
      insufficient_mass = (double) a1 - (double) a3 < (double) this.master.requiredMassFlowDifferential;
      insufficient_temperature = (double) a2 < (double) this.master.minActiveTemperature;
      return !insufficient_mass && !insufficient_temperature;
    }

    public void UpdateStatusItems()
    {
      KSelectable component = this.GetComponent<KSelectable>();
      this.inputBlockedHandle = this.UpdateStatusItem(Turbine.inputBlockedStatusItem, this.isInputBlocked, this.inputBlockedHandle, component);
      this.outputBlockedHandle = this.UpdateStatusItem(Turbine.outputBlockedStatusItem, this.isOutputBlocked, this.outputBlockedHandle, component);
      this.insufficientMassHandle = this.UpdateStatusItem(Turbine.insufficientMassStatusItem, this.insufficientMass, this.insufficientMassHandle, component);
      this.insufficientTemperatureHandle = this.UpdateStatusItem(Turbine.insufficientTemperatureStatusItem, this.insufficientTemperature, this.insufficientTemperatureHandle, component);
    }

    private Guid UpdateStatusItem(
      StatusItem item,
      bool show,
      Guid current_handle,
      KSelectable ksel)
    {
      Guid guid = current_handle;
      if (show != (current_handle != Guid.Empty))
        guid = !show ? ksel.RemoveStatusItem(current_handle) : ksel.AddStatusItem(item, (object) this.master);
      return guid;
    }

    public void DisableStatusItems()
    {
      KSelectable component = this.GetComponent<KSelectable>();
      component.RemoveStatusItem(this.inputBlockedHandle);
      component.RemoveStatusItem(this.outputBlockedHandle);
      component.RemoveStatusItem(this.insufficientMassHandle);
      component.RemoveStatusItem(this.insufficientTemperatureHandle);
    }
  }
}