// Decompiled with JetBrains decompiler
// Type: IceSculptureConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class IceSculptureConfig : IBuildingConfig
{
  public const string ID = "IceSculpture";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] construction_materials = new string[1]{ "Ice" };
    EffectorValues none = TUNING.NOISE_POLLUTION.NONE;
    EffectorValues decor = new EffectorValues()
    {
      amount = 20,
      radius = 8
    };
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("IceSculpture", 2, 2, "icesculpture_kanim", 10, 120f, tieR4, construction_materials, 273.15f, BuildLocationRule.OnFloor, decor, noise);
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
    sculpture.stages.Add(new Artable.Stage("Default", (string) STRINGS.BUILDINGS.PREFABS.ICESCULPTURE.NAME, "slab", 0, false, Artable.Status.Ready));
    sculpture.stages.Add(new Artable.Stage("Bad", (string) STRINGS.BUILDINGS.PREFABS.ICESCULPTURE.POORQUALITYNAME, "crap", 5, false, Artable.Status.Ugly));
    sculpture.stages.Add(new Artable.Stage("Average", (string) STRINGS.BUILDINGS.PREFABS.ICESCULPTURE.AVERAGEQUALITYNAME, "idle", 10, true, Artable.Status.Okay));
  }
}
