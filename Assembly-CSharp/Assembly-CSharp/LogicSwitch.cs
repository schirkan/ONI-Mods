// Decompiled with JetBrains decompiler
// Type: LogicSwitch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class LogicSwitch : Switch, IPlayerControlledToggle, ISim33ms
{
  public static readonly HashedString PORT_ID = (HashedString) nameof (LogicSwitch);
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;
  private static readonly EventSystem.IntraObjectHandler<LogicSwitch> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicSwitch>((System.Action<LogicSwitch, object>) ((component, data) => component.OnCopySettings(data)));
  private bool wasOn;
  private System.Action firstFrameCallback;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<LogicSwitch>(-905833192, LogicSwitch.OnCopySettingsDelegate);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.wasOn = this.switchedOn;
    this.UpdateLogicCircuit();
    this.GetComponent<KBatchedAnimController>().Play((HashedString) (this.switchedOn ? "on" : "off"));
  }

  protected override void OnCleanUp() => base.OnCleanUp();

  private void OnCopySettings(object data)
  {
    LogicSwitch component = ((GameObject) data).GetComponent<LogicSwitch>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || this.switchedOn == component.switchedOn)
      return;
    this.switchedOn = component.switchedOn;
    this.UpdateVisualization();
    this.UpdateLogicCircuit();
  }

  protected override void Toggle()
  {
    base.Toggle();
    this.UpdateVisualization();
    this.UpdateLogicCircuit();
  }

  private void UpdateVisualization()
  {
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    if (this.wasOn != this.switchedOn)
    {
      component.Play((HashedString) (this.switchedOn ? "on_pre" : "on_pst"));
      component.Queue((HashedString) (this.switchedOn ? "on" : "off"));
    }
    this.wasOn = this.switchedOn;
  }

  private void UpdateLogicCircuit() => this.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);

  protected override void UpdateSwitchStatus()
  {
    StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSwitchStatusActive : Db.Get().BuildingStatusItems.LogicSwitchStatusInactive;
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item);
  }

  public void Sim33ms(float dt)
  {
    if (!this.ToggleRequested)
      return;
    this.Toggle();
    this.ToggleRequested = false;
    this.GetSelectable().SetStatusItem(Db.Get().StatusItemCategories.Main, (StatusItem) null);
  }

  public void SetFirstFrameCallback(System.Action ffCb)
  {
    this.firstFrameCallback = ffCb;
    this.StartCoroutine(this.RunCallback());
  }

  private IEnumerator RunCallback()
  {
    yield return (object) null;
    if (this.firstFrameCallback != null)
    {
      this.firstFrameCallback();
      this.firstFrameCallback = (System.Action) null;
    }
    yield return (object) null;
  }

  public void ToggledByPlayer() => this.Toggle();

  public bool ToggledOn() => this.switchedOn;

  public KSelectable GetSelectable() => this.GetComponent<KSelectable>();

  public string SideScreenTitleKey => "STRINGS.BUILDINGS.PREFABS.LOGICSWITCH.SIDESCREEN_TITLE";

  public bool ToggleRequested { get; set; }
}
