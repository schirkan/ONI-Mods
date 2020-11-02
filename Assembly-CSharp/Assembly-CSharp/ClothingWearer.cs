﻿// Decompiled with JetBrains decompiler
// Type: ClothingWearer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/ClothingWearer")]
public class ClothingWearer : KMonoBehaviour
{
  private DecorProvider decorProvider;
  private AttributeModifier decorModifier;
  private AttributeModifier conductivityModifier;
  [Serialize]
  public ClothingWearer.ClothingInfo currentClothing;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.decorProvider = this.GetComponent<DecorProvider>();
    if (this.decorModifier == null)
      this.decorModifier = new AttributeModifier("Decor", 0.0f, (string) DUPLICANTS.MODIFIERS.CLOTHING.NAME, is_readonly: false);
    if (this.conductivityModifier != null)
      return;
    AttributeInstance attributeInstance = this.gameObject.GetAttributes().Get("ThermalConductivityBarrier");
    this.conductivityModifier = new AttributeModifier("ThermalConductivityBarrier", ClothingWearer.ClothingInfo.BASIC_CLOTHING.conductivityMod, (string) DUPLICANTS.MODIFIERS.CLOTHING.NAME, is_readonly: false);
    AttributeModifier conductivityModifier = this.conductivityModifier;
    attributeInstance.Add(conductivityModifier);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.decorProvider.decor.Add(this.decorModifier);
    this.decorProvider.decorRadius.Add(new AttributeModifier(Db.Get().BuildingAttributes.DecorRadius.Id, 3f));
    this.decorProvider.overrideName = string.Format((string) UI.OVERLAYS.DECOR.CLOTHING, (object) this.gameObject.GetProperName());
    if (this.currentClothing == null)
      this.ChangeToDefaultClothes();
    else
      this.ChangeClothes(this.currentClothing);
  }

  public void ChangeClothes(ClothingWearer.ClothingInfo clothingInfo)
  {
    this.decorProvider.baseRadius = 3f;
    this.currentClothing = clothingInfo;
    this.conductivityModifier.Description = clothingInfo.name;
    this.conductivityModifier.SetValue(this.currentClothing.conductivityMod);
    this.decorModifier.SetValue((float) this.currentClothing.decorMod);
  }

  public void ChangeToDefaultClothes() => this.ChangeClothes(new ClothingWearer.ClothingInfo(ClothingWearer.ClothingInfo.BASIC_CLOTHING.name, ClothingWearer.ClothingInfo.BASIC_CLOTHING.decorMod, ClothingWearer.ClothingInfo.BASIC_CLOTHING.conductivityMod, ClothingWearer.ClothingInfo.BASIC_CLOTHING.homeostasisEfficiencyMultiplier));

  public class ClothingInfo
  {
    [Serialize]
    public string name = "";
    [Serialize]
    public int decorMod;
    [Serialize]
    public float conductivityMod;
    [Serialize]
    public float homeostasisEfficiencyMultiplier;
    public static readonly ClothingWearer.ClothingInfo UGLY_CLOTHING = new ClothingWearer.ClothingInfo((string) EQUIPMENT.PREFABS.COOL_VEST.GENERICNAME, -30, 1f / 400f, -1.25f);
    public static readonly ClothingWearer.ClothingInfo BASIC_CLOTHING = new ClothingWearer.ClothingInfo((string) EQUIPMENT.PREFABS.COOL_VEST.GENERICNAME, -5, 1f / 400f, -1.25f);
    public static readonly ClothingWearer.ClothingInfo WARM_CLOTHING = new ClothingWearer.ClothingInfo((string) EQUIPMENT.PREFABS.WARM_VEST.NAME, -10, 0.01f, -1.25f);
    public static readonly ClothingWearer.ClothingInfo COOL_CLOTHING = new ClothingWearer.ClothingInfo((string) EQUIPMENT.PREFABS.COOL_VEST.NAME, -10, 0.0005f, 0.0f);
    public static readonly ClothingWearer.ClothingInfo FANCY_CLOTHING = new ClothingWearer.ClothingInfo((string) EQUIPMENT.PREFABS.FUNKY_VEST.NAME, 30, 1f / 400f, -1.25f);

    public ClothingInfo(
      string _name,
      int _decor,
      float _temperature,
      float _homeostasisEfficiencyMultiplier)
    {
      this.name = _name;
      this.decorMod = _decor;
      this.conductivityMod = _temperature;
      this.homeostasisEfficiencyMultiplier = _homeostasisEfficiencyMultiplier;
    }
  }
}