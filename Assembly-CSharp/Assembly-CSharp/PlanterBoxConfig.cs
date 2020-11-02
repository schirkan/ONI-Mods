// Decompiled with JetBrains decompiler
// Type: PlanterBoxConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class PlanterBoxConfig : IBuildingConfig
{
  public const string ID = "PlanterBox";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR2 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] farmable = MATERIALS.FARMABLE;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR1 = BUILDINGS.DECOR.PENALTY.TIER1;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("PlanterBox", 1, 1, "planterbox_kanim", 10, 3f, tieR2, farmable, 800f, BuildLocationRule.OnFloor, tieR1, noise);
    buildingDef.ForegroundLayer = Grid.SceneLayer.BuildingBack;
    buildingDef.Overheatable = false;
    buildingDef.Floodable = false;
    buildingDef.AudioCategory = "Glass";
    buildingDef.AudioSize = "large";
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    Storage storage = go.AddOrGet<Storage>();
    PlantablePlot plantablePlot = go.AddOrGet<PlantablePlot>();
    plantablePlot.AddDepositTag(GameTags.CropSeed);
    plantablePlot.SetFertilizationFlags(true, false);
    go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.Farm;
    BuildingTemplates.CreateDefaultStorage(go);
    List<Storage.StoredItemModifier> standardSealedStorage = Storage.StandardSealedStorage;
    storage.SetDefaultStoredItemModifiers(standardSealedStorage);
    go.AddOrGet<DropAllWorkable>();
    go.AddOrGet<PlanterBox>();
    go.AddOrGet<AnimTileable>();
    Prioritizable.AddRef(go);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
