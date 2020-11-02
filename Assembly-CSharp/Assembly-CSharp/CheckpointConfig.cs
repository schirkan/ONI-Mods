// Decompiled with JetBrains decompiler
// Type: CheckpointConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class CheckpointConfig : IBuildingConfig
{
  public const string ID = "Checkpoint";

  public override BuildingDef CreateBuildingDef()
  {
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    float[] tieR2 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] construction_materials = refinedMetals;
    EffectorValues tieR0 = TUNING.NOISE_POLLUTION.NOISY.TIER0;
    EffectorValues tieR1 = TUNING.BUILDINGS.DECOR.BONUS.TIER1;
    EffectorValues noise = tieR0;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("Checkpoint", 1, 3, "checkpoint_kanim", 30, 30f, tieR2, construction_materials, 1600f, BuildLocationRule.OnFloor, tieR1, noise);
    buildingDef.ForegroundLayer = Grid.SceneLayer.Front;
    buildingDef.PermittedRotations = PermittedRotations.FlipH;
    buildingDef.PreventIdleTraversalPastBuilding = true;
    buildingDef.Floodable = false;
    buildingDef.RequiresPowerInput = true;
    buildingDef.PowerInputOffset = new CellOffset(0, 2);
    buildingDef.EnergyConsumptionWhenActive = 10f;
    buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
    buildingDef.LogicInputPorts = new List<LogicPorts.Port>()
    {
      LogicPorts.Port.InputPort(Checkpoint.PORT_ID, new CellOffset(0, 2), (string) STRINGS.BUILDINGS.PREFABS.CHECKPOINT.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.CHECKPOINT.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.CHECKPOINT.LOGIC_PORT_INACTIVE, true)
    };
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) => go.AddOrGet<Checkpoint>();

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
