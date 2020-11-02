// Decompiled with JetBrains decompiler
// Type: FilteredDragTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class FilteredDragTool : DragTool
{
  private Dictionary<string, ToolParameterMenu.ToggleState> filterTargets = new Dictionary<string, ToolParameterMenu.ToggleState>();
  private Dictionary<string, ToolParameterMenu.ToggleState> overlayFilterTargets = new Dictionary<string, ToolParameterMenu.ToggleState>();
  private Dictionary<string, ToolParameterMenu.ToggleState> currentFilterTargets;
  private bool active;

  public bool IsActiveLayer(string layer)
  {
    if (this.currentFilterTargets[ToolParameterMenu.FILTERLAYERS.ALL] == ToolParameterMenu.ToggleState.On)
      return true;
    return this.currentFilterTargets.ContainsKey(layer.ToUpper()) && this.currentFilterTargets[layer.ToUpper()] == ToolParameterMenu.ToggleState.On;
  }

  public bool IsActiveLayer(ObjectLayer layer)
  {
    if (this.currentFilterTargets.ContainsKey(ToolParameterMenu.FILTERLAYERS.ALL) && this.currentFilterTargets[ToolParameterMenu.FILTERLAYERS.ALL] == ToolParameterMenu.ToggleState.On)
      return true;
    bool flag = false;
    foreach (KeyValuePair<string, ToolParameterMenu.ToggleState> currentFilterTarget in this.currentFilterTargets)
    {
      if (currentFilterTarget.Value == ToolParameterMenu.ToggleState.On && this.GetObjectLayerFromFilterLayer(currentFilterTarget.Key) == layer)
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  protected virtual void GetDefaultFilters(
    Dictionary<string, ToolParameterMenu.ToggleState> filters)
  {
    filters.Add(ToolParameterMenu.FILTERLAYERS.ALL, ToolParameterMenu.ToggleState.On);
    filters.Add(ToolParameterMenu.FILTERLAYERS.WIRES, ToolParameterMenu.ToggleState.Off);
    filters.Add(ToolParameterMenu.FILTERLAYERS.LIQUIDCONDUIT, ToolParameterMenu.ToggleState.Off);
    filters.Add(ToolParameterMenu.FILTERLAYERS.GASCONDUIT, ToolParameterMenu.ToggleState.Off);
    filters.Add(ToolParameterMenu.FILTERLAYERS.SOLIDCONDUIT, ToolParameterMenu.ToggleState.Off);
    filters.Add(ToolParameterMenu.FILTERLAYERS.BUILDINGS, ToolParameterMenu.ToggleState.Off);
    filters.Add(ToolParameterMenu.FILTERLAYERS.LOGIC, ToolParameterMenu.ToggleState.Off);
    filters.Add(ToolParameterMenu.FILTERLAYERS.BACKWALL, ToolParameterMenu.ToggleState.Off);
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.ResetFilter(this.filterTargets);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    OverlayScreen.Instance.OnOverlayChanged += new System.Action<HashedString>(this.OnOverlayChanged);
  }

  protected override void OnCleanUp()
  {
    OverlayScreen.Instance.OnOverlayChanged -= new System.Action<HashedString>(this.OnOverlayChanged);
    base.OnCleanUp();
  }

  public void ResetFilter() => this.ResetFilter(this.filterTargets);

  protected void ResetFilter(
    Dictionary<string, ToolParameterMenu.ToggleState> filters)
  {
    filters.Clear();
    this.GetDefaultFilters(filters);
    this.currentFilterTargets = filters;
  }

  protected override void OnActivateTool()
  {
    this.active = true;
    base.OnActivateTool();
    this.OnOverlayChanged(OverlayScreen.Instance.mode);
  }

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    this.active = false;
    ToolMenu.Instance.toolParameterMenu.ClearMenu();
    base.OnDeactivateTool(new_tool);
  }

  public virtual string GetFilterLayerFromGameObject(GameObject input)
  {
    BuildingComplete component1 = input.GetComponent<BuildingComplete>();
    BuildingUnderConstruction component2 = input.GetComponent<BuildingUnderConstruction>();
    if ((bool) (UnityEngine.Object) component1)
      return this.GetFilterLayerFromObjectLayer(component1.Def.ObjectLayer);
    if ((bool) (UnityEngine.Object) component2)
      return this.GetFilterLayerFromObjectLayer(component2.Def.ObjectLayer);
    if ((UnityEngine.Object) input.GetComponent<Clearable>() != (UnityEngine.Object) null || (UnityEngine.Object) input.GetComponent<Moppable>() != (UnityEngine.Object) null)
      return "CleanAndClear";
    return (UnityEngine.Object) input.GetComponent<Diggable>() != (UnityEngine.Object) null ? "DigPlacer" : "Default";
  }

  public string GetFilterLayerFromObjectLayer(ObjectLayer gamer_layer)
  {
    switch (gamer_layer)
    {
      case ObjectLayer.Building:
        return "Buildings";
      case ObjectLayer.Backwall:
        return "BackWall";
      case ObjectLayer.FoundationTile:
        return "Tiles";
      case ObjectLayer.GasConduit:
      case ObjectLayer.GasConduitConnection:
        return "GasPipes";
      case ObjectLayer.LiquidConduit:
      case ObjectLayer.LiquidConduitConnection:
        return "LiquidPipes";
      case ObjectLayer.SolidConduit:
      case ObjectLayer.SolidConduitConnection:
        return "SolidConduits";
      case ObjectLayer.Wire:
      case ObjectLayer.WireConnectors:
        return "Wires";
      case ObjectLayer.LogicGate:
      case ObjectLayer.LogicWire:
        return "Logic";
      default:
        return "Default";
    }
  }

  private ObjectLayer GetObjectLayerFromFilterLayer(string filter_layer)
  {
    switch (filter_layer.ToLower())
    {
      case "backwall":
        return ObjectLayer.Backwall;
      case "buildings":
        return ObjectLayer.Building;
      case "gaspipes":
        return ObjectLayer.GasConduit;
      case "liquidpipes":
        return ObjectLayer.LiquidConduit;
      case "logic":
        return ObjectLayer.LogicWire;
      case "solidconduits":
        return ObjectLayer.SolidConduit;
      case "tiles":
        return ObjectLayer.FoundationTile;
      case "wires":
        return ObjectLayer.Wire;
      default:
        throw new ArgumentException("Invalid filter layer: " + filter_layer);
    }
  }

  private void OnOverlayChanged(HashedString overlay)
  {
    if (!this.active)
      return;
    string str = (string) null;
    if (overlay == OverlayModes.Power.ID)
      str = ToolParameterMenu.FILTERLAYERS.WIRES;
    else if (overlay == OverlayModes.LiquidConduits.ID)
      str = ToolParameterMenu.FILTERLAYERS.LIQUIDCONDUIT;
    else if (overlay == OverlayModes.GasConduits.ID)
      str = ToolParameterMenu.FILTERLAYERS.GASCONDUIT;
    else if (overlay == OverlayModes.SolidConveyor.ID)
      str = ToolParameterMenu.FILTERLAYERS.SOLIDCONDUIT;
    else if (overlay == OverlayModes.Logic.ID)
      str = ToolParameterMenu.FILTERLAYERS.LOGIC;
    this.currentFilterTargets = this.filterTargets;
    if (str != null)
    {
      foreach (string key in new List<string>((IEnumerable<string>) this.filterTargets.Keys))
      {
        this.filterTargets[key] = ToolParameterMenu.ToggleState.Disabled;
        if (key == str)
          this.filterTargets[key] = ToolParameterMenu.ToggleState.On;
      }
    }
    else
    {
      if (this.overlayFilterTargets.Count == 0)
        this.ResetFilter(this.overlayFilterTargets);
      this.currentFilterTargets = this.overlayFilterTargets;
    }
    ToolMenu.Instance.toolParameterMenu.PopulateMenu(this.currentFilterTargets);
  }
}
