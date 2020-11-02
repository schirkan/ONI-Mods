// Decompiled with JetBrains decompiler
// Type: HatchConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

[EntityConfigOrder(1)]
public class HatchConfig : IEntityConfig
{
  public const string ID = "Hatch";
  public const string BASE_TRAIT_ID = "HatchBaseTrait";
  public const string EGG_ID = "HatchEgg";
  private const SimHashes EMIT_ELEMENT = SimHashes.Carbon;
  private static float KG_ORE_EATEN_PER_CYCLE = 140f;
  private static float CALORIES_PER_KG_OF_ORE = HatchTuning.STANDARD_CALORIES_PER_CYCLE / HatchConfig.KG_ORE_EATEN_PER_CYCLE;
  private static float MIN_POOP_SIZE_IN_KG = 25f;
  public static int EGG_SORT_ORDER = 0;

  public static GameObject CreateHatch(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject wildCreature = EntityTemplates.ExtendEntityToWildCreature(BaseHatchConfig.BaseHatch(id, name, desc, anim_file, "HatchBaseTrait", is_baby), HatchTuning.PEN_SIZE_PER_CREATURE, 100f);
    Trait trait = Db.Get().CreateTrait("HatchBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, HatchTuning.STANDARD_STOMACH_SIZE, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) HatchTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name));
    List<Diet.Info> infoList = BaseHatchConfig.BasicRockDiet(SimHashes.Carbon.CreateTag(), HatchConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL, (string) null, 0.0f);
    infoList.AddRange((IEnumerable<Diet.Info>) BaseHatchConfig.FoodDiet(SimHashes.Carbon.CreateTag(), HatchConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_1, (string) null, 0.0f));
    List<Diet.Info> diet_infos = infoList;
    double caloriesPerKgOfOre = (double) HatchConfig.CALORIES_PER_KG_OF_ORE;
    double minPoopSizeInKg = (double) HatchConfig.MIN_POOP_SIZE_IN_KG;
    return BaseHatchConfig.SetupDiet(wildCreature, diet_infos, (float) caloriesPerKgOfOre, (float) minPoopSizeInKg);
  }

  public GameObject CreatePrefab() => EntityTemplates.ExtendEntityToFertileCreature(HatchConfig.CreateHatch("Hatch", (string) STRINGS.CREATURES.SPECIES.HATCH.NAME, (string) STRINGS.CREATURES.SPECIES.HATCH.DESC, "hatch_kanim", false), "HatchEgg", (string) STRINGS.CREATURES.SPECIES.HATCH.EGG_NAME, (string) STRINGS.CREATURES.SPECIES.HATCH.DESC, "egg_hatch_kanim", HatchTuning.EGG_MASS, "HatchBaby", 60f, 20f, HatchTuning.EGG_CHANCES_BASE, HatchConfig.EGG_SORT_ORDER);

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
