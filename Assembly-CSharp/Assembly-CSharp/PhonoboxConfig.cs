﻿// Decompiled with JetBrains decompiler
// Type: PhonoboxConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class PhonoboxConfig : IBuildingConfig
{
  public const string ID = "Phonobox";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] rawMetals = MATERIALS.RAW_METALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR1 = BUILDINGS.DECOR.BONUS.TIER1;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("Phonobox", 5, 3, "jukebot_kanim", 30, 10f, tieR4, rawMetals, 1600f, BuildLocationRule.OnFloor, tieR1, noise);
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.Floodable = true;
    buildingDef.AudioCategory = "Metal";
    buildingDef.Overheatable = true;
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 960f;
    buildingDef.SelfHeatKilowattsWhenActive = 1f;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RecBuilding);
    go.AddOrGet<Phonobox>();
    RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
    roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
    roomTracker.requirement = RoomTracker.Requirement.Recommended;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
