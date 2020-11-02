// Decompiled with JetBrains decompiler
// Type: ApothecaryConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class ApothecaryConfig : IBuildingConfig
{
  public const string ID = "Apothecary";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] allMetals = MATERIALS.ALL_METALS;
    EffectorValues none1 = NOISE_POLLUTION.NONE;
    EffectorValues none2 = BUILDINGS.DECOR.NONE;
    EffectorValues noise = none1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("Apothecary", 2, 3, "apothecary_kanim", 30, 120f, tieR4, allMetals, 800f, BuildLocationRule.OnFloor, none2, noise);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 240f;
    buildingDef.ExhaustKilowattsWhenActive = 0.25f;
    buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Glass";
    buildingDef.AudioSize = "large";
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    Prioritizable.AddRef(go);
    go.AddOrGet<DropAllWorkable>();
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    Apothecary apothecary = go.AddOrGet<Apothecary>();
    BuildingTemplates.CreateComplexFabricatorStorage(go, (ComplexFabricator) apothecary);
    go.AddOrGet<ComplexFabricatorWorkable>();
    go.AddOrGet<FabricatorIngredientStatusManager>();
    go.AddOrGet<CopyBuildingSettings>();
  }

  public override void DoPostConfigureComplete(GameObject go) => go.AddOrGetDef<PoweredActiveStoppableController.Def>();
}
