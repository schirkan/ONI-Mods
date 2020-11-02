// Decompiled with JetBrains decompiler
// Type: ConduitSensorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

public abstract class ConduitSensorConfig : IBuildingConfig
{
  protected abstract ConduitType ConduitType { get; }

  protected BuildingDef CreateBuildingDef(
    string ID,
    string anim,
    float[] required_mass,
    string[] required_materials,
    List<LogicPorts.Port> output_ports)
  {
    string id = ID;
    string anim1 = anim;
    float[] construction_mass = required_mass;
    string[] construction_materials = required_materials;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR0 = BUILDINGS.DECOR.PENALTY.TIER0;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, 1, 1, anim1, 30, 30f, construction_mass, construction_materials, 1600f, BuildLocationRule.Anywhere, tieR0, noise);
    buildingDef.Overheatable = false;
    buildingDef.Floodable = false;
    buildingDef.Entombable = false;
    buildingDef.ViewMode = OverlayModes.Logic.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    buildingDef.AlwaysOperational = true;
    buildingDef.LogicOutputPorts = output_ports;
    SoundEventVolumeCache.instance.AddVolume(anim, "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
    SoundEventVolumeCache.instance.AddVolume(anim, "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
    GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, ID);
    return buildingDef;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
