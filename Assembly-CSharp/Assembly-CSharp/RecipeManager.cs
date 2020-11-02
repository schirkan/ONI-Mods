// Decompiled with JetBrains decompiler
// Type: RecipeManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class RecipeManager
{
  private static RecipeManager _Instance;
  public List<Recipe> recipes = new List<Recipe>();

  public static RecipeManager Get()
  {
    if (RecipeManager._Instance == null)
      RecipeManager._Instance = new RecipeManager();
    return RecipeManager._Instance;
  }

  public static void DestroyInstance() => RecipeManager._Instance = (RecipeManager) null;

  public void Add(Recipe recipe)
  {
    this.recipes.Add(recipe);
    if (!((Object) recipe.FabricationVisualizer != (Object) null))
      return;
    Object.DontDestroyOnLoad((Object) recipe.FabricationVisualizer);
  }
}
