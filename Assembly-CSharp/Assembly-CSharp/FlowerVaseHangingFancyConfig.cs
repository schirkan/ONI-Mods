﻿// Decompiled with JetBrains decompiler
// Type: FlowerVaseHangingFancyConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class FlowerVaseHangingFancyConfig : IBuildingConfig
{
  public const string ID = "FlowerVaseHangingFancy";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR1 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
    string[] transparents = MATERIALS.TRANSPARENTS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues decor = new EffectorValues()
    {
      amount = BUILDINGS.DECOR.BONUS.TIER1.amount,
      radius = BUILDINGS.DECOR.BONUS.TIER3.radius
    };
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("FlowerVaseHangingFancy", 1, 2, "flowervase_hanging_kanim", 10, 10f, tieR1, transparents, 800f, BuildLocationRule.OnCeiling, decor, noise);
    buildingDef.Floodable = false;
    buildingDef.Overheatable = false;
    buildingDef.ViewMode = OverlayModes.Decor.ID;
    buildingDef.AudioCategory = "Glass";
    buildingDef.AudioSize = "large";
    buildingDef.GenerateOffsets(1, 1);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<Storage>();
    Prioritizable.AddRef(go);
    PlantablePlot plantablePlot = go.AddOrGet<PlantablePlot>();
    plantablePlot.AddDepositTag(GameTags.DecorSeed);
    plantablePlot.plantLayer = Grid.SceneLayer.BuildingFront;
    plantablePlot.occupyingObjectVisualOffset = new Vector3(0.0f, -0.45f, 0.0f);
    go.AddOrGet<FlowerVase>();
    go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
