// Decompiled with JetBrains decompiler
// Type: LadderFastConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class LadderFastConfig : IBuildingConfig
{
  public const string ID = "LadderFast";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR1 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
    string[] plastics = MATERIALS.PLASTICS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR0 = BUILDINGS.DECOR.PENALTY.TIER0;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("LadderFast", 1, 1, "ladder_plastic_kanim", 10, 10f, tieR1, plastics, 1600f, BuildLocationRule.Anywhere, tieR0, noise);
    BuildingTemplates.CreateLadderDef(buildingDef);
    buildingDef.Floodable = false;
    buildingDef.Overheatable = false;
    buildingDef.Entombable = false;
    buildingDef.AudioCategory = "Plastic";
    buildingDef.AudioSize = "small";
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.DragBuild = true;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
    Ladder ladder = go.AddOrGet<Ladder>();
    ladder.upwardsMovementSpeedMultiplier = 1.2f;
    ladder.downwardsMovementSpeedMultiplier = 1.2f;
    go.AddOrGet<AnimTileable>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
