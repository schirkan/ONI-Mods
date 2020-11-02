﻿// Decompiled with JetBrains decompiler
// Type: BaseLogicWireConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

public abstract class BaseLogicWireConfig : IBuildingConfig
{
  public abstract override BuildingDef CreateBuildingDef();

  public BuildingDef CreateBuildingDef(
    string id,
    string anim,
    float construction_time,
    float[] construction_mass,
    EffectorValues decor,
    EffectorValues noise)
  {
    string id1 = id;
    string anim1 = anim;
    double num = (double) construction_time;
    float[] construction_mass1 = construction_mass;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    EffectorValues effectorValues = noise;
    EffectorValues decor1 = decor;
    EffectorValues noise1 = effectorValues;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id1, 1, 1, anim1, 10, (float) num, construction_mass1, refinedMetals, 1600f, BuildLocationRule.Anywhere, decor1, noise1);
    buildingDef.ViewMode = OverlayModes.Logic.ID;
    buildingDef.ObjectLayer = ObjectLayer.LogicWire;
    buildingDef.TileLayer = ObjectLayer.LogicWireTile;
    buildingDef.ReplacementLayer = ObjectLayer.ReplacementLogicWire;
    buildingDef.SceneLayer = Grid.SceneLayer.LogicWires;
    buildingDef.Floodable = false;
    buildingDef.Overheatable = false;
    buildingDef.Entombable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "small";
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.isKAnimTile = true;
    buildingDef.isUtility = true;
    buildingDef.DragBuild = true;
    GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, id);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    go.AddOrGet<LogicWire>();
    KAnimGraphTileVisualizer graphTileVisualizer = go.AddOrGet<KAnimGraphTileVisualizer>();
    graphTileVisualizer.connectionSource = KAnimGraphTileVisualizer.ConnectionSource.Logic;
    graphTileVisualizer.isPhysicalBuilding = true;
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    base.DoPostConfigureUnderConstruction(go);
    go.GetComponent<Constructable>().isDiggingRequired = false;
    KAnimGraphTileVisualizer graphTileVisualizer = go.AddOrGet<KAnimGraphTileVisualizer>();
    graphTileVisualizer.connectionSource = KAnimGraphTileVisualizer.ConnectionSource.Logic;
    graphTileVisualizer.isPhysicalBuilding = false;
  }

  protected void DoPostConfigureComplete(LogicWire.BitDepth rating, GameObject go)
  {
    go.GetComponent<LogicWire>().MaxBitDepth = rating;
    int bitDepthAsInt = LogicWire.GetBitDepthAsInt(rating);
    Descriptor descriptor = new Descriptor();
    descriptor.SetupDescriptor(string.Format((string) UI.BUILDINGEFFECTS.MAX_BITS, (object) bitDepthAsInt), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.MAX_BITS, (object[]) Array.Empty<object>()));
    BuildingDef def = go.GetComponent<Building>().Def;
    if (def.EffectDescription == null)
      def.EffectDescription = new List<Descriptor>();
    def.EffectDescription.Add(descriptor);
  }
}
