// Decompiled with JetBrains decompiler
// Type: LiquidMiniPumpConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class LiquidMiniPumpConfig : IBuildingConfig
{
  public const string ID = "LiquidMiniPump";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR2 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] plastics = MATERIALS.PLASTICS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR1 = BUILDINGS.DECOR.PENALTY.TIER1;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("LiquidMiniPump", 1, 2, "miniwaterpump_kanim", 100, 60f, tieR2, plastics, 1600f, BuildLocationRule.Anywhere, tieR1, noise);
    buildingDef.PermittedRotations = PermittedRotations.R360;
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 60f;
    buildingDef.ExhaustKilowattsWhenActive = 0.0f;
    buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
    buildingDef.OutputConduitType = ConduitType.Liquid;
    buildingDef.Floodable = false;
    buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.PowerInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 1);
    buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 1));
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, "LiquidMiniPump");
    return buildingDef;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<LogicOperationalController>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    go.AddOrGet<LoopingSounds>();
    go.AddOrGet<EnergyConsumer>();
    go.AddOrGet<Pump>();
    go.AddOrGet<Storage>().capacityKg = 2f;
    ElementConsumer elementConsumer = go.AddOrGet<ElementConsumer>();
    elementConsumer.configuration = ElementConsumer.Configuration.AllLiquid;
    elementConsumer.consumptionRate = 1f;
    elementConsumer.storeOnConsume = true;
    elementConsumer.showInStatusPanel = false;
    elementConsumer.consumptionRadius = (byte) 2;
    ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
    conduitDispenser.conduitType = ConduitType.Liquid;
    conduitDispenser.alwaysDispense = true;
    conduitDispenser.elementFilter = (SimHashes[]) null;
    go.AddOrGetDef<OperationalController.Def>();
    go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits);
  }
}
