// Decompiled with JetBrains decompiler
// Type: SolarPanelConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class SolarPanelConfig : IBuildingConfig
{
  public const string ID = "SolarPanel";
  public const float WATTS_PER_LUX = 0.00053f;
  public const float MAX_WATTS = 380f;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR3 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] glasses = MATERIALS.GLASSES;
    EffectorValues tieR5 = NOISE_POLLUTION.NOISY.TIER5;
    EffectorValues tieR2 = BUILDINGS.DECOR.PENALTY.TIER2;
    EffectorValues noise = tieR5;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("SolarPanel", 7, 3, "solar_panel_kanim", 100, 120f, tieR3, glasses, 2400f, BuildLocationRule.Anywhere, tieR2, noise);
    buildingDef.GeneratorWattageRating = 380f;
    buildingDef.GeneratorBaseCapacity = buildingDef.GeneratorWattageRating;
    buildingDef.ExhaustKilowattsWhenActive = 0.0f;
    buildingDef.SelfHeatKilowattsWhenActive = 0.0f;
    buildingDef.BuildLocationRule = BuildLocationRule.Anywhere;
    buildingDef.HitPoints = 10;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.AudioSize = "large";
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    go.AddOrGet<LoopingSounds>();
    Prioritizable.AddRef(go);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<Repairable>().expectedRepairTime = 52.5f;
    go.AddOrGet<SolarPanel>().powerDistributionOrder = 9;
    go.AddOrGetDef<PoweredActiveController.Def>();
  }
}
