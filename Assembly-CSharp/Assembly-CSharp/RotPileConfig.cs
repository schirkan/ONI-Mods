// Decompiled with JetBrains decompiler
// Type: RotPileConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using UnityEngine;

public class RotPileConfig : IEntityConfig
{
  public static string ID = "RotPile";

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity(RotPileConfig.ID, (string) ITEMS.FOOD.ROTPILE.NAME, (string) ITEMS.FOOD.ROTPILE.DESC, 1f, false, Assets.GetAnim((HashedString) "rotfood_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true);
    KPrefabID component = looseEntity.GetComponent<KPrefabID>();
    component.AddTag(GameTags.Organics);
    component.AddTag(GameTags.Compostable);
    looseEntity.AddOrGet<EntitySplitter>();
    looseEntity.AddOrGet<OccupyArea>();
    looseEntity.AddOrGet<Modifiers>();
    looseEntity.AddOrGet<RotPile>();
    looseEntity.AddComponent<DecorProvider>().SetValues(TUNING.DECOR.PENALTY.TIER2);
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst) => inst.GetComponent<DecorProvider>().overrideName = (string) ITEMS.FOOD.ROTPILE.NAME;

  public void OnSpawn(GameObject inst)
  {
  }
}
