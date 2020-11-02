﻿// Decompiled with JetBrains decompiler
// Type: LogicDiseaseSensorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class LogicDiseaseSensorConfig : IBuildingConfig
{
  public static string ID = "LogicDiseaseSensor";

  public override BuildingDef CreateBuildingDef()
  {
    string id = LogicDiseaseSensorConfig.ID;
    float[] construction_mass = new float[2]
    {
      TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0[0],
      TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1[0]
    };
    string[] construction_materials = new string[2]
    {
      "RefinedMetal",
      "Plastic"
    };
    EffectorValues none = TUNING.NOISE_POLLUTION.NONE;
    EffectorValues tieR0 = TUNING.BUILDINGS.DECOR.PENALTY.TIER0;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, 1, 1, "diseasesensor_kanim", 30, 30f, construction_mass, construction_materials, 1600f, BuildLocationRule.Anywhere, tieR0, noise);
    buildingDef.Overheatable = false;
    buildingDef.Floodable = false;
    buildingDef.Entombable = false;
    buildingDef.ViewMode = OverlayModes.Logic.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    buildingDef.AlwaysOperational = true;
    buildingDef.LogicOutputPorts = new List<LogicPorts.Port>()
    {
      LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), (string) STRINGS.BUILDINGS.PREFABS.LOGICDISEASESENSOR.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.LOGICDISEASESENSOR.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.LOGICDISEASESENSOR.LOGIC_PORT_INACTIVE, true)
    };
    SoundEventVolumeCache.instance.AddVolume("diseasesensor_kanim", "PowerSwitch_on", TUNING.NOISE_POLLUTION.NOISY.TIER3);
    SoundEventVolumeCache.instance.AddVolume("diseasesensor_kanim", "PowerSwitch_off", TUNING.NOISE_POLLUTION.NOISY.TIER3);
    GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, LogicDiseaseSensorConfig.ID);
    return buildingDef;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    LogicDiseaseSensor logicDiseaseSensor = go.AddOrGet<LogicDiseaseSensor>();
    logicDiseaseSensor.Threshold = 0.0f;
    logicDiseaseSensor.ActivateAboveThreshold = true;
    logicDiseaseSensor.manuallyControlled = false;
    go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits);
  }
}