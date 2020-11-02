// Decompiled with JetBrains decompiler
// Type: AquaSuitConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

public class AquaSuitConfig : IEquipmentConfig
{
  public const string ID = "Aqua_Suit";

  public EquipmentDef CreateEquipmentDef()
  {
    new Dictionary<string, float>()
    {
      {
        SimHashes.DirtyWater.ToString(),
        300f
      }
    };
    EquipmentDef equipmentDef = EquipmentTemplates.CreateEquipmentDef("Aqua_Suit", TUNING.EQUIPMENT.SUITS.SLOT, SimHashes.Water, (float) TUNING.EQUIPMENT.SUITS.AQUASUIT_MASS, "suit_water_slow_kanim", TUNING.EQUIPMENT.SUITS.SNAPON, "body_water_slow_kanim", 6, new List<AttributeModifier>()
    {
      new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.INSULATION, (float) TUNING.EQUIPMENT.SUITS.AQUASUIT_INSULATION, (string) STRINGS.EQUIPMENT.PREFABS.AQUA_SUIT.NAME),
      new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.ATHLETICS, (float) TUNING.EQUIPMENT.SUITS.AQUASUIT_ATHLETICS, (string) STRINGS.EQUIPMENT.PREFABS.AQUA_SUIT.NAME),
      new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.MAX_UNDERWATER_TRAVELCOST, (float) TUNING.EQUIPMENT.SUITS.AQUASUIT_UNDERWATER_TRAVELCOST, (string) STRINGS.EQUIPMENT.PREFABS.AQUA_SUIT.NAME)
    }, additional_tags: new Tag[1]{ GameTags.Suit });
    equipmentDef.RecipeDescription = (string) STRINGS.EQUIPMENT.PREFABS.AQUA_SUIT.RECIPE_DESC;
    return equipmentDef;
  }

  public void DoPostConfigure(GameObject go)
  {
    SuitTank suitTank = go.AddComponent<SuitTank>();
    suitTank.underwaterSupport = true;
    suitTank.element = "Oxygen";
    suitTank.amount = 11f;
    go.GetComponent<KPrefabID>().AddTag(GameTags.Clothes);
  }
}
