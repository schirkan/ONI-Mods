// Decompiled with JetBrains decompiler
// Type: LightBugPinkConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class LightBugPinkConfig : IEntityConfig
{
  public const string ID = "LightBugPink";
  public const string BASE_TRAIT_ID = "LightBugPinkBaseTrait";
  public const string EGG_ID = "LightBugPinkEgg";
  private static float KG_ORE_EATEN_PER_CYCLE = 1f;
  private static float CALORIES_PER_KG_OF_ORE = LightBugTuning.STANDARD_CALORIES_PER_CYCLE / LightBugPinkConfig.KG_ORE_EATEN_PER_CYCLE;
  public static int EGG_SORT_ORDER = LightBugConfig.EGG_SORT_ORDER + 3;

  public static GameObject CreateLightBug(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject prefab = BaseLightBugConfig.BaseLightBug(id, name, desc, anim_file, "LightBugPinkBaseTrait", LIGHT2D.LIGHTBUG_COLOR_PINK, TUNING.DECOR.BONUS.TIER6, is_baby, "pnk_");
    EntityTemplates.ExtendEntityToWildCreature(prefab, LightBugTuning.PEN_SIZE_PER_CREATURE, 25f);
    Trait trait = Db.Get().CreateTrait("LightBugPinkBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, LightBugTuning.STANDARD_STOMACH_SIZE, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) LightBugTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 5f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 25f, name));
    return BaseLightBugConfig.SetupDiet(prefab, new HashSet<Tag>()
    {
      TagManager.Create("FriedMushroom"),
      TagManager.Create("SpiceBread"),
      TagManager.Create(PrickleFruitConfig.ID),
      TagManager.Create("GrilledPrickleFruit"),
      TagManager.Create("Salsa"),
      SimHashes.Phosphorite.CreateTag()
    }, Tag.Invalid, LightBugPinkConfig.CALORIES_PER_KG_OF_ORE);
  }

  public GameObject CreatePrefab()
  {
    GameObject lightBug = LightBugPinkConfig.CreateLightBug("LightBugPink", (string) STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_PINK.NAME, (string) STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_PINK.DESC, "lightbug_kanim", false);
    EntityTemplates.ExtendEntityToFertileCreature(lightBug, "LightBugPinkEgg", (string) STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_PINK.EGG_NAME, (string) STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_PINK.DESC, "egg_lightbug_kanim", LightBugTuning.EGG_MASS, "LightBugPinkBaby", 15f, 5f, LightBugTuning.EGG_CHANCES_PINK, LightBugPinkConfig.EGG_SORT_ORDER);
    return lightBug;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst) => BaseLightBugConfig.SetupLoopingSounds(inst);
}
