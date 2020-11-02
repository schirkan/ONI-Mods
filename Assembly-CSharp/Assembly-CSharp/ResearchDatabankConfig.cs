﻿// Decompiled with JetBrains decompiler
// Type: ResearchDatabankConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class ResearchDatabankConfig : IEntityConfig
{
  public const string ID = "ResearchDatabank";
  public static readonly Tag TAG = TagManager.Create("ResearchDatabank");
  public const float MASS = 1f;

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("ResearchDatabank", (string) ITEMS.INDUSTRIAL_PRODUCTS.RESEARCH_DATABANK.NAME, (string) ITEMS.INDUSTRIAL_PRODUCTS.RESEARCH_DATABANK.DESC, 1f, true, Assets.GetAnim((HashedString) "floppy_disc_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.35f, 0.35f, true, additionalTags: new List<Tag>()
    {
      GameTags.IndustrialIngredient,
      GameTags.Experimental
    });
    looseEntity.AddOrGet<EntitySplitter>().maxStackSize = (float) TUNING.ROCKETRY.DESTINATION_RESEARCH.BASIC;
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
