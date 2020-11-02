// Decompiled with JetBrains decompiler
// Type: LogicCounterConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class LogicCounterConfig : IBuildingConfig
{
  public static string ID = "LogicCounter";

  public override BuildingDef CreateBuildingDef()
  {
    string id = LogicCounterConfig.ID;
    float[] tieR0_1 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    EffectorValues none = TUNING.NOISE_POLLUTION.NONE;
    EffectorValues tieR0_2 = TUNING.BUILDINGS.DECOR.PENALTY.TIER0;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, 1, 3, "logic_counter_kanim", 30, 30f, tieR0_1, refinedMetals, 1600f, BuildLocationRule.Anywhere, tieR0_2, noise);
    buildingDef.Overheatable = false;
    buildingDef.Floodable = false;
    buildingDef.Entombable = false;
    buildingDef.PermittedRotations = PermittedRotations.FlipV;
    buildingDef.ViewMode = OverlayModes.Logic.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.ObjectLayer = ObjectLayer.LogicGate;
    buildingDef.SceneLayer = Grid.SceneLayer.LogicGates;
    buildingDef.AlwaysOperational = true;
    buildingDef.LogicInputPorts = new List<LogicPorts.Port>()
    {
      LogicPorts.Port.InputPort(LogicCounter.INPUT_PORT_ID, new CellOffset(0, 0), (string) STRINGS.BUILDINGS.PREFABS.LOGICCOUNTER.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.LOGICCOUNTER.INPUT_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.LOGICCOUNTER.INPUT_PORT_INACTIVE, true),
      new LogicPorts.Port(LogicCounter.RESET_PORT_ID, new CellOffset(0, 1), (string) STRINGS.BUILDINGS.PREFABS.LOGICCOUNTER.LOGIC_PORT_RESET, (string) STRINGS.BUILDINGS.PREFABS.LOGICCOUNTER.RESET_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.LOGICCOUNTER.RESET_PORT_INACTIVE, false, LogicPortSpriteType.ResetUpdate, true)
    };
    buildingDef.LogicOutputPorts = new List<LogicPorts.Port>()
    {
      LogicPorts.Port.OutputPort(LogicCounter.OUTPUT_PORT_ID, new CellOffset(0, 2), (string) STRINGS.BUILDINGS.PREFABS.LOGICCOUNTER.LOGIC_PORT_OUTPUT, (string) STRINGS.BUILDINGS.PREFABS.LOGICCOUNTER.OUTPUT_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.LOGICCOUNTER.OUTPUT_PORT_INACTIVE)
    };
    SoundEventVolumeCache.instance.AddVolume("door_internal_kanim", "Open_DoorInternal", TUNING.NOISE_POLLUTION.NOISY.TIER3);
    SoundEventVolumeCache.instance.AddVolume("door_internal_kanim", "Close_DoorInternal", TUNING.NOISE_POLLUTION.NOISY.TIER3);
    GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, LogicCounterConfig.ID);
    return buildingDef;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<LogicCounter>().manuallyControlled = false;
    go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits);
    go.GetComponent<Switch>().defaultState = false;
  }
}
