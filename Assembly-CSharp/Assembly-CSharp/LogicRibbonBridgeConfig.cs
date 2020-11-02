﻿// Decompiled with JetBrains decompiler
// Type: LogicRibbonBridgeConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class LogicRibbonBridgeConfig : IBuildingConfig
{
  public const string ID = "LogicRibbonBridge";
  public static readonly HashedString BRIDGE_LOGIC_RIBBON_IO_ID = new HashedString("BRIDGE_LOGIC_RIBBON_IO");

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR0_1 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    EffectorValues none = TUNING.NOISE_POLLUTION.NONE;
    EffectorValues tieR0_2 = TUNING.BUILDINGS.DECOR.PENALTY.TIER0;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("LogicRibbonBridge", 3, 1, "logic_ribbon_bridge_kanim", 30, 3f, tieR0_1, refinedMetals, 1600f, BuildLocationRule.LogicBridge, tieR0_2, noise);
    buildingDef.ViewMode = OverlayModes.Logic.ID;
    buildingDef.ObjectLayer = ObjectLayer.LogicGate;
    buildingDef.SceneLayer = Grid.SceneLayer.LogicGates;
    buildingDef.Overheatable = false;
    buildingDef.Floodable = false;
    buildingDef.Entombable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "small";
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.PermittedRotations = PermittedRotations.R360;
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 2);
    buildingDef.AlwaysOperational = true;
    buildingDef.LogicInputPorts = new List<LogicPorts.Port>()
    {
      LogicPorts.Port.RibbonInputPort(LogicRibbonBridgeConfig.BRIDGE_LOGIC_RIBBON_IO_ID, new CellOffset(-1, 0), (string) STRINGS.BUILDINGS.PREFABS.LOGICRIBBONBRIDGE.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.LOGICRIBBONBRIDGE.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.LOGICRIBBONBRIDGE.LOGIC_PORT_INACTIVE),
      LogicPorts.Port.RibbonInputPort(LogicRibbonBridgeConfig.BRIDGE_LOGIC_RIBBON_IO_ID, new CellOffset(1, 0), (string) STRINGS.BUILDINGS.PREFABS.LOGICRIBBONBRIDGE.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.LOGICRIBBONBRIDGE.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.LOGICRIBBONBRIDGE.LOGIC_PORT_INACTIVE)
    };
    GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, "LogicRibbonBridge");
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) => BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    base.DoPostConfigurePreview(def, go);
    this.AddNetworkLink(go).visualizeOnly = true;
    go.AddOrGet<BuildingCellVisualizer>();
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    base.DoPostConfigureUnderConstruction(go);
    this.AddNetworkLink(go).visualizeOnly = true;
    go.AddOrGet<BuildingCellVisualizer>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    this.AddNetworkLink(go).visualizeOnly = false;
    go.AddOrGet<BuildingCellVisualizer>();
    go.AddOrGet<LogicRibbonBridge>();
  }

  private LogicUtilityNetworkLink AddNetworkLink(GameObject go)
  {
    LogicUtilityNetworkLink utilityNetworkLink = go.AddOrGet<LogicUtilityNetworkLink>();
    utilityNetworkLink.bitDepth = LogicWire.BitDepth.FourBit;
    utilityNetworkLink.link1 = new CellOffset(-1, 0);
    utilityNetworkLink.link2 = new CellOffset(1, 0);
    return utilityNetworkLink;
  }
}