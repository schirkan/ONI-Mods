// Decompiled with JetBrains decompiler
// Type: LogicRibbonReaderConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class LogicRibbonReaderConfig : IBuildingConfig
{
  public static string ID = "LogicRibbonReader";

  public override BuildingDef CreateBuildingDef()
  {
    string id = LogicRibbonReaderConfig.ID;
    float[] tieR0_1 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    EffectorValues none = TUNING.NOISE_POLLUTION.NONE;
    EffectorValues tieR0_2 = TUNING.BUILDINGS.DECOR.PENALTY.TIER0;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, 2, 1, "logic_ribbon_reader_kanim", 30, 30f, tieR0_1, refinedMetals, 1600f, BuildLocationRule.Anywhere, tieR0_2, noise);
    buildingDef.Overheatable = false;
    buildingDef.Floodable = false;
    buildingDef.Entombable = false;
    buildingDef.PermittedRotations = PermittedRotations.R360;
    buildingDef.InitialOrientation = Orientation.R180;
    buildingDef.ViewMode = OverlayModes.Logic.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.ObjectLayer = ObjectLayer.LogicGate;
    buildingDef.SceneLayer = Grid.SceneLayer.LogicGates;
    buildingDef.AlwaysOperational = true;
    buildingDef.LogicInputPorts = new List<LogicPorts.Port>()
    {
      LogicPorts.Port.RibbonInputPort(LogicRibbonReader.INPUT_PORT_ID, new CellOffset(0, 0), (string) STRINGS.BUILDINGS.PREFABS.LOGICRIBBONREADER.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.LOGICRIBBONREADER.INPUT_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.LOGICRIBBONREADER.INPUT_PORT_INACTIVE, true)
    };
    buildingDef.LogicOutputPorts = new List<LogicPorts.Port>()
    {
      LogicPorts.Port.OutputPort(LogicRibbonReader.OUTPUT_PORT_ID, new CellOffset(1, 0), (string) STRINGS.BUILDINGS.PREFABS.LOGICRIBBONREADER.LOGIC_PORT_OUTPUT, (string) STRINGS.BUILDINGS.PREFABS.LOGICRIBBONREADER.OUTPUT_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.LOGICRIBBONREADER.OUTPUT_PORT_INACTIVE, true)
    };
    SoundEventVolumeCache.instance.AddVolume("door_internal_kanim", "Open_DoorInternal", TUNING.NOISE_POLLUTION.NOISY.TIER3);
    SoundEventVolumeCache.instance.AddVolume("door_internal_kanim", "Close_DoorInternal", TUNING.NOISE_POLLUTION.NOISY.TIER3);
    GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, LogicRibbonReaderConfig.ID);
    return buildingDef;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<LogicRibbonReader>();
    go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits);
  }
}
