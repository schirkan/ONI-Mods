﻿// Decompiled with JetBrains decompiler
// Type: SteamTurbineConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class SteamTurbineConfig : IBuildingConfig
{
  public const string ID = "SteamTurbine";
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
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("SteamTurbine", 5, 4, "steamturbine_kanim", 30, 60f, construction_mass, construction_materials, 1600f, BuildLocationRule.Anywhere, none2, noise, 1f);
    buildingDef.GeneratorWattageRating = 2000f;
    buildingDef.GeneratorBaseCapacity = 2000f;
    buildingDef.Entombable = true;
    buildingDef.IsFoundation = false;
    buildingDef.PermittedRotations = PermittedRotations.FlipH;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.PowerOutputOffset = new CellOffset(1, 0);
    buildingDef.OverheatTemperature = 1273.15f;
    buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
    buildingDef.Deprecated = true;
    return buildingDef;
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    base.DoPostConfigureUnderConstruction(go);
    go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.CanPowerTinker.Id;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<Storage>().SetDefaultStoredItemModifiers(SteamTurbineConfig.StoredItemModifiers);
    Turbine turbine = go.AddOrGet<Turbine>();
    turbine.srcElem = SimHashes.Steam;
    turbine.pumpKGRate = 10f;
    turbine.requiredMassFlowDifferential = 3f;
    turbine.minEmitMass = 10f;
    turbine.maxRPM = 4000f;
    turbine.rpmAcceleration = turbine.maxRPM / 30f;
    turbine.rpmDeceleration = turbine.maxRPM / 20f;
    turbine.minGenerationRPM = 3000f;
    turbine.minActiveTemperature = 500f;
    turbine.emitTemperature = 425f;
    go.AddOrGet<Generator>();
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
    });
  }
}
