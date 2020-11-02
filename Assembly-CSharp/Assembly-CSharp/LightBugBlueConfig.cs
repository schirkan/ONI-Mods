// Decompiled with JetBrains decompiler
// Type: LightBugBlueConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class LightBugBlueConfig : IEntityConfig
{
  public const string ID = "LightBugBlue";
  public const string BASE_TRAIT_ID = "LightBugBlueBaseTrait";
  public const string EGG_ID = "LightBugBlueEgg";
  private static float KG_ORE_EATEN_PER_CYCLE = 1f;
  private static float CALORIES_PER_KG_OF_ORE = LightBugTuning.STANDARD_CALORIES_PER_CYCLE / LightBugBlueConfig.KG_ORE_EATEN_PER_CYCLE;
  public static int EGG_SORT_ORDER = LightBugConfig.EGG_SORT_ORDER + 4;

  public static GameObject CreateLightBug(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject prefab = BaseLightBugConfig.BaseLightBug(id, name, desc, anim_file, "LightBugBlueBaseTrait", LIGHT2D.LIGHTBUG_COLOR_BLUE, TUNING.DECOR.BONUS.TIER6, is_baby, "blu_");
    EntityTemplates.ExtendEntityToWildCreature(prefab, LightBugTuning.PEN_SIZE_PER_CREATURE, 25f);
    Trait trait = Db.Get().CreateTrait("LightBugBlueBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, LightBugTuning.STANDARD_STOMACH_SIZE, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) LightBugTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 5f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 25f, name));
    GameObject go = BaseLightBugConfig.SetupDiet(prefab, new HashSet<Tag>()
    {
      TagManager.Create("SpiceBread"),
      TagManager.Create("Salsa"),
      SimHashes.Phosphorite.CreateTag(),
      SimHashes.Phosphorus.CreateTag()
    }, Tag.Invalid, LightBugBlueConfig.CALORIES_PER_KG_OF_ORE);
    go.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[2]
    {
      SimHashes.Phosphorite.CreateTag(),
      SimHashes.Phosphorus.CreateTag()
    };
    return go;
  }

  public GameObject CreatePrefab()
  {
    GameObject lightBug = LightBugBlueConfig.CreateLightBug("LightBugBlue", (string) STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_BLUE.NAME, (string) STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_BLUE.DESC, "lightbug_kanim", false);
    EntityTemplates.ExtendEntityToFertileCreature(lightBug, "LightBugBlueEgg", (string) STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_BLUE.EGG_NAME, (string) STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_BLUE.DESC, "egg_lightbug_kanim", LightBugTuning.EGG_MASS, "LightBugBlueBaby", 15f, 5f, LightBugTuning.EGG_CHANCES_BLUE, LightBugBlueConfig.EGG_SORT_ORDER);
    return lightBug;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst) => BaseLightBugConfig.SetupLoopingSounds(inst);
}
