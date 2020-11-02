// Decompiled with JetBrains decompiler
// Type: MoleConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

public class MoleConfig : IEntityConfig
{
  public const string ID = "Mole";
  public const string BASE_TRAIT_ID = "MoleBaseTrait";
  public const string EGG_ID = "MoleEgg";
  private static float MIN_POOP_SIZE_IN_CALORIES = 2400000f;
  private static float CALORIES_PER_KG_OF_DIRT = 1000f;
  public static int EGG_SORT_ORDER = 800;

  public static GameObject CreateMole(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby = false)
  {
    GameObject gameObject = BaseMoleConfig.BaseMole(id, name, (string) STRINGS.CREATURES.SPECIES.MOLE.DESC, "MoleBaseTrait", anim_file, is_baby);
    gameObject.AddTag(GameTags.Creatures.Digger);
    EntityTemplates.ExtendEntityToWildCreature(gameObject, MoleTuning.PEN_SIZE_PER_CREATURE, 100f);
    Trait trait = Db.Get().CreateTrait("MoleBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, MoleTuning.STANDARD_STOMACH_SIZE, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) MoleTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name));
    Diet diet = new Diet(BaseMoleConfig.SimpleOreDiet(new List<Tag>()
    {
      SimHashes.Regolith.CreateTag(),
      SimHashes.Dirt.CreateTag(),
      SimHashes.IronOre.CreateTag()
    }, MoleConfig.CALORIES_PER_KG_OF_DIRT, TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL).ToArray());
    CreatureCalorieMonitor.Def def = gameObject.AddOrGetDef<CreatureCalorieMonitor.Def>();
    def.diet = diet;
    def.minPoopSizeInCalories = MoleConfig.MIN_POOP_SIZE_IN_CALORIES;
    gameObject.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
    gameObject.AddOrGetDef<OvercrowdingMonitor.Def>().spaceRequiredPerCreature = 0;
    gameObject.AddOrGet<LoopingSounds>();
    return gameObject;
  }

  public GameObject CreatePrefab()
  {
    GameObject mole = MoleConfig.CreateMole("Mole", (string) STRINGS.CREATURES.SPECIES.MOLE.NAME, (string) STRINGS.CREATURES.SPECIES.MOLE.DESC, "driller_kanim");
    string eggName = (string) STRINGS.CREATURES.SPECIES.MOLE.EGG_NAME;
    string desc = (string) STRINGS.CREATURES.SPECIES.MOLE.DESC;
    double eggMass = (double) MoleTuning.EGG_MASS;
    int eggSortOrder1 = MoleConfig.EGG_SORT_ORDER;
    List<FertilityMonitor.BreedingChance> eggChancesBase = MoleTuning.EGG_CHANCES_BASE;
    int eggSortOrder2 = eggSortOrder1;
    return EntityTemplates.ExtendEntityToFertileCreature(mole, "MoleEgg", eggName, desc, "egg_driller_kanim", (float) eggMass, "MoleBaby", 60f, 20f, eggChancesBase, eggSortOrder2);
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst) => MoleConfig.SetSpawnNavType(inst);

  public static void SetSpawnNavType(GameObject inst)
  {
    int cell = Grid.PosToCell(inst);
    Navigator component = inst.GetComponent<Navigator>();
    if (!((Object) component != (Object) null))
      return;
    if (Grid.IsSolidCell(cell))
    {
      component.SetCurrentNavType(NavType.Solid);
      inst.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.FXFront));
      inst.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.FXFront);
    }
    else
      inst.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Creatures);
  }
}
