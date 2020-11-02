// Decompiled with JetBrains decompiler
// Type: SpiceBreadConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class SpiceBreadConfig : IEntityConfig
{
  public const string ID = "SpiceBread";
  public static ComplexRecipe recipe;

  public GameObject CreatePrefab() => EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("SpiceBread", (string) ITEMS.FOOD.SPICEBREAD.NAME, (string) ITEMS.FOOD.SPICEBREAD.DESC, 1f, false, Assets.GetAnim((HashedString) "pepperbread_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true), TUNING.FOOD.FOOD_TYPES.SPICEBREAD);

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
