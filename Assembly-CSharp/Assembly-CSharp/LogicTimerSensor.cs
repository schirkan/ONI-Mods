﻿// Decompiled with JetBrains decompiler
// Type: LogicTimerSensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class LogicTimerSensor : Switch, ISaveLoadable, ISim33ms
{
  [Serialize]
  public float onDuration = 10f;
  [Serialize]
  public float offDuration = 10f;
  [Serialize]
  public bool displayCyclesMode;
  private bool wasOn;
  [SerializeField]
  [Serialize]
  public float timeElapsedInCurrentState;
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;
  private static readonly EventSystem.IntraObjectHandler<LogicTimerSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicTimerSensor>((System.Action<LogicTimerSensor, object>) ((component, data) => component.OnCopySettings(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<LogicTimerSensor>(-905833192, LogicTimerSensor.OnCopySettingsDelegate);
  }

  private void OnCopySettings(object data)
  {
    LogicTimerSensor component = ((GameObject) data).GetComponent<LogicTimerSensor>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.onDuration = component.onDuration;
    this.offDuration = component.offDuration;
    this.timeElapsedInCurrentState = component.timeElapsedInCurrentState;
    this.displayCyclesMode = component.displayCyclesMode;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.OnToggle += new System.Action<bool>(this.OnSwitchToggled);
    this.UpdateLogicCircuit();
    this.UpdateVisualState(true);
    this.wasOn = this.switchedOn;
  }

  public void Sim33ms(float dt)
  {
    if ((double) this.onDuration == 0.0 && (double) this.offDuration == 0.0)
      return;
    this.timeElapsedInCurrentState += dt;
    bool on = this.IsSwitchedOn;
    if (on)
    {
      if ((double) this.timeElapsedInCurrentState >= (double) this.onDuration)
      {
        on = false;
        this.timeElapsedInCurrentState -= this.onDuration;
      }
    }
    else if ((double) this.timeElapsedInCurrentState >= (double) this.offDuration)
    {
      on = true;
      this.timeElapsedInCurrentState -= this.offDuration;
    }
    this.SetState(on);
  }

  private void OnSwitchToggled(bool toggled_on)
  {
    this.UpdateLogicCircuit();
    this.UpdateVisualState();
  }

  private void UpdateLogicCircuit() => this.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);

  private void UpdateVisualState(bool force = false)
  {
    if (!(this.wasOn != this.switchedOn | force))
      return;
    this.wasOn = this.switchedOn;
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    component.Play((HashedString) (this.switchedOn ? "on_pre" : "on_pst"));
    component.Queue((HashedString) (this.switchedOn ? "on" : "off"));
  }

  protected override void UpdateSwitchStatus()
  {
    StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item);
  }

  public void ResetTimer()
  {
    this.SetState(true);
    this.OnSwitchToggled(true);
    this.timeElapsedInCurrentState = 0.0f;
  }
}