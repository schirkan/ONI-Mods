// Decompiled with JetBrains decompiler
// Type: MarbleSculptureConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class MarbleSculptureConfig : IBuildingConfig
{
  public const string ID = "MarbleSculpture";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] preciousRocks = MATERIALS.PRECIOUS_ROCKS;
    EffectorValues none = TUNING.NOISE_POLLUTION.NONE;
    EffectorValues decor = new EffectorValues()
    {
      amount = 20,
      radius = 8
    };
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("MarbleSculpture", 2, 3, "sculpture_marble_kanim", 10, 120f, tieR4, preciousRocks, 1600f, BuildLocationRule.OnFloor, decor, noise);
    buildingDef.Floodable = false;
    buildingDef.Overheatable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.ViewMode = OverlayModes.Decor.ID;
    buildingDef.DefaultAnimState = "slab";
    buildingDef.PermittedRotations = PermittedRotations.FlipH;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<BuildingComplete>().isArtable = true;
    go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    Sculpture sculpture = go.AddComponent<Sculpture>();
    sculpture.stages.Add(new Artable.Stage("Default", (string) STRINGS.BUILDINGS.PREFABS.MARBLESCULPTURE.NAME, "slab", 0, false, Artable.Status.Ready));
    sculpture.stages.Add(new Artable.Stage("Bad", (string) STRINGS.BUILDINGS.PREFABS.MARBLESCULPTURE.POORQUALITYNAME, "crap_1", 5, false, Artable.Status.Ugly));
    sculpture.stages.Add(new Artable.Stage("Average", (string) STRINGS.BUILDINGS.PREFABS.MARBLESCULPTURE.AVERAGEQUALITYNAME, "good_1", 10, false, Artable.Status.Okay));
    sculpture.stages.Add(new Artable.Stage("Good1", (string) STRINGS.BUILDINGS.PREFABS.MARBLESCULPTURE.EXCELLENTQUALITYNAME, "amazing_1", 15, true, Artable.Status.Great));
    sculpture.stages.Add(new Artable.Stage("Good2", (string) STRINGS.BUILDINGS.PREFABS.MARBLESCULPTURE.EXCELLENTQUALITYNAME, "amazing_2", 15, true, Artable.Status.Great));
    sculpture.stages.Add(new Artable.Stage("Good3", (string) STRINGS.BUILDINGS.PREFABS.MARBLESCULPTURE.EXCELLENTQUALITYNAME, "amazing_3", 15, true, Artable.Status.Great));
  }
}
