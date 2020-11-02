﻿// Decompiled with JetBrains decompiler
// Type: PacuTropicalConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class PacuTropicalConfig : IEntityConfig
{
  public const string ID = "PacuTropical";
  public const string BASE_TRAIT_ID = "PacuTropicalBaseTrait";
  public const string EGG_ID = "PacuTropicalEgg";
  public static readonly EffectorValues DECOR = TUNING.BUILDINGS.DECOR.BONUS.TIER4;
  public const int EGG_SORT_ORDER = 502;

  public static GameObject CreatePacu(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject wildCreature = EntityTemplates.ExtendEntityToWildCreature(BasePacuConfig.CreatePrefab(id, "PacuTropicalBaseTrait", name, desc, anim_file, is_baby, "trp_", 303.15f, 353.15f), PacuTuning.PEN_SIZE_PER_CREATURE, 25f);
    wildCreature.AddOrGet<DecorProvider>().SetValues(PacuTropicalConfig.DECOR);
    return wildCreature;
  }

  public GameObject CreatePrefab() => EntityTemplates.ExtendEntityToFertileCreature(EntityTemplates.ExtendEntityToWildCreature(PacuTropicalConfig.CreatePacu("PacuTropical", (string) STRINGS.CREATURES.SPECIES.PACU.VARIANT_TROPICAL.NAME, (string) STRINGS.CREATURES.SPECIES.PACU.VARIANT_TROPICAL.DESC, "pacu_kanim", false), PacuTuning.PEN_SIZE_PER_CREATURE, 25f), "PacuTropicalEgg", (string) STRINGS.CREATURES.SPECIES.PACU.VARIANT_TROPICAL.EGG_NAME, (string) STRINGS.CREATURES.SPECIES.PACU.VARIANT_TROPICAL.DESC, "egg_pacu_kanim", PacuTuning.EGG_MASS, "PacuTropicalBaby", 15f, 5f, PacuTuning.EGG_CHANCES_TROPICAL, 502, false, true, false, 0.75f);

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}