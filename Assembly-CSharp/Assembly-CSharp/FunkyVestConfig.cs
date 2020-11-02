﻿// Decompiled with JetBrains decompiler
// Type: FunkyVestConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class FunkyVestConfig : IEquipmentConfig
{
  public const string ID = "Funky_Vest";
  public static ComplexRecipe recipe;

  public EquipmentDef CreateEquipmentDef()
  {
    new Dictionary<string, float>()
    {
      {
        "BasicFabric",
        (float) TUNING.EQUIPMENT.VESTS.FUNKY_VEST_MASS
      }
    };
    ClothingWearer.ClothingInfo clothingInfo = ClothingWearer.ClothingInfo.FANCY_CLOTHING;
    List<AttributeModifier> AttributeModifiers = new List<AttributeModifier>();
    EquipmentDef equipmentDef = EquipmentTemplates.CreateEquipmentDef("Funky_Vest", TUNING.EQUIPMENT.CLOTHING.SLOT, SimHashes.Carbon, (float) TUNING.EQUIPMENT.VESTS.FUNKY_VEST_MASS, TUNING.EQUIPMENT.VESTS.FUNKY_VEST_ICON0, TUNING.EQUIPMENT.VESTS.SNAPON0, TUNING.EQUIPMENT.VESTS.FUNKY_VEST_ANIM0, 4, AttributeModifiers, TUNING.EQUIPMENT.VESTS.SNAPON1, true, EntityTemplates.CollisionShape.RECTANGLE, 0.75f, 0.4f);
    Descriptor descriptor1 = new Descriptor(string.Format("{0}: {1}", (object) DUPLICANTS.ATTRIBUTES.THERMALCONDUCTIVITYBARRIER.NAME, (object) GameUtil.GetFormattedDistance(ClothingWearer.ClothingInfo.FANCY_CLOTHING.conductivityMod)), string.Format("{0}: {1}", (object) DUPLICANTS.ATTRIBUTES.THERMALCONDUCTIVITYBARRIER.NAME, (object) GameUtil.GetFormattedDistance(ClothingWearer.ClothingInfo.FANCY_CLOTHING.conductivityMod)));
    Descriptor descriptor2 = new Descriptor(string.Format("{0}: {1}", (object) DUPLICANTS.ATTRIBUTES.DECOR.NAME, (object) ClothingWearer.ClothingInfo.FANCY_CLOTHING.decorMod), string.Format("{0}: {1}", (object) DUPLICANTS.ATTRIBUTES.DECOR.NAME, (object) ClothingWearer.ClothingInfo.FANCY_CLOTHING.decorMod));
    equipmentDef.additionalDescriptors.Add(descriptor1);
    equipmentDef.additionalDescriptors.Add(descriptor2);
    equipmentDef.OnEquipCallBack = (System.Action<Equippable>) (eq => CoolVestConfig.OnEquipVest(eq, clothingInfo));
    equipmentDef.OnUnequipCallBack = new System.Action<Equippable>(CoolVestConfig.OnUnequipVest);
    equipmentDef.RecipeDescription = (string) STRINGS.EQUIPMENT.PREFABS.FUNKY_VEST.RECIPE_DESC;
    return equipmentDef;
  }

  public static void SetupVest(GameObject go)
  {
    go.GetComponent<KPrefabID>().AddTag(GameTags.Clothes);
    Equippable equippable = go.GetComponent<Equippable>();
    if ((UnityEngine.Object) equippable == (UnityEngine.Object) null)
      equippable = go.AddComponent<Equippable>();
    equippable.SetQuality(QualityLevel.Poor);
    go.GetComponent<KBatchedAnimController>().sceneLayer = Grid.SceneLayer.BuildingBack;
  }

  public void DoPostConfigure(GameObject go)
  {
    FunkyVestConfig.SetupVest(go);
    go.GetComponent<KPrefabID>().AddTag(GameTags.PedestalDisplayable);
  }
}