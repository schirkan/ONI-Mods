// Decompiled with JetBrains decompiler
// Type: LettuceConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class LettuceConfig : IEntityConfig
{
  public const string ID = "Lettuce";

  public GameObject CreatePrefab() => EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Lettuce", (string) ITEMS.FOOD.LETTUCE.NAME, (string) ITEMS.FOOD.LETTUCE.DESC, 1f, false, Assets.GetAnim((HashedString) "sea_lettuce_leaves_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true), TUNING.FOOD.FOOD_TYPES.LETTUCE);

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
