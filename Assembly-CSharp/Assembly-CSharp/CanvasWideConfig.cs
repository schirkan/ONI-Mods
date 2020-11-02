// Decompiled with JetBrains decompiler
// Type: CanvasWideConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class CanvasWideConfig : IBuildingConfig
{
  public const string ID = "CanvasWide";

  public override BuildingDef CreateBuildingDef()
  {
    float[] construction_mass = new float[2]{ 400f, 1f };
    string[] construction_materials = new string[2]
    {
      "Metal",
      "BuildingFiber"
    };
    EffectorValues none = TUNING.NOISE_POLLUTION.NONE;
    EffectorValues decor = new EffectorValues()
    {
      amount = 15,
      radius = 6
    };
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("CanvasWide", 3, 2, "painting_wide_kanim", 30, 120f, construction_mass, construction_materials, 1600f, BuildLocationRule.Anywhere, decor, noise);
    buildingDef.Floodable = false;
    buildingDef.SceneLayer = Grid.SceneLayer.InteriorWall;
    buildingDef.Overheatable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.ViewMode = OverlayModes.Decor.ID;
    buildingDef.DefaultAnimState = "off";
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
    Painting painting = go.AddComponent<Painting>();
    painting.stages.Add(new Artable.Stage("Default", (string) STRINGS.BUILDINGS.PREFABS.CANVAS.NAME, "off", 0, false, Artable.Status.Ready));
    painting.stages.Add(new Artable.Stage("Bad", (string) STRINGS.BUILDINGS.PREFABS.CANVAS.POORQUALITYNAME, "art_a", 5, false, Artable.Status.Ugly));
    painting.stages.Add(new Artable.Stage("Average", (string) STRINGS.BUILDINGS.PREFABS.CANVAS.AVERAGEQUALITYNAME, "art_b", 10, false, Artable.Status.Okay));
    painting.stages.Add(new Artable.Stage("Good", (string) STRINGS.BUILDINGS.PREFABS.CANVAS.EXCELLENTQUALITYNAME, "art_c", 15, true, Artable.Status.Great));
    painting.stages.Add(new Artable.Stage("Good2", (string) STRINGS.BUILDINGS.PREFABS.CANVAS.EXCELLENTQUALITYNAME, "art_d", 15, true, Artable.Status.Great));
    painting.stages.Add(new Artable.Stage("Good3", (string) STRINGS.BUILDINGS.PREFABS.CANVAS.EXCELLENTQUALITYNAME, "art_e", 15, true, Artable.Status.Great));
    painting.stages.Add(new Artable.Stage("Good4", (string) STRINGS.BUILDINGS.PREFABS.CANVAS.EXCELLENTQUALITYNAME, "art_f", 15, true, Artable.Status.Great));
  }
}
