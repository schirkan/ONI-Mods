// Decompiled with JetBrains decompiler
// Type: SwitchConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class SwitchConfig : IBuildingConfig
{
  public static string ID = "Switch";

  public override BuildingDef CreateBuildingDef()
  {
    string id = SwitchConfig.ID;
    float[] tieR2 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] allMetals = MATERIALS.ALL_METALS;
    EffectorValues none1 = NOISE_POLLUTION.NONE;
    EffectorValues none2 = BUILDINGS.DECOR.NONE;
    EffectorValues noise = none1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, 1, 1, "switchpower_kanim", 10, 30f, tieR2, allMetals, 1600f, BuildLocationRule.Anywhere, none2, noise);
    buildingDef.Overheatable = false;
    buildingDef.Floodable = false;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    SoundEventVolumeCache.instance.AddVolume("switchpower_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
    SoundEventVolumeCache.instance.AddVolume("switchpower_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<BuildingComplete>().isManuallyOperated = false;
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
    CircuitSwitch circuitSwitch = go.AddOrGet<CircuitSwitch>();
    circuitSwitch.objectLayer = ObjectLayer.Wire;
    circuitSwitch.manuallyControlled = false;
    Prioritizable.AddRef(go);
  }

  public override void DoPostConfigureComplete(GameObject go) => go.AddComponent<BuildingCellVisualizer>();
}
