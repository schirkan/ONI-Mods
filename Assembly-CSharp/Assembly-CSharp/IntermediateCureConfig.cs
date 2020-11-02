﻿// Decompiled with JetBrains decompiler
// Type: IntermediateCureConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class IntermediateCureConfig : IEntityConfig
{
  public const string ID = "IntermediateCure";
  public static ComplexRecipe recipe;

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("IntermediateCure", (string) ITEMS.PILLS.INTERMEDIATECURE.NAME, (string) ITEMS.PILLS.INTERMEDIATECURE.DESC, 1f, true, Assets.GetAnim((HashedString) "iv_slimelung_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true);
    looseEntity.GetComponent<KPrefabID>().AddTag(GameTags.MedicalSupplies);
    ComplexRecipe.RecipeElement[] ingredients = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement((Tag) SwampLilyFlowerConfig.ID, 1f),
      new ComplexRecipe.RecipeElement(SimHashes.Phosphorite.CreateTag(), 1f)
    };
    ComplexRecipe.RecipeElement[] results = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "IntermediateCure", 1f)
    };
    IntermediateCureConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Apothecary", (IList<ComplexRecipe.RecipeElement>) ingredients, (IList<ComplexRecipe.RecipeElement>) results), ingredients, results)
    {
      time = 100f,
      description = (string) ITEMS.PILLS.INTERMEDIATECURE.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>() { (Tag) "Apothecary" },
      sortOrder = 10,
      requiredTech = "MedicineII"
    };
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
