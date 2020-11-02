// Decompiled with JetBrains decompiler
// Type: ConsumerManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ConsumerManager")]
public class ConsumerManager : KMonoBehaviour, ISaveLoadable
{
  public static ConsumerManager instance;
  [Serialize]
  private List<Tag> undiscoveredConsumableTags = new List<Tag>();
  [Serialize]
  private List<Tag> defaultForbiddenTagsList = new List<Tag>();

  public static void DestroyInstance() => ConsumerManager.instance = (ConsumerManager) null;

  public event System.Action<Tag> OnDiscover;

  public List<Tag> DefaultForbiddenTagsList => this.defaultForbiddenTagsList;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    ConsumerManager.instance = this;
    this.RefreshDiscovered();
    WorldInventory.Instance.OnDiscover += new System.Action<Tag, Tag>(this.OnWorldInventoryDiscover);
    Game.Instance.Subscribe(-107300940, new System.Action<object>(this.RefreshDiscovered));
  }

  public bool isDiscovered(Tag id) => !this.undiscoveredConsumableTags.Contains(id);

  private void OnWorldInventoryDiscover(Tag category_tag, Tag tag)
  {
    if (!this.undiscoveredConsumableTags.Contains(tag))
      return;
    this.RefreshDiscovered();
  }

  public void RefreshDiscovered(object data = null)
  {
    foreach (EdiblesManager.FoodInfo foodTypes in FOOD.FOOD_TYPES_LIST)
    {
      if (!this.ShouldBeDiscovered(foodTypes.Id.ToTag()) && !this.undiscoveredConsumableTags.Contains(foodTypes.Id.ToTag()))
      {
        this.undiscoveredConsumableTags.Add(foodTypes.Id.ToTag());
        if (this.OnDiscover != null)
          this.OnDiscover("UndiscoveredSomething".ToTag());
      }
      else if (this.undiscoveredConsumableTags.Contains(foodTypes.Id.ToTag()) && this.ShouldBeDiscovered(foodTypes.Id.ToTag()))
      {
        this.undiscoveredConsumableTags.Remove(foodTypes.Id.ToTag());
        if (this.OnDiscover != null)
          this.OnDiscover(foodTypes.Id.ToTag());
        if (!WorldInventory.Instance.IsDiscovered(foodTypes.Id.ToTag()))
        {
          if ((double) foodTypes.CaloriesPerUnit == 0.0)
            WorldInventory.Instance.Discover(foodTypes.Id.ToTag(), GameTags.CookingIngredient);
          else
            WorldInventory.Instance.Discover(foodTypes.Id.ToTag(), GameTags.Edible);
        }
      }
    }
  }

  private bool ShouldBeDiscovered(Tag food_id)
  {
    if (WorldInventory.Instance.IsDiscovered(food_id))
      return true;
    foreach (Recipe recipe in RecipeManager.Get().recipes)
    {
      if (recipe.Result == food_id)
      {
        foreach (string fabricator in recipe.fabricators)
        {
          if (Db.Get().TechItems.IsTechItemComplete(fabricator))
            return true;
        }
      }
    }
    foreach (Crop crop in Components.Crops.Items)
    {
      if (Grid.IsVisible(Grid.PosToCell(crop.gameObject)) && crop.cropId == food_id.Name)
        return true;
    }
    return false;
  }
}
