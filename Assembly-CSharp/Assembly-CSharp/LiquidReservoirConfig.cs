// Decompiled with JetBrains decompiler
// Type: LiquidReservoirConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class LiquidReservoirConfig : IBuildingConfig
{
  public const string ID = "LiquidReservoir";
  private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;
  private const int WIDTH = 2;
  private const int HEIGHT = 3;

  public override BuildingDef CreateBuildingDef()
  {
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("LiquidReservoir", 2, 3, "liquidreservoir_kanim", 100, 120f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4, MATERIALS.ALL_METALS, 800f, BuildLocationRule.OnFloor, TUNING.BUILDINGS.DECOR.PENALTY.TIER1, TUNING.NOISE_POLLUTION.NOISY.TIER0);
    buildingDef.InputConduitType = ConduitType.Liquid;
    buildingDef.OutputConduitType = ConduitType.Liquid;
    buildingDef.Floodable = false;
    buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.UtilityInputOffset = new CellOffset(1, 2);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
    buildingDef.LogicOutputPorts = new List<LogicPorts.Port>()
    {
      LogicPorts.Port.OutputPort(SmartReservoir.PORT_ID, new CellOffset(0, 0), (string) STRINGS.BUILDINGS.PREFABS.SMARTRESERVOIR.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.SMARTRESERVOIR.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.SMARTRESERVOIR.LOGIC_PORT_INACTIVE)
    };
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, "LiquidReservoir");
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<Reservoir>();
    Storage defaultStorage = BuildingTemplates.CreateDefaultStorage(go);
    defaultStorage.showDescriptor = true;
    defaultStorage.allowItemRemoval = false;
    defaultStorage.storageFilters = STORAGEFILTERS.LIQUIDS;
    defaultStorage.capacityKg = 5000f;
    defaultStorage.SetDefaultStoredItemModifiers(GasReservoirConfig.ReservoirStoredItemModifiers);
    go.AddOrGet<SmartReservoir>();
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Liquid;
    conduitConsumer.ignoreMinMassCheck = true;
    conduitConsumer.forceAlwaysSatisfied = true;
    conduitConsumer.alwaysConsume = true;
    conduitConsumer.capacityKG = defaultStorage.capacityKg;
    ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
    conduitDispenser.conduitType = ConduitType.Liquid;
    conduitDispenser.elementFilter = (SimHashes[]) null;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGetDef<StorageController.Def>();
    go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits);
  }
}
