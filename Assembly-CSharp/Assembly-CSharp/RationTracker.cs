﻿// Decompiled with JetBrains decompiler
// Type: RationTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/RationTracker")]
public class RationTracker : KMonoBehaviour, ISaveLoadable
{
  private static RationTracker instance;
  [Serialize]
  public RationTracker.Frame currentFrame;
  [Serialize]
  public RationTracker.Frame previousFrame;
  [Serialize]
  public Dictionary<string, float> caloriesConsumedByFood = new Dictionary<string, float>();
  private static readonly EventSystem.IntraObjectHandler<RationTracker> OnNewDayDelegate = new EventSystem.IntraObjectHandler<RationTracker>((System.Action<RationTracker, object>) ((component, data) => component.OnNewDay(data)));

  public static void DestroyInstance() => RationTracker.instance = (RationTracker) null;

  public static RationTracker Get() => RationTracker.instance;

  protected override void OnPrefabInit() => RationTracker.instance = this;

  protected override void OnSpawn() => this.Subscribe<RationTracker>(631075836, RationTracker.OnNewDayDelegate);

  private void OnNewDay(object data)
  {
    this.previousFrame = this.currentFrame;
    this.currentFrame = new RationTracker.Frame();
  }

  public float CountRations(Dictionary<string, float> unitCountByFoodType, bool excludeUnreachable = true)
  {
    float num = 0.0f;
    ICollection<Pickupable> pickupables = WorldInventory.Instance.GetPickupables(GameTags.Edible);
    if (pickupables != null)
    {
      foreach (Pickupable pickupable in (IEnumerable<Pickupable>) pickupables)
      {
        if (!pickupable.KPrefabID.HasTag(GameTags.StoredPrivate))
        {
          Edible component = pickupable.GetComponent<Edible>();
          num += component.Calories;
          if (unitCountByFoodType != null)
          {
            if (!unitCountByFoodType.ContainsKey(component.FoodID))
              unitCountByFoodType[component.FoodID] = 0.0f;
            unitCountByFoodType[component.FoodID] += component.Units;
          }
        }
      }
    }
    return num;
  }

  public float CountRationsByFoodType(string foodID, bool excludeUnreachable = true)
  {
    float num = 0.0f;
    ICollection<Pickupable> pickupables = WorldInventory.Instance.GetPickupables(GameTags.Edible);
    if (pickupables != null)
    {
      foreach (Pickupable pickupable in (IEnumerable<Pickupable>) pickupables)
      {
        if (!pickupable.KPrefabID.HasTag(GameTags.StoredPrivate))
        {
          Edible component = pickupable.GetComponent<Edible>();
          if (component.FoodID == foodID)
            num += component.Calories;
        }
      }
    }
    return num;
  }

  public void RegisterCaloriesProduced(float calories) => this.currentFrame.caloriesProduced += calories;

  public void RegisterRationsConsumed(Edible edible)
  {
    this.currentFrame.caloriesConsumed += edible.caloriesConsumed;
    if (!this.caloriesConsumedByFood.ContainsKey(edible.FoodInfo.Id))
      this.caloriesConsumedByFood.Add(edible.FoodInfo.Id, edible.caloriesConsumed);
    else
      this.caloriesConsumedByFood[edible.FoodInfo.Id] += edible.caloriesConsumed;
  }

  public float GetCaloiresConsumedByFood(List<string> foodTypes)
  {
    float num = 0.0f;
    foreach (string foodType in foodTypes)
    {
      if (this.caloriesConsumedByFood.ContainsKey(foodType))
        num += this.caloriesConsumedByFood[foodType];
    }
    return num;
  }

  public float GetCaloriesConsumed()
  {
    float num = 0.0f;
    foreach (KeyValuePair<string, float> keyValuePair in this.caloriesConsumedByFood)
      num += keyValuePair.Value;
    return num;
  }

  public struct Frame
  {
    public float caloriesProduced;
    public float caloriesConsumed;
  }
}
