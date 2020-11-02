﻿// Decompiled with JetBrains decompiler
// Type: MachineShopConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class MachineShopConfig : IBuildingConfig
{
  public const string ID = "MachineShop";
  public static readonly Tag MATERIAL_FOR_TINKER = GameTags.RefinedMetal;
  public const float MASS_PER_TINKER = 5f;
  public static readonly string ROLE_PERK = "IncreaseMachinery";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR3 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    EffectorValues tieR1 = NOISE_POLLUTION.NOISY.TIER1;
    EffectorValues none = BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("MachineShop", 4, 2, "machineshop_kanim", 30, 30f, tieR3, refinedMetals, 1600f, BuildLocationRule.OnFloor, none, noise);
    buildingDef.Deprecated = true;
    buildingDef.ViewMode = OverlayModes.Rooms.ID;
    buildingDef.Overheatable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "large";
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.MachineShop);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}