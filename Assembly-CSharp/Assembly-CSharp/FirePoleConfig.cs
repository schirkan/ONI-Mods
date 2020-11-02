// Decompiled with JetBrains decompiler
// Type: FirePoleConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class FirePoleConfig : IBuildingConfig
{
  public const string ID = "FirePole";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR2 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] allMetals = MATERIALS.ALL_METALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR0 = BUILDINGS.DECOR.PENALTY.TIER0;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("FirePole", 1, 1, "firepole_kanim", 10, 10f, tieR2, allMetals, 1600f, BuildLocationRule.Anywhere, tieR0, noise);
    BuildingTemplates.CreateLadderDef(buildingDef);
    buildingDef.Floodable = false;
    buildingDef.Overheatable = false;
    buildingDef.Entombable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "small";
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.DragBuild = true;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
    Ladder ladder = go.AddOrGet<Ladder>();
    ladder.isPole = true;
    ladder.upwardsMovementSpeedMultiplier = 0.25f;
    ladder.downwardsMovementSpeedMultiplier = 4f;
    go.AddOrGet<AnimTileable>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
