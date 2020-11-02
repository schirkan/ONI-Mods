// Decompiled with JetBrains decompiler
// Type: FriedMushroomConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class FriedMushroomConfig : IEntityConfig
{
  public const string ID = "FriedMushroom";
  public static ComplexRecipe recipe;

  public GameObject CreatePrefab() => EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("FriedMushroom", (string) ITEMS.FOOD.FRIEDMUSHROOM.NAME, (string) ITEMS.FOOD.FRIEDMUSHROOM.DESC, 1f, false, Assets.GetAnim((HashedString) "funguscapfried_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true), TUNING.FOOD.FOOD_TYPES.FRIED_MUSHROOM);

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
