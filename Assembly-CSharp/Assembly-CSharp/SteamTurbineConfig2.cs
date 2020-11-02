﻿// Decompiled with JetBrains decompiler
// Type: SteamTurbineConfig2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class SteamTurbineConfig2 : IBuildingConfig
{
  public const string ID = "SteamTurbine2";
  public static float MAX_WATTAGE = 850f;
  private static readonly List<Storage.StoredItemModifier> StoredItemModifiers = new List<Storage.StoredItemModifier>()
  {
    Storage.StoredItemModifier.Hide,
    Storage.StoredItemModifier.Insulate,
    Storage.StoredItemModifier.Seal
  };

  public override BuildingDef CreateBuildingDef()
  {
    string[] strArray = new string[2]
    {
      "RefinedMetal",
      "Plastic"
    };
    float[] construction_mass = new float[2]
    {
      BUILDINGS.CONSTRUCTION_MASS_KG.TIER5[0],
      BUILDINGS.CONSTRUCTION_MASS_KG.TIER3[0]
    };
    string[] construction_materials = strArray;
    EffectorValues none1 = NOISE_POLLUTION.NONE;
    EffectorValues none2 = BUILDINGS.DECOR.NONE;
    EffectorValues noise = none1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("SteamTurbine2", 5, 3, "steamturbine2_kanim", 30, 60f, construction_mass, construction_materials, 1600f, BuildLocationRule.OnFloor, none2, noise, 1f);
    buildingDef.OutputConduitType = ConduitType.Liquid;
    buildingDef.UtilityOutputOffset = new CellOffset(2, 2);
    buildingDef.GeneratorWattageRating = SteamTurbineConfig2.MAX_WATTAGE;
    buildingDef.GeneratorBaseCapacity = SteamTurbineConfig2.MAX_WATTAGE;
    buildingDef.Entombable = true;
    buildingDef.IsFoundation = false;
    buildingDef.PermittedRotations = PermittedRotations.FlipH;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.PowerOutputOffset = new CellOffset(1, 0);
    buildingDef.OverheatTemperature = 1273.15f;
    buildingDef.SelfHeatKilowattsWhenActive = 4f;
    buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
    return buildingDef;
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    base.DoPostConfigureUnderConstruction(go);
    go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.CanPowerTinker.Id;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    Storage storage1 = go.AddComponent<Storage>();
    storage1.showDescriptor = false;
    storage1.showInUI = false;
    storage1.storageFilters = STORAGEFILTERS.LIQUIDS;
    storage1.SetDefaultStoredItemModifiers(SteamTurbineConfig2.StoredItemModifiers);
    storage1.capacityKg = 10f;
    Storage storage2 = go.AddComponent<Storage>();
    storage2.showDescriptor = false;
    storage2.showInUI = false;
    storage2.storageFilters = STORAGEFILTERS.GASES;
    storage2.SetDefaultStoredItemModifiers(SteamTurbineConfig2.StoredItemModifiers);
    SteamTurbine steamTurbine = go.AddOrGet<SteamTurbine>();
    steamTurbine.srcElem = SimHashes.Steam;
    steamTurbine.destElem = SimHashes.Water;
    steamTurbine.pumpKGRate = 2f;
    steamTurbine.maxSelfHeat = 64f;
    steamTurbine.wasteHeatToTurbinePercent = 0.1f;
    ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
    conduitDispenser.elementFilter = new SimHashes[1]
    {
      SimHashes.Water
    };
    conduitDispenser.conduitType = ConduitType.Liquid;
    conduitDispenser.storage = storage1;
    conduitDispenser.alwaysDispense = true;
    go.AddOrGet<LogicOperationalController>();
    Prioritizable.AddRef(go);
    go.GetComponent<KPrefabID>().prefabSpawnFn += (KPrefabID.PrefabFn) (game_object =>
    {
      HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(game_object);
      StructureTemperaturePayload payload = GameComps.StructureTemperatures.GetPayload(handle);
      Extents extents = game_object.GetComponent<Building>().GetExtents();
      Extents newExtents = new Extents(extents.x, extents.y - 1, extents.width, extents.height + 1);
      payload.OverrideExtents(newExtents);
      GameComps.StructureTemperatures.SetPayload(handle, ref payload);
      Storage[] components = game_object.GetComponents<Storage>();
      game_object.GetComponent<SteamTurbine>().SetStorage(components[1], components[0]);
    });
    Tinkerable.MakePowerTinkerable(go);
  }
}
