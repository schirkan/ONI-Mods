﻿// Decompiled with JetBrains decompiler
// Type: BeachChairConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class BeachChairConfig : IBuildingConfig
{
  public const string ID = "BeachChair";
  public const int TAN_LUX = 10000;
  private const float TANK_SIZE_KG = 20f;
  private const float SPILL_RATE_KG = 0.05f;

  public override BuildingDef CreateBuildingDef()
  {
    float[] construction_mass = new float[2]{ 400f, 2f };
    string[] construction_materials = new string[2]
    {
      "BuildableRaw",
      "BuildingFiber"
    };
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR4 = BUILDINGS.DECOR.BONUS.TIER4;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("BeachChair", 2, 3, "beach_chair_kanim", 30, 60f, construction_mass, construction_materials, 1600f, BuildLocationRule.OnFloor, tieR4, noise);
    buildingDef.DlcId = "PACK1";
    buildingDef.Floodable = true;
    buildingDef.AudioCategory = "Metal";
    buildingDef.Overheatable = true;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RecBuilding);
    go.AddOrGet<BeachChairWorkable>().basePriority = RELAXATION.PRIORITY.TIER4;
    BeachChair beachChair = go.AddOrGet<BeachChair>();
    beachChair.specificEffectUnlit = "BeachChairUnlit";
    beachChair.specificEffectLit = "BeachChairLit";
    beachChair.trackingEffect = "RecentlyBeachChair";
    RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
    roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
    roomTracker.requirement = RoomTracker.Requirement.Recommended;
    go.AddOrGet<AnimTileable>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
