﻿// Decompiled with JetBrains decompiler
// Type: MicrobeMusherConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class MicrobeMusherConfig : IBuildingConfig
{
  public const string ID = "MicrobeMusher";
  public static EffectorValues DECOR = TUNING.BUILDINGS.DECOR.PENALTY.TIER2;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] allMetals = MATERIALS.ALL_METALS;
    EffectorValues tieR3 = TUNING.NOISE_POLLUTION.NOISY.TIER3;
    EffectorValues decor = MicrobeMusherConfig.DECOR;
    EffectorValues noise = tieR3;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("MicrobeMusher", 2, 3, "microbemusher_kanim", 30, 30f, tieR4, allMetals, 800f, BuildLocationRule.OnFloor, decor, noise);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 240f;
    buildingDef.ExhaustKilowattsWhenActive = 0.5f;
    buildingDef.SelfHeatKilowattsWhenActive = 2f;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Glass";
    buildingDef.AudioSize = "large";
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<DropAllWorkable>();
    Prioritizable.AddRef(go);
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    go.AddOrGet<ConduitConsumer>().conduitType = ConduitType.Liquid;
    MicrobeMusher microbeMusher = go.AddOrGet<MicrobeMusher>();
    microbeMusher.mushbarSpawnOffset = new Vector3(1f, 0.0f, 0.0f);
    go.AddOrGet<FabricatorIngredientStatusManager>();
    go.AddOrGet<CopyBuildingSettings>();
    go.AddOrGet<ComplexFabricatorWorkable>().overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_musher_kanim")
    };
    microbeMusher.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
    BuildingTemplates.CreateComplexFabricatorStorage(go, (ComplexFabricator) microbeMusher);
    this.ConfigureRecipes();
    go.AddOrGetDef<PoweredController.Def>();
  }

  private void ConfigureRecipes()
  {
    ComplexRecipe.RecipeElement[] ingredients1 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement("Dirt".ToTag(), 75f),
      new ComplexRecipe.RecipeElement("Water".ToTag(), 75f)
    };
    ComplexRecipe.RecipeElement[] results1 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("MushBar".ToTag(), 1f)
    };
    MushBarConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("MicrobeMusher", (IList<ComplexRecipe.RecipeElement>) ingredients1, (IList<ComplexRecipe.RecipeElement>) results1), ingredients1, results1)
    {
      time = 40f,
      description = (string) ITEMS.FOOD.MUSHBAR.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "MicrobeMusher"
      },
      sortOrder = 1
    };
    ComplexRecipe.RecipeElement[] ingredients2 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement((Tag) "BasicPlantFood", 2f),
      new ComplexRecipe.RecipeElement("Water".ToTag(), 50f)
    };
    ComplexRecipe.RecipeElement[] results2 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("BasicPlantBar".ToTag(), 1f)
    };
    BasicPlantBarConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("MicrobeMusher", (IList<ComplexRecipe.RecipeElement>) ingredients2, (IList<ComplexRecipe.RecipeElement>) results2), ingredients2, results2)
    {
      time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
      description = (string) ITEMS.FOOD.BASICPLANTBAR.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "MicrobeMusher"
      },
      sortOrder = 2
    };
    ComplexRecipe.RecipeElement[] ingredients3 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement((Tag) "BeanPlantSeed", 6f),
      new ComplexRecipe.RecipeElement("Water".ToTag(), 50f)
    };
    ComplexRecipe.RecipeElement[] results3 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("Tofu".ToTag(), 1f)
    };
    TofuConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("MicrobeMusher", (IList<ComplexRecipe.RecipeElement>) ingredients3, (IList<ComplexRecipe.RecipeElement>) results3), ingredients3, results3)
    {
      time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
      description = (string) ITEMS.FOOD.TOFU.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "MicrobeMusher"
      },
      sortOrder = 3
    };
    ComplexRecipe.RecipeElement[] ingredients4 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement((Tag) "ColdWheatSeed", 5f),
      new ComplexRecipe.RecipeElement((Tag) PrickleFruitConfig.ID, 1f)
    };
    ComplexRecipe.RecipeElement[] results4 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("FruitCake".ToTag(), 1f)
    };
    FruitCakeConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("MicrobeMusher", (IList<ComplexRecipe.RecipeElement>) ingredients4, (IList<ComplexRecipe.RecipeElement>) results4), ingredients4, results4)
    {
      time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
      description = (string) ITEMS.FOOD.FRUITCAKE.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "MicrobeMusher"
      },
      sortOrder = 3
    };
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
