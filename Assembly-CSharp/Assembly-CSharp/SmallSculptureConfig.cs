// Decompiled with JetBrains decompiler
// Type: SmallSculptureConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class SmallSculptureConfig : IBuildingConfig
{
  public const string ID = "SmallSculpture";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR3 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] rawMinerals = MATERIALS.RAW_MINERALS;
    EffectorValues none = TUNING.NOISE_POLLUTION.NONE;
    EffectorValues decor = new EffectorValues()
    {
      amount = 5,
      radius = 4
    };
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("SmallSculpture", 1, 2, "sculpture_1x2_kanim", 10, 60f, tieR3, rawMinerals, 1600f, BuildLocationRule.OnFloor, decor, noise);
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
    sculpture.stages.Add(new Artable.Stage("Default", (string) STRINGS.BUILDINGS.PREFABS.SMALLSCULPTURE.NAME, "slab", 0, false, Artable.Status.Ready));
    sculpture.stages.Add(new Artable.Stage("Bad", (string) STRINGS.BUILDINGS.PREFABS.SMALLSCULPTURE.POORQUALITYNAME, "crap_1", 5, false, Artable.Status.Ugly));
    sculpture.stages.Add(new Artable.Stage("Average", (string) STRINGS.BUILDINGS.PREFABS.SMALLSCULPTURE.AVERAGEQUALITYNAME, "good_1", 10, false, Artable.Status.Okay));
    sculpture.stages.Add(new Artable.Stage("Good", (string) STRINGS.BUILDINGS.PREFABS.SMALLSCULPTURE.EXCELLENTQUALITYNAME, "amazing_1", 15, true, Artable.Status.Great));
    sculpture.stages.Add(new Artable.Stage("Good2", (string) STRINGS.BUILDINGS.PREFABS.SMALLSCULPTURE.EXCELLENTQUALITYNAME, "amazing_2", 15, true, Artable.Status.Great));
    sculpture.stages.Add(new Artable.Stage("Good3", (string) STRINGS.BUILDINGS.PREFABS.SMALLSCULPTURE.EXCELLENTQUALITYNAME, "amazing_3", 15, true, Artable.Status.Great));
  }
}
