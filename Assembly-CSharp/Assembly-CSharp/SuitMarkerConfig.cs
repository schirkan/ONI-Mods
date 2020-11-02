// Decompiled with JetBrains decompiler
// Type: SuitMarkerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class SuitMarkerConfig : IBuildingConfig
{
  public const string ID = "SuitMarker";

  public override BuildingDef CreateBuildingDef()
  {
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    float[] construction_mass = new float[2]
    {
      BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0],
      BUILDINGS.CONSTRUCTION_MASS_KG.TIER1[0]
    };
    string[] construction_materials = refinedMetals;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR1 = BUILDINGS.DECOR.BONUS.TIER1;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("SuitMarker", 1, 3, "changingarea_arrow_kanim", 30, 30f, construction_mass, construction_materials, 1600f, BuildLocationRule.OnFloor, tieR1, noise);
    buildingDef.PermittedRotations = PermittedRotations.FlipH;
    buildingDef.PreventIdleTraversalPastBuilding = true;
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "SuitMarker");
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    SuitMarker suitMarker = go.AddOrGet<SuitMarker>();
    suitMarker.LockerTags = new Tag[1]
    {
      new Tag("SuitLocker")
    };
    suitMarker.PathFlag = PathFinder.PotentialPath.Flags.HasAtmoSuit;
    go.AddOrGet<AnimTileable>().tags = new Tag[2]
    {
      new Tag("SuitMarker"),
      new Tag("SuitLocker")
    };
    go.AddTag(GameTags.JetSuitBlocker);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
