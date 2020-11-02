// Decompiled with JetBrains decompiler
// Type: AdvancedDoctorStationConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class AdvancedDoctorStationConfig : IBuildingConfig
{
  public const string ID = "AdvancedDoctorStation";
  private static Tag SUPPLY_TAG = (Tag) "AdvancedCure";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR3 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    EffectorValues none1 = NOISE_POLLUTION.NONE;
    EffectorValues none2 = BUILDINGS.DECOR.NONE;
    EffectorValues noise = none1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("AdvancedDoctorStation", 2, 3, "bed_medical_kanim", 100, 10f, tieR3, refinedMetals, 1600f, BuildLocationRule.OnFloor, none2, noise);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 480f;
    buildingDef.ExhaustKilowattsWhenActive = 0.25f;
    buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
    buildingDef.AudioCategory = "Metal";
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.Clinic);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    Storage storage = go.AddOrGet<Storage>();
    storage.showInUI = true;
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.requestedItemTag = AdvancedDoctorStationConfig.SUPPLY_TAG;
    manualDeliveryKg.capacity = 10f;
    manualDeliveryKg.refillMass = 5f;
    manualDeliveryKg.minimumMass = 1f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.DoctorFetch.IdHash;
    manualDeliveryKg.operationalRequirement = FetchOrder2.OperationalRequirement.Functional;
    DoctorStation doctorStation = go.AddOrGet<DoctorStation>();
    doctorStation.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_medical_bed_kanim")
    };
    doctorStation.workLayer = Grid.SceneLayer.BuildingFront;
    doctorStation.supplyTag = AdvancedDoctorStationConfig.SUPPLY_TAG;
    RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
    roomTracker.requiredRoomType = Db.Get().RoomTypes.Hospital.Id;
    roomTracker.requirement = RoomTracker.Requirement.CustomRecommended;
    roomTracker.customStatusItemID = Db.Get().BuildingStatusItems.ClinicOutsideHospital.Id;
    DoctorStationDoctorWorkable stationDoctorWorkable = go.AddOrGet<DoctorStationDoctorWorkable>();
    stationDoctorWorkable.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_medical_bed_doctor_kanim")
    };
    stationDoctorWorkable.SetWorkTime(60f);
    stationDoctorWorkable.requiredSkillPerk = Db.Get().SkillPerks.CanAdvancedMedicine.Id;
  }
}
