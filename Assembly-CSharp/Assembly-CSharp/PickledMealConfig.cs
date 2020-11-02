// Decompiled with JetBrains decompiler
// Type: PickledMealConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class PickledMealConfig : IEntityConfig
{
  public const string ID = "PickledMeal";
  public static ComplexRecipe recipe;

  public GameObject CreatePrefab()
  {
    GameObject food = EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("PickledMeal", (string) ITEMS.FOOD.PICKLEDMEAL.NAME, (string) ITEMS.FOOD.PICKLEDMEAL.DESC, 1f, false, Assets.GetAnim((HashedString) "pickledmeal_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true), TUNING.FOOD.FOOD_TYPES.PICKLEDMEAL);
    food.GetComponent<KPrefabID>().AddTag(GameTags.Pickled);
    return food;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
