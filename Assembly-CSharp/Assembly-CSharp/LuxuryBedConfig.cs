// Decompiled with JetBrains decompiler
// Type: LuxuryBedConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class LuxuryBedConfig : IBuildingConfig
{
  public static string ID = "LuxuryBed";

  public override BuildingDef CreateBuildingDef()
  {
    string id = LuxuryBedConfig.ID;
    float[] tieR3 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] plastics = MATERIALS.PLASTICS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR2 = BUILDINGS.DECOR.BONUS.TIER2;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, 4, 2, "elegantbed_kanim", 10, 10f, tieR3, plastics, 1600f, BuildLocationRule.OnFloor, tieR2, noise);
    buildingDef.Overheatable = false;
    buildingDef.AudioCategory = "Metal";
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.Bed);
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.LuxuryBed);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.GetComponent<KAnimControllerBase>().initialAnim = "off";
    Bed bed = go.AddOrGet<Bed>();
    bed.effects = new string[2]
    {
      "LuxuryBedStamina",
      "BedHealth"
    };
    bed.workLayer = Grid.SceneLayer.BuildingFront;
    Sleepable sleepable = go.AddOrGet<Sleepable>();
    sleepable.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_sleep_bed_kanim")
    };
    sleepable.workLayer = Grid.SceneLayer.BuildingFront;
    go.AddOrGet<Ownable>().slotID = Db.Get().AssignableSlots.Bed.Id;
  }
}
