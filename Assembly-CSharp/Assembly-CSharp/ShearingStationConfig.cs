// Decompiled with JetBrains decompiler
// Type: ShearingStationConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using TUNING;
using UnityEngine;

public class ShearingStationConfig : IBuildingConfig
{
  public const string ID = "ShearingStation";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] rawMinerals = MATERIALS.RAW_MINERALS;
    EffectorValues none1 = NOISE_POLLUTION.NONE;
    EffectorValues none2 = BUILDINGS.DECOR.NONE;
    EffectorValues noise = none1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("ShearingStation", 3, 3, "shearing_station_kanim", 100, 10f, tieR4, rawMinerals, 1600f, BuildLocationRule.OnFloor, none2, noise);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 60f;
    buildingDef.ExhaustKilowattsWhenActive = 0.125f;
    buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
    buildingDef.Floodable = true;
    buildingDef.Entombable = true;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "large";
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
    buildingDef.DefaultAnimState = "on";
    buildingDef.ShowInBuildMenu = true;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LoopingSounds>();
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RanchStation);
    RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
    roomTracker.requiredRoomType = Db.Get().RoomTypes.CreaturePen.Id;
    roomTracker.requirement = RoomTracker.Requirement.Required;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    RanchStation.Def def = go.AddOrGetDef<RanchStation.Def>();
    def.isCreatureEligibleToBeRanchedCb = (Func<GameObject, RanchStation.Instance, bool>) ((creature_go, ranch_station_smi) =>
    {
      ScaleGrowthMonitor.Instance smi = creature_go.GetSMI<ScaleGrowthMonitor.Instance>();
      return smi != null && smi.IsFullyGrown();
    });
    def.onRanchCompleteCb = (System.Action<GameObject>) (creature_go => creature_go.GetSMI<ScaleGrowthMonitor.Instance>().Shear());
    def.interactLoopCount = 6;
    def.rancherInteractAnim = (HashedString) "anim_interacts_shearingstation_kanim";
    def.synchronizeBuilding = true;
    Prioritizable.AddRef(go);
  }
}
