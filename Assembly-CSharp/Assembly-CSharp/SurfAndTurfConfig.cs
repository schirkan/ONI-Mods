// Decompiled with JetBrains decompiler
// Type: SurfAndTurfConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class SurfAndTurfConfig : IEntityConfig
{
  public const string ID = "SurfAndTurf";
  public static ComplexRecipe recipe;

  public GameObject CreatePrefab() => EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("SurfAndTurf", (string) ITEMS.FOOD.SURFANDTURF.NAME, (string) ITEMS.FOOD.SURFANDTURF.DESC, 1f, false, Assets.GetAnim((HashedString) "surfnturf_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true), TUNING.FOOD.FOOD_TYPES.SURF_AND_TURF);

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
