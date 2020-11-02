// Decompiled with JetBrains decompiler
// Type: MassiveHeatSinkConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class MassiveHeatSinkConfig : IBuildingConfig
{
  public const string ID = "MassiveHeatSink";
  private const float CONSUMPTION_RATE = 0.01f;
  private const float STORAGE_CAPACITY = 0.09999999f;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR5_1 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
    string[] rawMetals = MATERIALS.RAW_METALS;
    EffectorValues tieR5_2 = NOISE_POLLUTION.NOISY.TIER5;
    EffectorValues tieR2 = BUILDINGS.DECOR.BONUS.TIER2;
    EffectorValues noise = tieR5_2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("MassiveHeatSink", 4, 4, "massiveheatsink_kanim", 100, 120f, tieR5_1, rawMetals, 2400f, BuildLocationRule.OnFloor, tieR2, noise);
    buildingDef.ExhaustKilowattsWhenActive = -16f;
    buildingDef.SelfHeatKilowattsWhenActive = -64f;
    buildingDef.Floodable = true;
    buildingDef.Entombable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.InputConduitType = ConduitType.Gas;
    return buildingDef;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    go.AddOrGet<MassiveHeatSink>();
    go.AddOrGet<MinimumOperatingTemperature>().minimumTemperature = 100f;
    PrimaryElement component = go.GetComponent<PrimaryElement>();
    component.SetElement(SimHashes.Iron);
    component.Temperature = 294.15f;
    go.AddOrGet<LoopingSounds>();
    go.AddOrGet<Storage>().capacityKg = 0.09999999f;
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Gas;
    conduitConsumer.consumptionRate = 1f;
    conduitConsumer.capacityTag = GameTagExtensions.Create(SimHashes.Hydrogen);
    conduitConsumer.capacityKG = 0.09999999f;
    conduitConsumer.forceAlwaysSatisfied = true;
    conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
    go.AddOrGet<ElementConverter>().consumedElements = new ElementConverter.ConsumedElement[1]
    {
      new ElementConverter.ConsumedElement(ElementLoader.FindElementByHash(SimHashes.Hydrogen).tag, 0.01f)
    };
    go.AddOrGetDef<PoweredActiveController.Def>();
  }
}
