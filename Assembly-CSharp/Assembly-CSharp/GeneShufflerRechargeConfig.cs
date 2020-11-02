﻿// Decompiled with JetBrains decompiler
// Type: GeneShufflerRechargeConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class GeneShufflerRechargeConfig : IEntityConfig
{
  public const string ID = "GeneShufflerRecharge";
  public static readonly Tag tag = TagManager.Create("GeneShufflerRecharge");
  public const float MASS = 5f;

  public GameObject CreatePrefab() => EntityTemplates.CreateLooseEntity("GeneShufflerRecharge", (string) ITEMS.INDUSTRIAL_PRODUCTS.GENE_SHUFFLER_RECHARGE.NAME, (string) ITEMS.INDUSTRIAL_PRODUCTS.GENE_SHUFFLER_RECHARGE.DESC, 5f, true, Assets.GetAnim((HashedString) "vacillator_charge_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true, additionalTags: new List<Tag>()
  {
    GameTags.IndustrialIngredient
  });

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
