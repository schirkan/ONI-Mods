// Decompiled with JetBrains decompiler
// Type: TUNING.STORAGEFILTERS
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

namespace TUNING
{
  public class STORAGEFILTERS
  {
    public static List<Tag> FOOD = new List<Tag>()
    {
      GameTags.Edible,
      GameTags.CookingIngredient,
      GameTags.Medicine
    };
    public static List<Tag> BAGABLE_CREATURES = new List<Tag>()
    {
      GameTags.BagableCreature
    };
    public static List<Tag> SWIMMING_CREATURES = new List<Tag>()
    {
      GameTags.SwimmingCreature
    };
    public static List<Tag> NOT_EDIBLE_SOLIDS = new List<Tag>()
    {
      GameTags.Alloy,
      GameTags.RefinedMetal,
      GameTags.Metal,
      GameTags.BuildableRaw,
      GameTags.BuildableProcessed,
      GameTags.Farmable,
      GameTags.Organics,
      GameTags.Compostable,
      GameTags.Seed,
      GameTags.Agriculture,
      GameTags.Filter,
      GameTags.ConsumableOre,
      GameTags.Liquifiable,
      GameTags.IndustrialProduct,
      GameTags.IndustrialIngredient,
      GameTags.MedicalSupplies,
      GameTags.Clothes,
      GameTags.ManufacturedMaterial,
      GameTags.Egg,
      GameTags.RareMaterials,
      GameTags.Other
    };
    public static List<Tag> LIQUIDS = new List<Tag>()
    {
      GameTags.Liquid
    };
    public static List<Tag> GASES = new List<Tag>()
    {
      GameTags.Breathable,
      GameTags.Unbreathable
    };
  }
}
