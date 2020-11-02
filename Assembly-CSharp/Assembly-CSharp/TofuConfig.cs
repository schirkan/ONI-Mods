﻿// Decompiled with JetBrains decompiler
// Type: TofuConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class TofuConfig : IEntityConfig
{
  public const string ID = "Tofu";
  public static ComplexRecipe recipe;

  public GameObject CreatePrefab()
  {
    GameObject food = EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Tofu", (string) ITEMS.FOOD.TOFU.NAME, (string) ITEMS.FOOD.TOFU.DESC, 1f, false, Assets.GetAnim((HashedString) "loafu_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true), TUNING.FOOD.FOOD_TYPES.TOFU);
    ComplexRecipeManager.Get().GetRecipe(TofuConfig.recipe.id).FabricationVisualizer = MushBarConfig.CreateFabricationVisualizer(food);
    return food;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
