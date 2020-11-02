// Decompiled with JetBrains decompiler
// Type: BasicPlantFoodConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class BasicPlantFoodConfig : IEntityConfig
{
  public const string ID = "BasicPlantFood";

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("BasicPlantFood", (string) ITEMS.FOOD.BASICPLANTFOOD.NAME, (string) ITEMS.FOOD.BASICPLANTFOOD.DESC, 1f, false, Assets.GetAnim((HashedString) "meallicegrain_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, true);
    EntityTemplates.ExtendEntityToFood(looseEntity, TUNING.FOOD.FOOD_TYPES.BASICPLANTFOOD);
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
