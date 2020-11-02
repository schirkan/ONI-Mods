// Decompiled with JetBrains decompiler
// Type: CreatureFeederConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class CreatureFeederConfig : IBuildingConfig
{
  public const string ID = "CreatureFeeder";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR3 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] rawMetals = MATERIALS.RAW_METALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR2 = BUILDINGS.DECOR.PENALTY.TIER2;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("CreatureFeeder", 1, 2, "feeder_kanim", 100, 120f, tieR3, rawMetals, 1600f, BuildLocationRule.OnFloor, tieR2, noise);
    buildingDef.AudioCategory = "Metal";
    return buildingDef;
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    Prioritizable.AddRef(go);
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.CreatureFeeder);
    Storage storage = go.AddOrGet<Storage>();
    storage.capacityKg = 2000f;
    storage.showInUI = true;
    storage.showDescriptor = true;
    storage.allowItemRemoval = false;
    storage.allowSettingOnlyFetchMarkedItems = false;
    go.AddOrGet<StorageLocker>().choreTypeID = Db.Get().ChoreTypes.RanchingFetch.Id;
    go.AddOrGet<UserNameable>();
    go.AddOrGet<TreeFilterable>();
    go.AddOrGet<CreatureFeeder>();
  }

  public override void DoPostConfigureComplete(GameObject go) => go.AddOrGetDef<StorageController.Def>();

  public override void ConfigurePost(BuildingDef def)
  {
    List<Tag> tagList = new List<Tag>();
    Tag[] target_species = new Tag[4]
    {
      GameTags.Creatures.Species.LightBugSpecies,
      GameTags.Creatures.Species.HatchSpecies,
      GameTags.Creatures.Species.MoleSpecies,
      GameTags.Creatures.Species.CrabSpecies
    };
    foreach (KeyValuePair<Tag, Diet> collectDiet in DietManager.CollectDiets(target_species))
      tagList.Add(collectDiet.Key);
    def.BuildingComplete.GetComponent<Storage>().storageFilters = tagList;
  }
}
