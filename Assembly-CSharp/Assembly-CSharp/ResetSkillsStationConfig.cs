﻿// Decompiled with JetBrains decompiler
// Type: ResetSkillsStationConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class ResetSkillsStationConfig : IBuildingConfig
{
  public const string ID = "ResetSkillsStation";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] allMetals = MATERIALS.ALL_METALS;
    EffectorValues tieR1 = NOISE_POLLUTION.NOISY.TIER1;
    EffectorValues none = BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("ResetSkillsStation", 3, 3, "reSpeccer_kanim", 30, 30f, tieR4, allMetals, 1600f, BuildLocationRule.OnFloor, none, noise);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 480f;
    buildingDef.ExhaustKilowattsWhenActive = 0.5f;
    buildingDef.SelfHeatKilowattsWhenActive = 4f;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "large";
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    go.AddTag(GameTags.NotRoomAssignable);
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    go.AddOrGet<Ownable>().slotID = Db.Get().AssignableSlots.ResetSkillsStation.Id;
    ResetSkillsStation resetSkillsStation = go.AddOrGet<ResetSkillsStation>();
    resetSkillsStation.workTime = 180f;
    resetSkillsStation.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_reSpeccer_kanim")
    };
    resetSkillsStation.workLayer = Grid.SceneLayer.BuildingFront;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
