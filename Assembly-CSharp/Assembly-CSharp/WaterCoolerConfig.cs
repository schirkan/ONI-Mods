// Decompiled with JetBrains decompiler
// Type: WaterCoolerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class WaterCoolerConfig : IBuildingConfig
{
  public const string ID = "WaterCooler";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] rawMinerals = MATERIALS.RAW_MINERALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR1 = BUILDINGS.DECOR.BONUS.TIER1;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("WaterCooler", 2, 2, "watercooler_kanim", 30, 10f, tieR4, rawMinerals, 1600f, BuildLocationRule.OnFloor, tieR1, noise);
    buildingDef.Floodable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.Overheatable = false;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RecBuilding);
    Prioritizable.AddRef(go);
    Storage storage = go.AddOrGet<Storage>();
    storage.capacityKg = 10f;
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.requestedItemTag = GameTags.Water;
    manualDeliveryKg.capacity = 10f;
    manualDeliveryKg.refillMass = 9f;
    manualDeliveryKg.minimumMass = 1f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
    go.AddOrGet<WaterCooler>();
    RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
    roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
    roomTracker.requirement = RoomTracker.Requirement.Recommended;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
