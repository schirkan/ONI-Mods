// Decompiled with JetBrains decompiler
// Type: FriedMushBarConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class FriedMushBarConfig : IEntityConfig
{
  public const string ID = "FriedMushBar";
  public static ComplexRecipe recipe;

  public GameObject CreatePrefab() => EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("FriedMushBar", (string) ITEMS.FOOD.FRIEDMUSHBAR.NAME, (string) ITEMS.FOOD.FRIEDMUSHBAR.DESC, 1f, false, Assets.GetAnim((HashedString) "mushbarfried_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true), TUNING.FOOD.FOOD_TYPES.FRIEDMUSHBAR);

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
