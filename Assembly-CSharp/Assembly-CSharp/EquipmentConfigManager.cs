// Decompiled with JetBrains decompiler
// Type: EquipmentConfigManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/EquipmentConfigManager")]
public class EquipmentConfigManager : KMonoBehaviour
{
  public static EquipmentConfigManager Instance;

  public static void DestroyInstance() => EquipmentConfigManager.Instance = (EquipmentConfigManager) null;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    EquipmentConfigManager.Instance = this;
  }

  public void RegisterEquipment(IEquipmentConfig config)
  {
    EquipmentDef equipmentDef = config.CreateEquipmentDef();
    GameObject looseEntity = EntityTemplates.CreateLooseEntity(equipmentDef.Id, equipmentDef.Name, equipmentDef.RecipeDescription, equipmentDef.Mass, true, equipmentDef.Anim, "object", Grid.SceneLayer.Ore, equipmentDef.CollisionShape, equipmentDef.width, equipmentDef.height, true, element: equipmentDef.OutputElement);
    Equippable equippable = looseEntity.AddComponent<Equippable>();
    equippable.def = equipmentDef;
    Debug.Assert((Object) equippable.def != (Object) null);
    equippable.slotID = equipmentDef.Slot;
    Debug.Assert(equippable.slot != null);
    config.DoPostConfigure(looseEntity);
    Assets.AddPrefab(looseEntity.GetComponent<KPrefabID>());
  }

  private void LoadRecipe(EquipmentDef def, Equippable equippable)
  {
    Recipe recipe = new Recipe(def.Id, recipeDescription: def.RecipeDescription);
    recipe.SetFabricator(def.FabricatorId, def.FabricationTime);
    recipe.TechUnlock = def.RecipeTechUnlock;
    foreach (KeyValuePair<string, float> inputElementMass in def.InputElementMassMap)
      recipe.AddIngredient(new Recipe.Ingredient(inputElementMass.Key, inputElementMass.Value));
  }
}
