// Decompiled with JetBrains decompiler
// Type: DoctorStationConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class DoctorStationConfig : IBuildingConfig
{
  public const string ID = "DoctorStation";
  private static Tag SUPPLY_TAG = (Tag) "IntermediateCure";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR3 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] rawMinerals = MATERIALS.RAW_MINERALS;
    EffectorValues none1 = NOISE_POLLUTION.NONE;
    EffectorValues none2 = BUILDINGS.DECOR.NONE;
    EffectorValues noise = none1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("DoctorStation", 3, 2, "treatment_chair_kanim", 10, 10f, tieR3, rawMinerals, 1600f, BuildLocationRule.OnFloor, none2, noise);
    buildingDef.Overheatable = false;
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
    manualDeliveryKg.requestedItemTag = DoctorStationConfig.SUPPLY_TAG;
    manualDeliveryKg.capacity = 10f;
    manualDeliveryKg.refillMass = 5f;
    manualDeliveryKg.minimumMass = 1f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.DoctorFetch.IdHash;
    manualDeliveryKg.operationalRequirement = FetchOrder2.OperationalRequirement.Functional;
    DoctorStation doctorStation = go.AddOrGet<DoctorStation>();
    doctorStation.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_treatment_chair_sick_kanim")
    };
    doctorStation.workLayer = Grid.SceneLayer.BuildingFront;
    doctorStation.supplyTag = DoctorStationConfig.SUPPLY_TAG;
    RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
    roomTracker.requiredRoomType = Db.Get().RoomTypes.Hospital.Id;
    roomTracker.requirement = RoomTracker.Requirement.CustomRecommended;
    roomTracker.customStatusItemID = Db.Get().BuildingStatusItems.ClinicOutsideHospital.Id;
    DoctorStationDoctorWorkable stationDoctorWorkable = go.AddOrGet<DoctorStationDoctorWorkable>();
    stationDoctorWorkable.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_treatment_chair_doctor_kanim")
    };
    stationDoctorWorkable.SetWorkTime(40f);
    stationDoctorWorkable.requiredSkillPerk = Db.Get().SkillPerks.CanDoctor.Id;
  }
}
