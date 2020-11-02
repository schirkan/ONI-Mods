// Decompiled with JetBrains decompiler
// Type: BaseBatteryConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public abstract class BaseBatteryConfig : IBuildingConfig
{
  public BuildingDef CreateBuildingDef(
    string id,
    int width,
    int height,
    int hitpoints,
    string anim,
    float construction_time,
    float[] construction_mass,
    string[] construction_materials,
    float melting_point,
    float exhaust_temperature_active,
    float self_heat_kilowatts_active,
    EffectorValues decor,
    EffectorValues noise)
  {
    string id1 = id;
    int width1 = width;
    int height1 = height;
    int num1 = hitpoints;
    string anim1 = anim;
    int hitpoints1 = num1;
    double num2 = (double) construction_time;
    float[] construction_mass1 = construction_mass;
    string[] construction_materials1 = construction_materials;
    double num3 = (double) melting_point;
    EffectorValues tieR0 = NOISE_POLLUTION.NOISY.TIER0;
    EffectorValues decor1 = decor;
    EffectorValues noise1 = tieR0;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id1, width1, height1, anim1, hitpoints1, (float) num2, construction_mass1, construction_materials1, (float) num3, BuildLocationRule.OnFloor, decor1, noise1);
    buildingDef.ExhaustKilowattsWhenActive = exhaust_temperature_active;
    buildingDef.SelfHeatKilowattsWhenActive = self_heat_kilowatts_active;
    buildingDef.Entombable = false;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.RequiresPowerOutput = true;
    buildingDef.UseWhitePowerOutputConnectorColour = true;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) => go.AddComponent<RequireInputs>();

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<Battery>().powerSortOrder = 1000;
    go.AddOrGetDef<PoweredActiveController.Def>();
  }
}
