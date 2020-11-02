// Decompiled with JetBrains decompiler
// Type: Wire
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Wire")]
public class Wire : KMonoBehaviour, IDisconnectable, IFirstFrameCallback, IWattageRating, IHaveUtilityNetworkMgr, IUtilityNetworkItem, IBridgedNetworkItem
{
  [SerializeField]
  public Wire.WattageRating MaxWattageRating;
  [SerializeField]
  private bool disconnected = true;
  public static readonly KAnimHashedString OutlineSymbol = new KAnimHashedString("outline");
  public float circuitOverloadTime;
  private static readonly EventSystem.IntraObjectHandler<Wire> OnBuildingBrokenDelegate = new EventSystem.IntraObjectHandler<Wire>((System.Action<Wire, object>) ((component, data) => component.OnBuildingBroken(data)));
  private static readonly EventSystem.IntraObjectHandler<Wire> OnBuildingFullyRepairedDelegate = new EventSystem.IntraObjectHandler<Wire>((System.Action<Wire, object>) ((component, data) => component.OnBuildingFullyRepaired(data)));
  private static StatusItem WireCircuitStatus = (StatusItem) null;
  private static StatusItem WireMaxWattageStatus = (StatusItem) null;
  private System.Action firstFrameCallback;

  public static float GetMaxWattageAsFloat(Wire.WattageRating rating)
  {
    switch (rating)
    {
      case Wire.WattageRating.Max500:
        return 500f;
      case Wire.WattageRating.Max1000:
        return 1000f;
      case Wire.WattageRating.Max2000:
        return 2000f;
      case Wire.WattageRating.Max20000:
        return 20000f;
      case Wire.WattageRating.Max50000:
        return 50000f;
      default:
        return 0.0f;
    }
  }

  public bool IsConnected => Game.Instance.electricalConduitSystem.GetNetworkForCell(Grid.PosToCell(this.transform.GetPosition())) is ElectricalUtilityNetwork;

  public ushort NetworkID => !(Game.Instance.electricalConduitSystem.GetNetworkForCell(Grid.PosToCell(this.transform.GetPosition())) is ElectricalUtilityNetwork networkForCell) ? ushort.MaxValue : (ushort) networkForCell.id;

  protected override void OnSpawn()
  {
    Game.Instance.electricalConduitSystem.AddToNetworks(Grid.PosToCell(this.transform.GetPosition()), (object) this, false);
    this.InitializeSwitchState();
    this.Subscribe<Wire>(774203113, Wire.OnBuildingBrokenDelegate);
    this.Subscribe<Wire>(-1735440190, Wire.OnBuildingFullyRepairedDelegate);
    this.GetComponent<KSelectable>().AddStatusItem(Wire.WireCircuitStatus, (object) this);
    this.GetComponent<KSelectable>().AddStatusItem(Wire.WireMaxWattageStatus, (object) this);
    this.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(Wire.OutlineSymbol, false);
  }

  protected override void OnCleanUp()
  {
    int cell = Grid.PosToCell(this.transform.GetPosition());
    BuildingComplete component = this.GetComponent<BuildingComplete>();
    if (component.Def.ReplacementLayer == ObjectLayer.NumLayers || (UnityEngine.Object) Grid.Objects[cell, (int) component.Def.ReplacementLayer] == (UnityEngine.Object) null)
      Game.Instance.electricalConduitSystem.RemoveFromNetworks(cell, (object) this, false);
    this.Unsubscribe<Wire>(774203113, Wire.OnBuildingBrokenDelegate);
    this.Unsubscribe<Wire>(-1735440190, Wire.OnBuildingFullyRepairedDelegate);
    base.OnCleanUp();
  }

