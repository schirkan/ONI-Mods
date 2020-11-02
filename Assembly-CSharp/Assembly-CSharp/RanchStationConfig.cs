// Decompiled with JetBrains decompiler
// Type: RanchStationConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using TUNING;
using UnityEngine;

public class RanchStationConfig : IBuildingConfig
{
  public const string ID = "RanchStation";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] allMetals = MATERIALS.ALL_METALS;
    EffectorValues tieR1 = NOISE_POLLUTION.NOISY.TIER1;
    EffectorValues none = BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("RanchStation", 2, 3, "rancherstation_kanim", 30, 30f, tieR4, allMetals, 1600f, BuildLocationRule.OnFloor, none, noise);
    buildingDef.ViewMode = OverlayModes.Rooms.ID;
    buildingDef.Overheatable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "large";
    buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RanchStation);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<LogicOperationalController>();
    RanchStation.Def def = go.AddOrGetDef<RanchStation.Def>();
    def.isCreatureEligibleToBeRanchedCb = (Func<GameObject, RanchStation.Instance, bool>) ((creature_go, ranch_station_smi) => !creature_go.GetComponent<Effects>().HasEffect("Ranched"));
    def.onRanchCompleteCb = (System.Action<GameObject>) (creature_go =>
    {
      RanchStation.Instance targetRanchStation = creature_go.GetSMI<RanchableMonitor.Instance>().targetRanchStation;
      RancherChore.RancherChoreStates.Instance smi = targetRanchStation.GetSMI<RancherChore.RancherChoreStates.Instance>();
      float num = (float) (1.0 + (double) targetRanchStation.GetSMI<RancherChore.RancherChoreStates.Instance>().sm.rancher.Get(smi).GetAttributes().Get(Db.Get().Attributes.Ranching.Id).GetTotalValue() * 0.100000001490116);
      creature_go.GetComponent<Effects>().Add("Ranched", true).timeRemaining *= num;
    });
    def.ranchedPreAnim = (HashedString) "grooming_pre";
    def.ranchedLoopAnim = (HashedString) "grooming_loop";
    def.ranchedPstAnim = (HashedString) "grooming_pst";
    def.getTargetRanchCell = (Func<RanchStation.Instance, int>) (smi =>
    {
      int cell = Grid.InvalidCell;
      if (!smi.IsNullOrStopped())
      {
        cell = Grid.CellRight(Grid.PosToCell(smi.transform.GetPosition()));
        if (!smi.targetRanchable.IsNullOrStopped() && smi.targetRanchable.HasTag(GameTags.Creatures.Flyer))
          cell = Grid.CellAbove(cell);
      }
      return cell;
    });
    RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
    roomTracker.requiredRoomType = Db.Get().RoomTypes.CreaturePen.Id;
    roomTracker.requirement = RoomTracker.Requirement.Required;
    go.AddOrGet<SkillPerkMissingComplainer>().requiredSkillPerk = Db.Get().SkillPerks.CanWrangleCreatures.Id;
    Prioritizable.AddRef(go);
  }
}
