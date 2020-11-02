// Decompiled with JetBrains decompiler
// Type: EggIncubatorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class EggIncubatorConfig : IBuildingConfig
{
  public const string ID = "EggIncubator";
  public static readonly List<Storage.StoredItemModifier> IncubatorStorage = new List<Storage.StoredItemModifier>()
  {
    Storage.StoredItemModifier.Preserve
  };

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR3 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR0 = BUILDINGS.DECOR.BONUS.TIER0;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("EggIncubator", 2, 3, "incubator_kanim", 30, 120f, tieR3, refinedMetals, 1600f, BuildLocationRule.OnFloor, tieR0, noise);
    buildingDef.AudioCategory = "Metal";
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 240f;
    buildingDef.ExhaustKilowattsWhenActive = 0.5f;
    buildingDef.SelfHeatKilowattsWhenActive = 4f;
    buildingDef.OverheatTemperature = 363.15f;
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    buildingDef.ForegroundLayer = Grid.SceneLayer.BuildingFront;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    Prioritizable.AddRef(go);
    BuildingTemplates.CreateDefaultStorage(go).SetDefaultStoredItemModifiers(EggIncubatorConfig.IncubatorStorage);
    EggIncubator eggIncubator = go.AddOrGet<EggIncubator>();
    eggIncubator.AddDepositTag(GameTags.Egg);
    eggIncubator.SetWorkTime(5f);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
