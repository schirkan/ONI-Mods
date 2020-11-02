﻿// Decompiled with JetBrains decompiler
// Type: IceCooledFanConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class IceCooledFanConfig : IBuildingConfig
{
  public const string ID = "IceCooledFan";
  private float COOLING_RATE = 32f;
  private float TARGET_TEMPERATURE = 278.15f;
  private float ICE_CAPACITY = 50f;
  private static readonly CellOffset[] overrideOffsets = new CellOffset[4]
  {
    new CellOffset(-2, 1),
    new CellOffset(2, 1),
    new CellOffset(-1, 0),
    new CellOffset(1, 0)
  };

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] allMetals = MATERIALS.ALL_METALS;
    EffectorValues tieR2 = NOISE_POLLUTION.NOISY.TIER2;
    EffectorValues none = BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("IceCooledFan", 2, 2, "fanice_kanim", 30, 30f, tieR4, allMetals, 1600f, BuildLocationRule.OnFloor, none, noise);
    buildingDef.SelfHeatKilowattsWhenActive = (float) (-(double) this.COOLING_RATE * 0.25);
    buildingDef.ExhaustKilowattsWhenActive = (float) (-(double) this.COOLING_RATE * 0.75);
    buildingDef.Overheatable = false;
    buildingDef.ViewMode = OverlayModes.Temperature.ID;
    buildingDef.AudioCategory = "Metal";
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    Storage storage1 = go.AddComponent<Storage>();
    storage1.capacityKg = 50f;
    storage1.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
    Storage storage2 = go.AddComponent<Storage>();
    storage2.capacityKg = 50f;
    storage2.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    go.AddOrGet<MinimumOperatingTemperature>().minimumTemperature = 273.15f;
    go.AddOrGet<LoopingSounds>();
    Prioritizable.AddRef(go);
    IceCooledFan iceCooledFan = go.AddOrGet<IceCooledFan>();
    iceCooledFan.coolingRate = this.COOLING_RATE;
    iceCooledFan.targetTemperature = this.TARGET_TEMPERATURE;
    iceCooledFan.iceStorage = storage1;
    iceCooledFan.liquidStorage = storage2;
    iceCooledFan.minCooledTemperature = 278.15f;
    iceCooledFan.minEnvironmentMass = 0.25f;
    iceCooledFan.minCoolingRange = new Vector2I(-2, 0);
    iceCooledFan.maxCoolingRange = new Vector2I(2, 4);
    iceCooledFan.consumptionTag = GameTags.IceOre;
    ManualDeliveryKG manualDeliveryKg = go.AddComponent<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage1);
    manualDeliveryKg.requestedItemTag = GameTags.IceOre;
    manualDeliveryKg.capacity = this.ICE_CAPACITY;
    manualDeliveryKg.refillMass = this.ICE_CAPACITY * 0.2f;
    manualDeliveryKg.minimumMass = 10f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
    go.AddOrGet<IceCooledFanWorkable>().overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_icefan_kanim")
    };
  }

  public override void DoPostConfigureComplete(GameObject go) => go.GetComponent<KPrefabID>().prefabSpawnFn += (KPrefabID.PrefabFn) (game_object =>
  {
    HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(game_object);
    StructureTemperaturePayload payload = GameComps.StructureTemperatures.GetPayload(handle);
    int cell = Grid.PosToCell(game_object);
    payload.OverrideExtents(new Extents(cell, IceCooledFanConfig.overrideOffsets));
    GameComps.StructureTemperatures.SetPayload(handle, ref payload);
  });
}
