// Decompiled with JetBrains decompiler
// Type: TemperatureControlledSwitchConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class TemperatureControlledSwitchConfig : IBuildingConfig
{
  public static string ID = "TemperatureControlledSwitch";

  public override BuildingDef CreateBuildingDef()
  {
    string id = TemperatureControlledSwitchConfig.ID;
    float[] tieR3 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] allMetals = MATERIALS.ALL_METALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR0 = BUILDINGS.DECOR.PENALTY.TIER0;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, 1, 1, "switchthermal_kanim", 30, 30f, tieR3, allMetals, 1600f, BuildLocationRule.Anywhere, tieR0, noise);
    buildingDef.Deprecated = true;
    buildingDef.Overheatable = false;
    buildingDef.Floodable = false;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    SoundEventVolumeCache.instance.AddVolume("switchthermal_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
    SoundEventVolumeCache.instance.AddVolume("switchthermal_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
    TemperatureControlledSwitch controlledSwitch = go.AddOrGet<TemperatureControlledSwitch>();
    controlledSwitch.objectLayer = ObjectLayer.Wire;
    controlledSwitch.manuallyControlled = false;
    controlledSwitch.minTemp = 0.0f;
    controlledSwitch.maxTemp = 573.15f;
  }

  public override void DoPostConfigureComplete(GameObject go) => go.AddComponent<BuildingCellVisualizer>();
}
