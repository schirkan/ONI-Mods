// Decompiled with JetBrains decompiler
// Type: LightBugCrystalConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class LightBugCrystalConfig : IEntityConfig
{
  public const string ID = "LightBugCrystal";
  public const string BASE_TRAIT_ID = "LightBugCrystalBaseTrait";
  public const string EGG_ID = "LightBugCrystalEgg";
  private static float KG_ORE_EATEN_PER_CYCLE = 1f;
  private static float CALORIES_PER_KG_OF_ORE = LightBugTuning.STANDARD_CALORIES_PER_CYCLE / LightBugCrystalConfig.KG_ORE_EATEN_PER_CYCLE;
  public static int EGG_SORT_ORDER = LightBugConfig.EGG_SORT_ORDER + 7;

  public static GameObject CreateLightBug(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject prefab = BaseLightBugConfig.BaseLightBug(id, name, desc, anim_file, "LightBugCrystalBaseTrait", LIGHT2D.LIGHTBUG_COLOR_CRYSTAL, TUNING.DECOR.BONUS.TIER8, is_baby, "cry_");
    EntityTemplates.ExtendEntityToWildCreature(prefab, LightBugTuning.PEN_SIZE_PER_CREATURE, 75f);
    Trait trait = Db.Get().CreateTrait("LightBugCrystalBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, LightBugTuning.STANDARD_STOMACH_SIZE, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) LightBugTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 5f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 75f, name));
    GameObject go = BaseLightBugConfig.SetupDiet(prefab, new HashSet<Tag>()
    {
      TagManager.Create("CookedMeat"),
      SimHashes.Diamond.CreateTag()
    }, Tag.Invalid, LightBugCrystalConfig.CALORIES_PER_KG_OF_ORE);
    go.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[1]
    {
      SimHashes.Diamond.CreateTag()
    };
    return go;
  }

  public GameObject CreatePrefab()
  {
    GameObject lightBug = LightBugCrystalConfig.CreateLightBug("LightBugCrystal", (string) STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_CRYSTAL.NAME, (string) STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_CRYSTAL.DESC, "lightbug_kanim", false);
    EntityTemplates.ExtendEntityToFertileCreature(lightBug, "LightBugCrystalEgg", (string) STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_CRYSTAL.EGG_NAME, (string) STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_CRYSTAL.DESC, "egg_lightbug_kanim", LightBugTuning.EGG_MASS, "LightBugCrystalBaby", 45f, 15f, LightBugTuning.EGG_CHANCES_CRYSTAL, LightBugCrystalConfig.EGG_SORT_ORDER);
    return lightBug;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst) => BaseLightBugConfig.SetupLoopingSounds(inst);
}
