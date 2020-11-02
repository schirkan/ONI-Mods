﻿// Decompiled with JetBrains decompiler
// Type: LiquidLogicValveConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class LiquidLogicValveConfig : IBuildingConfig
{
  public const string ID = "LiquidLogicValve";
  private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR1_1 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    EffectorValues tieR1_2 = TUNING.NOISE_POLLUTION.NOISY.TIER1;
    EffectorValues tieR0 = TUNING.BUILDINGS.DECOR.PENALTY.TIER0;
    EffectorValues noise = tieR1_2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("LiquidLogicValve", 1, 2, "valveliquid_logic_kanim", 30, 10f, tieR1_1, refinedMetals, 1600f, BuildLocationRule.Anywhere, tieR0, noise);
    buildingDef.InputConduitType = ConduitType.Liquid;
    buildingDef.OutputConduitType = ConduitType.Liquid;
    buildingDef.Floodable = false;
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 10f;
    buildingDef.PowerInputOffset = new CellOffset(0, 1);
    buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.PermittedRotations = PermittedRotations.R360;
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 1);
    buildingDef.LogicInputPorts = new List<LogicPorts.Port>()
    {
      LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, new CellOffset(0, 0), (string) STRINGS.BUILDINGS.PREFABS.LIQUIDLOGICVALVE.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.LIQUIDLOGICVALVE.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.LIQUIDLOGICVALVE.LOGIC_PORT_INACTIVE, true)
    };
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, "LiquidLogicValve");
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    Object.DestroyImmediate((Object) go.GetComponent<BuildingEnabledButton>());
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    OperationalValve operationalValve = go.AddOrGet<OperationalValve>();
    operationalValve.conduitType = ConduitType.Liquid;
    operationalValve.maxFlow = 10f;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    Object.DestroyImmediate((Object) go.GetComponent<ConduitConsumer>());
    Object.DestroyImmediate((Object) go.GetComponent<ConduitDispenser>());
    go.GetComponent<RequireInputs>().SetRequirements(true, false);
    go.AddOrGet<LogicOperationalController>().unNetworkedValue = 0;
    go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits);
  }
}