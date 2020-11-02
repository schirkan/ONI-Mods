// Decompiled with JetBrains decompiler
// Type: ForestForagePlantConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class ForestForagePlantConfig : IEntityConfig
{
  public const string ID = "ForestForagePlant";

  public GameObject CreatePrefab() => EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("ForestForagePlant", (string) ITEMS.FOOD.FORESTFORAGEPLANT.NAME, (string) ITEMS.FOOD.FORESTFORAGEPLANT.DESC, 1f, false, Assets.GetAnim((HashedString) "podmelon_fruit_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, true), TUNING.FOOD.FOOD_TYPES.FORESTFORAGEPLANT);

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
