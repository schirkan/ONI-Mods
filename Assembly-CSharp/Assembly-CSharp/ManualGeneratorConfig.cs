// Decompiled with JetBrains decompiler
// Type: ManualGeneratorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class ManualGeneratorConfig : IBuildingConfig
{
  public const string ID = "ManualGenerator";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR3_1 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] allMetals = MATERIALS.ALL_METALS;
    EffectorValues tieR3_2 = NOISE_POLLUTION.NOISY.TIER3;
    EffectorValues none = BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR3_2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("ManualGenerator", 2, 2, "generatormanual_kanim", 30, 30f, tieR3_1, allMetals, 1600f, BuildLocationRule.OnFloor, none, noise);
    buildingDef.GeneratorWattageRating = 400f;
    buildingDef.GeneratorBaseCapacity = 10000f;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.Breakable = true;
    buildingDef.ForegroundLayer = Grid.SceneLayer.BuildingFront;
    buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
    buildingDef.SelfHeatKilowattsWhenActive = 1f;
    return buildingDef;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<LogicOperationalController>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    go.AddOrGet<LoopingSounds>();
    Prioritizable.AddRef(go);
    go.AddOrGet<Generator>().powerDistributionOrder = 10;
    ManualGenerator manualGenerator = go.AddOrGet<ManualGenerator>();
    manualGenerator.SetSliderValue(50f, 0);
    manualGenerator.workLayer = Grid.SceneLayer.BuildingFront;
    KBatchedAnimController kbatchedAnimController = go.AddOrGet<KBatchedAnimController>();
    kbatchedAnimController.fgLayer = Grid.SceneLayer.BuildingFront;
    kbatchedAnimController.initialAnim = "off";
    Tinkerable.MakePowerTinkerable(go);
  }
}
