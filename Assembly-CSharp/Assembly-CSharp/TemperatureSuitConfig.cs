﻿// Decompiled with JetBrains decompiler
// Type: TemperatureSuitConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureSuitConfig : IEquipmentConfig
{
  public const string ID = "Temperature_Suit";

  public EquipmentDef CreateEquipmentDef()
  {
    new Dictionary<string, float>()
    {
      {
        SimHashes.Ice.ToString(),
        300f
      }
    };
    EquipmentDef equipmentDef = EquipmentTemplates.CreateEquipmentDef("Temperature_Suit", TUNING.EQUIPMENT.SUITS.SLOT, SimHashes.Water, (float) TUNING.EQUIPMENT.SUITS.TEMPERATURESUIT_MASS, TUNING.EQUIPMENT.SUITS.ANIM, TUNING.EQUIPMENT.SUITS.SNAPON, "body_oxygen_kanim", 6, new List<AttributeModifier>()
    {
      new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.INSULATION, TUNING.EQUIPMENT.SUITS.TEMPERATURESUIT_INSULATION, (string) STRINGS.EQUIPMENT.PREFABS.TEMPERATURE_SUIT.NAME),
      new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.ATHLETICS, (float) TUNING.EQUIPMENT.SUITS.TEMPERATURESUIT_ATHLETICS, (string) STRINGS.EQUIPMENT.PREFABS.TEMPERATURE_SUIT.NAME)
    }, additional_tags: new Tag[1]{ GameTags.Suit });
    equipmentDef.RecipeDescription = (string) STRINGS.EQUIPMENT.PREFABS.TEMPERATURE_SUIT.RECIPE_DESC;
    return equipmentDef;
  }

  public void DoPostConfigure(GameObject go)
  {
    SuitTank suitTank = go.AddComponent<SuitTank>();
    suitTank.element = "Water";
    suitTank.amount = 100f;
    go.GetComponent<KPrefabID>().AddTag(GameTags.PedestalDisplayable);
  }
}
