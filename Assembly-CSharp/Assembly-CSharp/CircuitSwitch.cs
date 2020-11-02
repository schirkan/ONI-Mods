// Decompiled with JetBrains decompiler
// Type: CircuitSwitch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class CircuitSwitch : Switch, IPlayerControlledToggle, ISim33ms
{
  [SerializeField]
  public ObjectLayer objectLayer;
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;
  private static readonly EventSystem.IntraObjectHandler<CircuitSwitch> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<CircuitSwitch>((System.Action<CircuitSwitch, object>) ((component, data) => component.OnCopySettings(data)));
  private Wire attachedWire;
  private Guid wireConnectedGUID;
  private bool wasOn;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<CircuitSwitch>(-905833192, CircuitSwitch.OnCopySettingsDelegate);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.OnToggle += new System.Action<bool>(this.CircuitOnToggle);
    int cell = Grid.PosToCell(this.transform.GetPosition());
    GameObject gameObject = Grid.Objects[cell, (int) this.objectLayer];
    Wire wire = (UnityEngine.Object) gameObject != (UnityEngine.Object) null ? gameObject.GetComponent<Wire>() : (Wire) null;
    if ((UnityEngine.Object) wire == (UnityEngine.Object) null)
      this.wireConnectedGUID = this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoWireConnected);
    this.AttachWire(wire);
    this.wasOn = this.switchedOn;
    this.UpdateCircuit();
    this.GetComponent<KBatchedAnimController>().Play((HashedString) (this.switchedOn ? "on" : "off"));
  }

  protected override void OnCleanUp()
  {
    if ((UnityEngine.Object) this.attachedWire != (UnityEngine.Object) null)
      this.UnsubscribeFromWire(this.attachedWire);
    bool switchedOn = this.switchedOn;
    this.switchedOn = true;
    this.UpdateCircuit(false);
    this.switchedOn = switchedOn;
  }

  private void OnCopySettings(object data)
  {
    CircuitSwitch component = ((GameObject) data).GetComponent<CircuitSwitch>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.switchedOn = component.switchedOn;
    this.UpdateCircuit();
  }

  public bool IsConnected()
  {
    int cell = Grid.PosToCell(this.transform.GetPosition());
    GameObject gameObject = Grid.Objects[cell, (int) this.objectLayer];
    return (UnityEngine.Object) gameObject != (UnityEngine.Object) null && gameObject.GetComponent<IDisconnectable>() != null;
  }

  private void CircuitOnToggle(bool on) => this.UpdateCircuit();

  public void AttachWire(Wire wire)
  {
    if ((UnityEngine.Object) wire == (UnityEngine.Object) this.attachedWire)
      return;
    if ((UnityEngine.Object) this.attachedWire != (UnityEngine.Object) null)
      this.UnsubscribeFromWire(this.attachedWire);
    this.attachedWire = wire;
    if ((UnityEngine.Object) this.attachedWire != (UnityEngine.Object) null)
    {
      this.SubscribeToWire(this.attachedWire);
      this.UpdateCircuit();
      this.wireConnectedGUID = this.GetComponent<KSelectable>().RemoveStatusItem(this.wireConnectedGUID);
    }
    else
    {
      if (!(this.wireConnectedGUID == Guid.Empty))
        return;
      this.wireConnectedGUID = this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoWireConnected);
    }
  }

  private void OnWireDestroyed(object data)
  {
    if (!((UnityEngine.Object) this.attachedWire != (UnityEngine.Object) null))
      return;
    this.attachedWire.Unsubscribe(1969584890, new System.Action<object>(this.OnWireDestroyed));
  }

  private void OnWireStateChanged(object data) => this.UpdateCircuit();

  private void SubscribeToWire(Wire wire)
  {
    wire.Subscribe(1969584890, new System.Action<object>(this.OnWireDestroyed));
    wire.Subscribe(-1735440190, new System.Action<object>(this.OnWireStateChanged));
    wire.Subscribe(774203113, new System.Action<object>(this.OnWireStateChanged));
  }

  private void UnsubscribeFromWire(Wire wire)
  {
    wire.Unsubscribe(1969584890, new System.Action<object>(this.OnWireDestroyed));
    wire.Unsubscribe(-1735440190, new System.Action<object>(this.OnWireStateChanged));
    wire.Unsubscribe(774203113, new System.Action<object>(this.OnWireStateChanged));
  }

  private void UpdateCircuit(bool should_update_anim = true)
  {
    if ((UnityEngine.Object) this.attachedWire != (UnityEngine.Object) null)
    {
      if (this.switchedOn)
        this.attachedWire.Connect();
      else
        this.attachedWire.Disconnect();
    }
    if (should_update_anim && this.wasOn != this.switchedOn)
    {
      KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
      component.Play((HashedString) (this.switchedOn ? "on_pre" : "on_pst"));
      component.Queue((HashedString) (this.switchedOn ? "on" : "off"));
      Game.Instance.userMenu.Refresh(this.gameObject);
    }
    this.wasOn = this.switchedOn;
  }

  public void Sim33ms(float dt)
  {
    if (!this.ToggleRequested)
      return;
    this.Toggle();
    this.ToggleRequested = false;
    this.GetSelectable().SetStatusItem(Db.Get().StatusItemCategories.Main, (StatusItem) null);
  }

  public void ToggledByPlayer() => this.Toggle();

  public bool ToggledOn() => this.switchedOn;

  public KSelectable GetSelectable() => this.GetComponent<KSelectable>();

  public string SideScreenTitleKey => "STRINGS.BUILDINGS.PREFABS.SWITCH.SIDESCREEN_TITLE";

  public bool ToggleRequested { get; set; }
}
