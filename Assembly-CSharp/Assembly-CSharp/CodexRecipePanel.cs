// Decompiled with JetBrains decompiler
// Type: CodexRecipePanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodexRecipePanel : CodexWidget<CodexRecipePanel>
{
  private LocText title;
  private GameObject materialPrefab;
  private GameObject fabricatorPrefab;
  private GameObject ingredientsContainer;
  private GameObject resultsContainer;
  private GameObject fabricatorContainer;
  private ComplexRecipe complexRecipe;
  private Recipe recipe;

  public string linkID { get; set; }

  public CodexRecipePanel()
  {
  }

  public CodexRecipePanel(ComplexRecipe recipe) => this.complexRecipe = recipe;

  public CodexRecipePanel(Recipe rec) => this.recipe = rec;

  public override void Configure(
    GameObject contentGameObject,
    Transform displayPane,
    Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
  {
    HierarchyReferences component = contentGameObject.GetComponent<HierarchyReferences>();
    this.title = component.GetReference<LocText>("Title");
    this.materialPrefab = component.GetReference<RectTransform>("MaterialPrefab").gameObject;
    this.fabricatorPrefab = component.GetReference<RectTransform>("FabricatorPrefab").gameObject;
    this.ingredientsContainer = component.GetReference<RectTransform>("IngredientsContainer").gameObject;
    this.resultsContainer = component.GetReference<RectTransform>("ResultsContainer").gameObject;
    this.fabricatorContainer = component.GetReference<RectTransform>("FabricatorContainer").gameObject;
    this.ClearPanel();
    if (this.recipe != null)
    {
      this.ConfigureRecipe();
    }
    else
    {
      if (this.complexRecipe == null)
        return;
      this.ConfigureComplexRecipe();
    }
  }

  private void ConfigureRecipe()
  {
    this.title.text = this.recipe.Result.ProperName();
    foreach (Recipe.Ingredient ingredient in this.recipe.Ingredients)
    {
      GameObject gameObject = Util.KInstantiateUI(this.materialPrefab, this.ingredientsContainer, true);
      HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
      Tuple<Sprite, Color> uiSprite = Def.GetUISprite((object) ingredient.tag);
      component.GetReference<Image>("Icon").sprite = uiSprite.first;
      component.GetReference<Image>("Icon").color = uiSprite.second;
      component.GetReference<LocText>("Amount").text = GameUtil.GetFormattedByTag(ingredient.tag, ingredient.amount);
      component.GetReference<LocText>("Amount").color = Color.black;
      string str = ingredient.tag.ProperName();
      GameObject prefab = Assets.GetPrefab(ingredient.tag);
      if ((UnityEngine.Object) prefab.GetComponent<Edible>() != (UnityEngine.Object) null)
        str = str + "\n    • " + string.Format((string) STRINGS.UI.GAMEOBJECTEFFECTS.FOOD_QUALITY, (object) GameUtil.GetFormattedFoodQuality(prefab.GetComponent<Edible>().GetQuality()));
      gameObject.GetComponent<ToolTip>().toolTip = str;
    }
    GameObject gameObject1 = Util.KInstantiateUI(this.materialPrefab, this.resultsContainer, true);
    HierarchyReferences component1 = gameObject1.GetComponent<HierarchyReferences>();
    Tuple<Sprite, Color> uiSprite1 = Def.GetUISprite((object) this.recipe.Result);
    component1.GetReference<Image>("Icon").sprite = uiSprite1.first;
    component1.GetReference<Image>("Icon").color = uiSprite1.second;
    component1.GetReference<LocText>("Amount").text = GameUtil.GetFormattedByTag(this.recipe.Result, this.recipe.OutputUnits);
    component1.GetReference<LocText>("Amount").color = Color.black;
    string str1 = this.recipe.Result.ProperName();
    GameObject prefab1 = Assets.GetPrefab(this.recipe.Result);
    if ((UnityEngine.Object) prefab1.GetComponent<Edible>() != (UnityEngine.Object) null)
      str1 = str1 + "\n    • " + string.Format((string) STRINGS.UI.GAMEOBJECTEFFECTS.FOOD_QUALITY, (object) GameUtil.GetFormattedFoodQuality(prefab1.GetComponent<Edible>().GetQuality()));
    gameObject1.GetComponent<ToolTip>().toolTip = str1;
  }

  private void ConfigureComplexRecipe()
  {
    this.title.text = this.complexRecipe.results[0].material.ProperName();
    foreach (ComplexRecipe.RecipeElement ingredient in this.complexRecipe.ingredients)
    {
      ComplexRecipe.RecipeElement ing = ingredient;
      HierarchyReferences component = Util.KInstantiateUI(this.materialPrefab, this.ingredientsContainer, true).GetComponent<HierarchyReferences>();
      Tuple<Sprite, Color> uiSprite = Def.GetUISprite((object) ing.material);
      component.GetReference<Image>("Icon").sprite = uiSprite.first;
      component.GetReference<Image>("Icon").color = uiSprite.second;
      component.GetReference<LocText>("Amount").text = GameUtil.GetFormattedByTag(ing.material, ing.amount);
      component.GetReference<LocText>("Amount").color = Color.black;
      string str = ing.material.ProperName();
      GameObject prefab = Assets.GetPrefab(ing.material);
      if ((UnityEngine.Object) prefab.GetComponent<Edible>() != (UnityEngine.Object) null)
        str = str + "\n    • " + string.Format((string) STRINGS.UI.GAMEOBJECTEFFECTS.FOOD_QUALITY, (object) GameUtil.GetFormattedFoodQuality(prefab.GetComponent<Edible>().GetQuality()));
      component.GetReference<ToolTip>("Tooltip").toolTip = str;
      component.GetReference<KButton>("Button").onClick += (System.Action) (() => ManagementMenu.Instance.codexScreen.ChangeArticle(CodexCache.FormatLinkID(ing.material.ToString())));
    }
    foreach (ComplexRecipe.RecipeElement result in this.complexRecipe.results)
    {
      ComplexRecipe.RecipeElement res = result;
      HierarchyReferences component = Util.KInstantiateUI(this.materialPrefab, this.resultsContainer, true).GetComponent<HierarchyReferences>();
      Tuple<Sprite, Color> uiSprite = Def.GetUISprite((object) res.material);
      component.GetReference<Image>("Icon").sprite = uiSprite.first;
      component.GetReference<Image>("Icon").color = uiSprite.second;
      component.GetReference<LocText>("Amount").text = GameUtil.GetFormattedByTag(res.material, res.amount);
      component.GetReference<LocText>("Amount").color = Color.black;
      string str = res.material.ProperName();
      GameObject prefab = Assets.GetPrefab(res.material);
      if ((UnityEngine.Object) prefab.GetComponent<Edible>() != (UnityEngine.Object) null)
        str = str + "\n    • " + string.Format((string) STRINGS.UI.GAMEOBJECTEFFECTS.FOOD_QUALITY, (object) GameUtil.GetFormattedFoodQuality(prefab.GetComponent<Edible>().GetQuality()));
      component.GetReference<ToolTip>("Tooltip").toolTip = str;
      component.GetReference<KButton>("Button").onClick += (System.Action) (() => ManagementMenu.Instance.codexScreen.ChangeArticle(CodexCache.FormatLinkID(res.material.ToString())));
    }
    string fabricatorId = this.complexRecipe.id.Substring(0, this.complexRecipe.id.IndexOf('_'));
    HierarchyReferences component1 = Util.KInstantiateUI(this.fabricatorPrefab, this.fabricatorContainer, true).GetComponent<HierarchyReferences>();
    Tuple<Sprite, Color> uiSprite1 = Def.GetUISprite((object) fabricatorId);
    component1.GetReference<Image>("Icon").sprite = uiSprite1.first;
    component1.GetReference<Image>("Icon").color = uiSprite1.second;
    component1.GetReference<LocText>("Time").text = GameUtil.GetFormattedTime(this.complexRecipe.time);
    component1.GetReference<LocText>("Time").color = Color.black;
    GameObject prefab1 = Assets.GetPrefab(fabricatorId.ToTag());
    component1.GetReference<ToolTip>("Tooltip").toolTip = prefab1.GetProperName();
    component1.GetReference<KButton>("Button").onClick += (System.Action) (() => ManagementMenu.Instance.codexScreen.ChangeArticle(CodexCache.FormatLinkID(fabricatorId)));
  }

  private void ClearPanel()
  {
    foreach (Component component in this.ingredientsContainer.transform)
      UnityEngine.Object.Destroy((UnityEngine.Object) component.gameObject);
    foreach (Component component in this.resultsContainer.transform)
      UnityEngine.Object.Destroy((UnityEngine.Object) component.gameObject);
    foreach (Component component in this.fabricatorContainer.transform)
      UnityEngine.Object.Destroy((UnityEngine.Object) component.gameObject);
  }
}
