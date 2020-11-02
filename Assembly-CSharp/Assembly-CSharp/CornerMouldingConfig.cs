// Decompiled with JetBrains decompiler
// Type: CornerMouldingConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class CornerMouldingConfig : IBuildingConfig
{
  public const string ID = "CornerMoulding";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR2 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] rawMinerals = MATERIALS.RAW_MINERALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues decor = new EffectorValues()
    {
      amount = 5,
      radius = 3
    };
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("CornerMoulding", 1, 1, "corner_tile_kanim", 10, 30f, tieR2, rawMinerals, 800f, BuildLocationRule.InCorner, decor, noise);
    buildingDef.DefaultAnimState = "corner";
    buildingDef.Floodable = false;
    buildingDef.Overheatable = false;
    buildingDef.ViewMode = OverlayModes.Decor.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "small";
    buildingDef.PermittedRotations = PermittedRotations.FlipH;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) => go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration);

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
