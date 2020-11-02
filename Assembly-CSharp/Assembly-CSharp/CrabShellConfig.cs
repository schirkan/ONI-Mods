// Decompiled with JetBrains decompiler
// Type: CrabShellConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class CrabShellConfig : IEntityConfig
{
  public const string ID = "CrabShell";
  public static readonly Tag TAG = TagManager.Create("CrabShell");
  public const float MASS = 10f;

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("CrabShell", (string) ITEMS.INDUSTRIAL_PRODUCTS.CRAB_SHELL.NAME, (string) ITEMS.INDUSTRIAL_PRODUCTS.CRAB_SHELL.DESC, 10f, true, Assets.GetAnim((HashedString) "crabshells_large_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, additionalTags: new List<Tag>()
    {
      GameTags.IndustrialIngredient,
      GameTags.Organics
    });
    looseEntity.AddOrGet<EntitySplitter>();
    looseEntity.AddOrGet<SimpleMassStatusItem>();
    EntityTemplates.CreateAndRegisterCompostableFromPrefab(looseEntity);
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
