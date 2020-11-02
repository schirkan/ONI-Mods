// Decompiled with JetBrains decompiler
// Type: BasicPlantBarConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class BasicPlantBarConfig : IEntityConfig
{
  public const string ID = "BasicPlantBar";
  public static ComplexRecipe recipe;

  public GameObject CreatePrefab()
  {
    GameObject food = EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("BasicPlantBar", (string) ITEMS.FOOD.BASICPLANTBAR.NAME, (string) ITEMS.FOOD.BASICPLANTBAR.DESC, 1f, false, Assets.GetAnim((HashedString) "liceloaf_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true), TUNING.FOOD.FOOD_TYPES.BASICPLANTBAR);
    ComplexRecipeManager.Get().GetRecipe(BasicPlantBarConfig.recipe.id).FabricationVisualizer = MushBarConfig.CreateFabricationVisualizer(food);
    return food;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
