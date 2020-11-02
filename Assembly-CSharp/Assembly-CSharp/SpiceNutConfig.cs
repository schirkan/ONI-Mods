// Decompiled with JetBrains decompiler
// Type: SpiceNutConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class SpiceNutConfig : IEntityConfig
{
  public static float SEEDS_PER_FRUIT = 1f;
  public static string ID = "SpiceNut";

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity(SpiceNutConfig.ID, (string) ITEMS.FOOD.SPICENUT.NAME, (string) ITEMS.FOOD.SPICENUT.DESC, 1f, false, Assets.GetAnim((HashedString) "spicenut_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, true);
    EntityTemplates.ExtendEntityToFood(looseEntity, TUNING.FOOD.FOOD_TYPES.SPICENUT);
    SoundEventVolumeCache.instance.AddVolume("vinespicenut_kanim", "VineSpiceNut_grow", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("vinespicenut_kanim", "VineSpiceNut_harvest", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
