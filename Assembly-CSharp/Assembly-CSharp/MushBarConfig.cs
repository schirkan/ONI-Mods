// Decompiled with JetBrains decompiler
// Type: MushBarConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class MushBarConfig : IEntityConfig
{
  public const string ID = "MushBar";
  public static ComplexRecipe recipe;

  public GameObject CreatePrefab()
  {
    GameObject food = EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("MushBar", (string) ITEMS.FOOD.MUSHBAR.NAME, (string) ITEMS.FOOD.MUSHBAR.DESC, 1f, false, Assets.GetAnim((HashedString) "mushbar_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true), TUNING.FOOD.FOOD_TYPES.MUSHBAR);
    ComplexRecipeManager.Get().GetRecipe(MushBarConfig.recipe.id).FabricationVisualizer = MushBarConfig.CreateFabricationVisualizer(food);
    return food;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }

  public static GameObject CreateFabricationVisualizer(GameObject result)
  {
    KBatchedAnimController component = result.GetComponent<KBatchedAnimController>();
    GameObject gameObject = new GameObject();
    gameObject.name = result.name + "Visualizer";
    gameObject.SetActive(false);
    gameObject.transform.SetLocalPosition(Vector3.zero);
    KBatchedAnimController kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
    kbatchedAnimController.AnimFiles = component.AnimFiles;
    kbatchedAnimController.initialAnim = "fabricating";
    kbatchedAnimController.isMovable = true;
    KBatchedAnimTracker kbatchedAnimTracker = gameObject.AddComponent<KBatchedAnimTracker>();
    kbatchedAnimTracker.symbol = new HashedString("meter_ration");
    kbatchedAnimTracker.offset = Vector3.zero;
    kbatchedAnimTracker.skipInitialDisable = true;
    Object.DontDestroyOnLoad((Object) gameObject);
    return gameObject;
  }
}
