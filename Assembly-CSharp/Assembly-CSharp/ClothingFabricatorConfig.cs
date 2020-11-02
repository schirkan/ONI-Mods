﻿// Decompiled with JetBrains decompiler
// Type: ClothingFabricatorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class ClothingFabricatorConfig : IBuildingConfig
{
  public const string ID = "ClothingFabricator";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR3 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    EffectorValues tieR5 = TUNING.NOISE_POLLUTION.NOISY.TIER5;
    EffectorValues none = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR5;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("ClothingFabricator", 4, 3, "clothingfactory_kanim", 100, 240f, tieR3, refinedMetals, 1600f, BuildLocationRule.OnFloor, none, noise);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 240f;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.PowerInputOffset = new CellOffset(2, 0);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    go.AddOrGet<DropAllWorkable>();
    Prioritizable.AddRef(go);
    ComplexFabricator fabricator = go.AddOrGet<ComplexFabricator>();
    go.AddOrGet<ComplexFabricatorWorkable>().overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_clothingfactory_kanim")
    };
    go.AddOrGet<ComplexFabricatorWorkable>().AnimOffset = new Vector3(-1f, 0.0f, 0.0f);
    fabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
    go.AddOrGet<FabricatorIngredientStatusManager>();
    go.AddOrGet<CopyBuildingSettings>();
    this.ConfigureRecipes();
    BuildingTemplates.CreateComplexFabricatorStorage(go, fabricator);
  }

  private void ConfigureRecipes()
  {
    ComplexRecipe.RecipeElement[] ingredients1 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("BasicFabric".ToTag(), (float) TUNING.EQUIPMENT.VESTS.WARM_VEST_MASS)
    };
    ComplexRecipe.RecipeElement[] results1 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("Warm_Vest".ToTag(), 1f)
    };
    WarmVestConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("ClothingFabricator", (IList<ComplexRecipe.RecipeElement>) ingredients1, (IList<ComplexRecipe.RecipeElement>) results1), ingredients1, results1)
    {
      time = TUNING.EQUIPMENT.VESTS.WARM_VEST_FABTIME,
      description = (string) STRINGS.EQUIPMENT.PREFABS.WARM_VEST.RECIPE_DESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "ClothingFabricator"
      },
      sortOrder = 1
    };
    ComplexRecipe.RecipeElement[] ingredients2 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("BasicFabric".ToTag(), (float) TUNING.EQUIPMENT.VESTS.COOL_VEST_MASS)
    };
    ComplexRecipe.RecipeElement[] results2 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("Cool_Vest".ToTag(), 1f)
    };
    CoolVestConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("ClothingFabricator", (IList<ComplexRecipe.RecipeElement>) ingredients2, (IList<ComplexRecipe.RecipeElement>) results2), ingredients2, results2)
    {
      time = TUNING.EQUIPMENT.VESTS.COOL_VEST_FABTIME,
      description = (string) STRINGS.EQUIPMENT.PREFABS.COOL_VEST.RECIPE_DESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "ClothingFabricator"
      },
      sortOrder = 1
    };
    ComplexRecipe.RecipeElement[] ingredients3 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("BasicFabric".ToTag(), (float) TUNING.EQUIPMENT.VESTS.FUNKY_VEST_MASS)
    };
    ComplexRecipe.RecipeElement[] results3 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("Funky_Vest".ToTag(), 1f)
    };
    FunkyVestConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("ClothingFabricator", (IList<ComplexRecipe.RecipeElement>) ingredients3, (IList<ComplexRecipe.RecipeElement>) results3), ingredients3, results3)
    {
      time = TUNING.EQUIPMENT.VESTS.FUNKY_VEST_FABTIME,
      description = (string) STRINGS.EQUIPMENT.PREFABS.FUNKY_VEST.RECIPE_DESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "ClothingFabricator"
      },
      sortOrder = 1
    };
  }

  public override void DoPostConfigureComplete(GameObject go) => go.GetComponent<KPrefabID>().prefabSpawnFn += (KPrefabID.PrefabFn) (game_object =>
  {
    ComplexFabricatorWorkable component = game_object.GetComponent<ComplexFabricatorWorkable>();
    component.WorkerStatusItem = Db.Get().DuplicantStatusItems.Fabricating;
    component.AttributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
    component.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
    component.SkillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
    component.SkillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
  });
}
