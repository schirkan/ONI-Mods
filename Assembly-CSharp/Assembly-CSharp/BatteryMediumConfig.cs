// Decompiled with JetBrains decompiler
// Type: BatteryMediumConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class BatteryMediumConfig : BaseBatteryConfig
{
  public const string ID = "BatteryMedium";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] allMetals = MATERIALS.ALL_METALS;
    EffectorValues tieR1 = NOISE_POLLUTION.NOISY.TIER1;
    EffectorValues tieR2 = BUILDINGS.DECOR.PENALTY.TIER2;
    EffectorValues noise = tieR1;
    BuildingDef buildingDef = this.CreateBuildingDef("BatteryMedium", 2, 2, 30, "batterymed_kanim", 60f, tieR4, allMetals, 800f, 0.25f, 1f, tieR2, noise);
    SoundEventVolumeCache.instance.AddVolume("batterymed_kanim", "Battery_med_rattle", NOISE_POLLUTION.NOISY.TIER2);
    return buildingDef;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    Battery battery = go.AddOrGet<Battery>();
    battery.capacity = 40000f;
    battery.joulesLostPerSecond = 3.333333f;
    base.DoPostConfigureComplete(go);
  }
}
