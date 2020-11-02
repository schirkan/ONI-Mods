// Decompiled with JetBrains decompiler
// Type: GrilledPrickleFruitConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class GrilledPrickleFruitConfig : IEntityConfig
{
  public const string ID = "GrilledPrickleFruit";
  public static ComplexRecipe recipe;

  public GameObject CreatePrefab() => EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("GrilledPrickleFruit", (string) ITEMS.FOOD.GRILLEDPRICKLEFRUIT.NAME, (string) ITEMS.FOOD.GRILLEDPRICKLEFRUIT.DESC, 1f, false, Assets.GetAnim((HashedString) "gristleberry_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.7f, true), TUNING.FOOD.FOOD_TYPES.GRILLED_PRICKLEFRUIT);

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
