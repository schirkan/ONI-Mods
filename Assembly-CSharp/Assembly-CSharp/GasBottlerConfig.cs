﻿// Decompiled with JetBrains decompiler
// Type: GasBottlerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class GasBottlerConfig : IBuildingConfig
{
  public const string ID = "GasBottler";
  private const ConduitType CONDUIT_TYPE = ConduitType.Gas;
  private const int WIDTH = 3;
  private const int HEIGHT = 2;

  public override BuildingDef CreateBuildingDef()
  {
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("GasBottler", 3, 2, "gas_bottler_kanim", 100, 120f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER4, MATERIALS.ALL_METALS, 800f, BuildLocationRule.OnFloor, BUILDINGS.DECOR.PENALTY.TIER1, NOISE_POLLUTION.NOISY.TIER0);
    buildingDef.InputConduitType = ConduitType.Gas;
    buildingDef.Floodable = false;
    buildingDef.ViewMode = OverlayModes.GasConduits.ID;
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.GasVentIDs, "GasBottler");
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    Storage defaultStorage = BuildingTemplates.CreateDefaultStorage(go);
    defaultStorage.showDescriptor = true;
    defaultStorage.storageFilters = STORAGEFILTERS.GASES;
    defaultStorage.capacityKg = 10f;
    defaultStorage.allowItemRemoval = false;
    go.AddOrGet<DropAllWorkable>();
    GasBottler gasBottler = go.AddOrGet<GasBottler>();
    gasBottler.storage = defaultStorage;
    gasBottler.workTime = 9f;
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.storage = defaultStorage;
    conduitConsumer.conduitType = ConduitType.Gas;
    conduitConsumer.ignoreMinMassCheck = true;
    conduitConsumer.forceAlwaysSatisfied = true;
    conduitConsumer.alwaysConsume = true;
    conduitConsumer.capacityKG = defaultStorage.capacityKg;
    conduitConsumer.keepZeroMassObject = false;
  }

  public override void DoPostConfigureComplete(GameObject go) => go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits);
}