  private void InitializeSwitchState()
  {
    int cell = Grid.PosToCell(this.transform.GetPosition());
    bool flag = false;
    GameObject gameObject = Grid.Objects[cell, 1];
    if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
    {
      CircuitSwitch component = gameObject.GetComponent<CircuitSwitch>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        flag = true;
        component.AttachWire(this);
      }
    }
    if (flag)
      return;
    this.Connect();
  }

  public UtilityConnections GetWireConnections() => Game.Instance.electricalConduitSystem.GetConnections(Grid.PosToCell(this.transform.GetPosition()), true);

  public string GetWireConnectionsString() => Game.Instance.electricalConduitSystem.GetVisualizerString(this.GetWireConnections());

  private void OnBuildingBroken(object data) => this.Disconnect();

  private void OnBuildingFullyRepaired(object data) => this.InitializeSwitchState();

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.GetComponent<KPrefabID>().AddTag(GameTags.Wires);
    if (Wire.WireCircuitStatus == null)
      Wire.WireCircuitStatus = new StatusItem("WireCircuitStatus", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID).SetResolveStringCallback((Func<string, object, string>) ((str, data) =>
      {
        Wire wire = (Wire) data;
        int cell = Grid.PosToCell(wire.transform.GetPosition());
        CircuitManager circuitManager = Game.Instance.circuitManager;
        ushort circuitId = circuitManager.GetCircuitID(cell);
        float wattsUsedByCircuit = circuitManager.GetWattsUsedByCircuit(circuitId);
        GameUtil.WattageFormatterUnit unit = GameUtil.WattageFormatterUnit.Watts;
        if (wire.MaxWattageRating >= Wire.WattageRating.Max20000)
          unit = GameUtil.WattageFormatterUnit.Kilowatts;
        float maxWattageAsFloat = Wire.GetMaxWattageAsFloat(wire.MaxWattageRating);
        float neededWhenActive = circuitManager.GetWattsNeededWhenActive(circuitId);
        string wireLoadColor = GameUtil.GetWireLoadColor(wattsUsedByCircuit, maxWattageAsFloat, neededWhenActive);
        string str1 = str;
        string newValue;
        if (!(wireLoadColor == Color.white.ToHexString()))
          newValue = "<color=#" + wireLoadColor + ">" + GameUtil.GetFormattedWattage(wattsUsedByCircuit, unit) + "</color>";
        else
          newValue = GameUtil.GetFormattedWattage(wattsUsedByCircuit, unit);
        str = str1.Replace("{CurrentLoadAndColor}", newValue);
        str = str.Replace("{MaxLoad}", GameUtil.GetFormattedWattage(maxWattageAsFloat, unit));
        str = str.Replace("{WireType}", this.GetProperName());
        return str;
      }));
    if (Wire.WireMaxWattageStatus != null)
      return;
    Wire.WireMaxWattageStatus = new StatusItem("WireMaxWattageStatus", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID).SetResolveStringCallback((Func<string, object, string>) ((str, data) =>
    {
      Wire wire = (Wire) data;
      GameUtil.WattageFormatterUnit unit = GameUtil.WattageFormatterUnit.Watts;
      if (wire.MaxWattageRating >= Wire.WattageRating.Max20000)
        unit = GameUtil.WattageFormatterUnit.Kilowatts;
      int cell = Grid.PosToCell(wire.transform.GetPosition());
      CircuitManager circuitManager = Game.Instance.circuitManager;
      float neededWhenActive = circuitManager.GetWattsNeededWhenActive(circuitManager.GetCircuitID(cell));
      float maxWattageAsFloat = Wire.GetMaxWattageAsFloat(wire.MaxWattageRating);
      string str1 = str;
      string newValue;
      if ((double) neededWhenActive <= (double) maxWattageAsFloat)
        newValue = GameUtil.GetFormattedWattage(neededWhenActive, unit);
      else
        newValue = "<color=#" + new Color(0.9843137f, 0.6901961f, 0.2313726f).ToHexString() + ">" + GameUtil.GetFormattedWattage(neededWhenActive, unit) + "</color>";
      str = str1.Replace("{TotalPotentialLoadAndColor}", newValue);
      str = str.Replace("{MaxLoad}", GameUtil.GetFormattedWattage(maxWattageAsFloat, unit));
      return str;
    }));
  }

  public Wire.WattageRating GetMaxWattageRating() => this.MaxWattageRating;

  public bool IsDisconnected() => this.disconnected;

  public bool Connect()
  {
    BuildingHP component = this.GetComponent<BuildingHP>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null || component.HitPoints > 0)
    {
      this.disconnected = false;
      Game.Instance.electricalConduitSystem.ForceRebuildNetworks();
    }
    return !this.disconnected;
  }

  public void Disconnect()
  {
    this.disconnected = true;
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.WireDisconnected);
    Game.Instance.electricalConduitSystem.ForceRebuildNetworks();
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

  public IUtilityNetworkMgr GetNetworkManager() => (IUtilityNetworkMgr) Game.Instance.electricalConduitSystem;

  public void AddNetworks(ICollection<UtilityNetwork> networks)
  {
    UtilityNetwork networkForCell = Game.Instance.electricalConduitSystem.GetNetworkForCell(Grid.PosToCell(this.transform.GetPosition()));
    if (networkForCell == null)
      return;
    networks.Add(networkForCell);
  }

  public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
  {
    UtilityNetwork networkForCell = Game.Instance.electricalConduitSystem.GetNetworkForCell(Grid.PosToCell(this.transform.GetPosition()));
    return networks.Contains(networkForCell);
  }

  public int GetNetworkCell() => Grid.PosToCell((KMonoBehaviour) this);

  public enum WattageRating
  {
    Max500,
    Max1000,
    Max2000,
    Max20000,
    Max50000,
    NumRatings,
  }
}
