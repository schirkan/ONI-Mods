// Decompiled with JetBrains decompiler
// Type: GlassForgeConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class GlassForgeConfig : IBuildingConfig
{
  public const string ID = "GlassForge";
  private const float INPUT_KG = 100f;
  public static readonly CellOffset outPipeOffset = new CellOffset(1, 3);
  private static readonly List<Storage.StoredItemModifier> RefineryStoredItemModifiers = new List<Storage.StoredItemModifier>()
  {
    Storage.StoredItemModifier.Hide,
    Storage.StoredItemModifier.Preserve
  };

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR5 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
    string[] allMinerals = MATERIALS.ALL_MINERALS;
    EffectorValues tieR6 = TUNING.NOISE_POLLUTION.NOISY.TIER6;
    EffectorValues tieR2 = TUNING.BUILDINGS.DECOR.PENALTY.TIER2;
    EffectorValues noise = tieR6;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("GlassForge", 5, 4, "glassrefinery_kanim", 30, 60f, tieR5, allMinerals, 2400f, BuildLocationRule.OnFloor, tieR2, noise);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 1200f;
    buildingDef.SelfHeatKilowattsWhenActive = 16f;
    buildingDef.OutputConduitType = ConduitType.Liquid;
    buildingDef.UtilityOutputOffset = GlassForgeConfig.outPipeOffset;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.AudioSize = "large";
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<DropAllWorkable>();
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    GlassForge glassForge = go.AddOrGet<GlassForge>();
    glassForge.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
    go.AddOrGet<FabricatorIngredientStatusManager>();
    go.AddOrGet<CopyBuildingSettings>();
    ComplexFabricatorWorkable fabricatorWorkable = go.AddOrGet<ComplexFabricatorWorkable>();
    glassForge.duplicantOperated = true;
    BuildingTemplates.CreateComplexFabricatorStorage(go, (ComplexFabricator) glassForge);
    glassForge.outStorage.capacityKg = 2000f;
    glassForge.storeProduced = true;
    glassForge.inStorage.SetDefaultStoredItemModifiers(GlassForgeConfig.RefineryStoredItemModifiers);
    glassForge.buildStorage.SetDefaultStoredItemModifiers(GlassForgeConfig.RefineryStoredItemModifiers);
    glassForge.outStorage.SetDefaultStoredItemModifiers(GlassForgeConfig.RefineryStoredItemModifiers);
    glassForge.outputOffset = new Vector3(1f, 0.5f);
    KAnimFile[] kanimFileArray = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_metalrefinery_kanim")
    };
    fabricatorWorkable.overrideAnims = kanimFileArray;
    glassForge.resultState = ComplexFabricator.ResultState.Melted;
    ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
    conduitDispenser.storage = glassForge.outStorage;
    conduitDispenser.conduitType = ConduitType.Liquid;
    conduitDispenser.elementFilter = (SimHashes[]) null;
    conduitDispenser.alwaysDispense = true;
    ComplexRecipe.RecipeElement[] ingredients = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Sand).tag, 100f)
    };
    ComplexRecipe.RecipeElement[] results = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.MoltenGlass).tag, 25f)
    };
    string obsolete_id = ComplexRecipeManager.MakeObsoleteRecipeID("GlassForge", ingredients[0].material);
    string str = ComplexRecipeManager.MakeRecipeID("GlassForge", (IList<ComplexRecipe.RecipeElement>) ingredients, (IList<ComplexRecipe.RecipeElement>) results);
    ComplexRecipe complexRecipe = new ComplexRecipe(str, ingredients, results)
    {
      time = 40f,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.GLASSFORGE.RECIPE_DESCRIPTION, (object) ElementLoader.GetElement(results[0].material).name, (object) ElementLoader.GetElement(ingredients[0].material).name),
      fabricators = new List<Tag>()
      {
        TagManager.Create("GlassForge")
      }
    };
    ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id, str);
    Prioritizable.AddRef(go);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    SymbolOverrideControllerUtil.AddToPrefab(go);
    go.AddOrGetDef<PoweredActiveStoppableController.Def>();
    go.GetComponent<KPrefabID>().prefabSpawnFn += (KPrefabID.PrefabFn) (game_object =>
    {
      ComplexFabricatorWorkable component = game_object.GetComponent<ComplexFabricatorWorkable>();
      component.WorkerStatusItem = Db.Get().DuplicantStatusItems.Processing;
      component.AttributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
      component.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
      component.SkillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
      component.SkillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
    });
  }
}
