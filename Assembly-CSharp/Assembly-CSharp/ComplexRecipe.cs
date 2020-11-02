// Decompiled with JetBrains decompiler
// Type: ComplexRecipe
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class ComplexRecipe
{
  public string id;
  public ComplexRecipe.RecipeElement[] ingredients;
  public ComplexRecipe.RecipeElement[] results;
  public float time;
  public GameObject FabricationVisualizer;
  public ComplexRecipe.RecipeNameDisplay nameDisplay;
  public string description;
  public List<Tag> fabricators;
  public int sortOrder;
  public string requiredTech;

  public Tag FirstResult => this.results[0].material;

  public ComplexRecipe(
    string id,
    ComplexRecipe.RecipeElement[] ingredients,
    ComplexRecipe.RecipeElement[] results)
  {
    this.id = id;
    this.ingredients = ingredients;
    this.results = results;
    ComplexRecipeManager.Get().Add(this);
  }

  public float TotalResultUnits()
  {
    float num = 0.0f;
    foreach (ComplexRecipe.RecipeElement result in this.results)
      num += result.amount;
    return num;
  }

  public bool RequiresTechUnlock() => !string.IsNullOrEmpty(this.requiredTech);

  public bool IsRequiredTechUnlocked() => string.IsNullOrEmpty(this.requiredTech) || Db.Get().Techs.Get(this.requiredTech).IsComplete();

  public Sprite GetUIIcon()
  {
    Sprite sprite = (Sprite) null;
    KBatchedAnimController component = Assets.GetPrefab(this.nameDisplay == ComplexRecipe.RecipeNameDisplay.Ingredient ? this.ingredients[0].material : this.results[0].material).GetComponent<KBatchedAnimController>();
    if ((Object) component != (Object) null)
      sprite = Def.GetUISpriteFromMultiObjectAnim(component.AnimFiles[0]);
    return sprite;
  }

  public Color GetUIColor() => Color.white;

  public string GetUIName(bool includeAmounts)
  {
    switch (this.nameDisplay)
    {
      case ComplexRecipe.RecipeNameDisplay.Result:
        return includeAmounts ? string.Format((string) UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_SIMPLE_INCLUDE_AMOUNTS, (object) this.results[0].material.ProperName(), (object) this.results[0].amount) : this.results[0].material.ProperName();
      case ComplexRecipe.RecipeNameDisplay.IngredientToResult:
        if (!includeAmounts)
          return string.Format((string) UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO, (object) this.ingredients[0].material.ProperName(), (object) this.results[0].material.ProperName());
        return string.Format((string) UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO_INCLUDE_AMOUNTS, (object) this.ingredients[0].material.ProperName(), (object) this.results[0].material.ProperName(), (object) this.ingredients[0].amount, (object) this.results[0].amount);
      case ComplexRecipe.RecipeNameDisplay.ResultWithIngredient:
        if (!includeAmounts)
          return string.Format((string) UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_WITH, (object) this.ingredients[0].material.ProperName(), (object) this.results[0].material.ProperName());
        return string.Format((string) UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_WITH_INCLUDE_AMOUNTS, (object) this.ingredients[0].material.ProperName(), (object) this.results[0].material.ProperName(), (object) this.ingredients[0].amount, (object) this.results[0].amount);
      default:
        return includeAmounts ? string.Format((string) UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_SIMPLE_INCLUDE_AMOUNTS, (object) this.ingredients[0].material.ProperName(), (object) this.ingredients[0].amount) : this.ingredients[0].material.ProperName();
    }
  }

  public enum RecipeNameDisplay
  {
    Ingredient,
    Result,
    IngredientToResult,
    ResultWithIngredient,
  }

  public class RecipeElement
  {
    public Tag material;

    public RecipeElement(Tag material, float amount)
    {
      this.material = material;
      this.amount = amount;
    }

    public float amount { get; private set; }
  }
}
