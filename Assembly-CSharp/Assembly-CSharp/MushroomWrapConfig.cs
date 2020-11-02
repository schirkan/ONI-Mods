// Decompiled with JetBrains decompiler
// Type: MushroomWrapConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class MushroomWrapConfig : IEntityConfig
{
  public const string ID = "MushroomWrap";
  public static ComplexRecipe recipe;

  public GameObject CreatePrefab() => EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("MushroomWrap", (string) ITEMS.FOOD.MUSHROOMWRAP.NAME, (string) ITEMS.FOOD.MUSHROOMWRAP.DESC, 1f, false, Assets.GetAnim((HashedString) "mushroom_wrap_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.5f, true), TUNING.FOOD.FOOD_TYPES.MUSHROOM_WRAP);

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
