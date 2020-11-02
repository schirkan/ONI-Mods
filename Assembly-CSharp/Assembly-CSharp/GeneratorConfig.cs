// Decompiled with JetBrains decompiler
// Type: GeneratorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class GeneratorConfig : IBuildingConfig
{
  public const string ID = "Generator";
  private const float COAL_BURN_RATE = 1f;
  private const float COAL_CAPACITY = 600f;
  public const float CO2_OUTPUT_TEMPERATURE = 383.15f;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR5_1 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
    string[] allMetals = MATERIALS.ALL_METALS;
    EffectorValues tieR5_2 = NOISE_POLLUTION.NOISY.TIER5;
    EffectorValues tieR2 = BUILDINGS.DECOR.PENALTY.TIER2;
    EffectorValues noise = tieR5_2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("Generator", 3, 3, "generatorphos_kanim", 100, 120f, tieR5_1, allMetals, 2400f, BuildLocationRule.OnFloor, tieR2, noise);
    buildingDef.GeneratorWattageRating = 600f;
    buildingDef.GeneratorBaseCapacity = 20000f;
    buildingDef.ExhaustKilowattsWhenActive = 8f;
    buildingDef.SelfHeatKilowattsWhenActive = 1f;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.AudioSize = "large";
    buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    EnergyGenerator energyGenerator = go.AddOrGet<EnergyGenerator>();
    energyGenerator.formula = EnergyGenerator.CreateSimpleFormula(SimHashes.Carbon.CreateTag(), 1f, 600f, SimHashes.CarbonDioxide, 0.02f, false, new CellOffset(1, 2), 383.15f);
    energyGenerator.meterOffset = Meter.Offset.Behind;
    energyGenerator.SetSliderValue(50f, 0);
    energyGenerator.powerDistributionOrder = 9;
    Storage storage = go.AddOrGet<Storage>();
    storage.capacityKg = 600f;
    go.AddOrGet<LoopingSounds>();
    Prioritizable.AddRef(go);
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.requestedItemTag = new Tag("Coal");
    manualDeliveryKg.capacity = storage.capacityKg;
    manualDeliveryKg.refillMass = 100f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.PowerFetch.IdHash;
    Tinkerable.MakePowerTinkerable(go);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<LogicOperationalController>();
    go.AddOrGetDef<PoweredActiveController.Def>();
  }
}
