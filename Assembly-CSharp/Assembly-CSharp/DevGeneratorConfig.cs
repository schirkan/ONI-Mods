// Decompiled with JetBrains decompiler
// Type: DevGeneratorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class DevGeneratorConfig : IBuildingConfig
{
  public const string ID = "DevGenerator";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR0 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
    string[] allMetals = MATERIALS.ALL_METALS;
    EffectorValues tieR5 = NOISE_POLLUTION.NOISY.TIER5;
    EffectorValues tieR2 = BUILDINGS.DECOR.PENALTY.TIER2;
    EffectorValues noise = tieR5;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("DevGenerator", 1, 1, "dev_generator_kanim", 100, 3f, tieR0, allMetals, 2400f, BuildLocationRule.Anywhere, tieR2, noise);
    buildingDef.GeneratorWattageRating = 100000f;
    buildingDef.GeneratorBaseCapacity = 200000f;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.AudioSize = "large";
    buildingDef.Floodable = false;
    buildingDef.DebugOnly = true;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    EnergyGenerator energyGenerator = go.AddOrGet<EnergyGenerator>();
    energyGenerator.hasMeter = false;
    energyGenerator.ignoreBatteryRefillPercent = true;
    energyGenerator.formula = new EnergyGenerator.Formula();
    energyGenerator.powerDistributionOrder = 9;
  }

  public override void DoPostConfigureComplete(GameObject go) => go.AddOrGetDef<PoweredActiveController.Def>();
}
