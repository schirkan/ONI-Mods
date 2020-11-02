// Decompiled with JetBrains decompiler
// Type: StandardAttributeFormatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class StandardAttributeFormatter : IAttributeFormatter
{
  public GameUtil.UnitClass unitClass;

  public GameUtil.TimeSlice DeltaTimeSlice { get; set; }

  public StandardAttributeFormatter(GameUtil.UnitClass unitClass, GameUtil.TimeSlice deltaTimeSlice)
  {
    this.unitClass = unitClass;
    this.DeltaTimeSlice = deltaTimeSlice;
  }

  public virtual string GetFormattedAttribute(AttributeInstance instance) => this.GetFormattedValue(instance.GetTotalDisplayValue());

  public virtual string GetFormattedModifier(AttributeModifier modifier, GameObject parent_instance) => this.GetFormattedValue(modifier.Value, this.DeltaTimeSlice);

  public virtual string GetFormattedValue(
    float value,
    GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None,
    GameObject parent_instance = null)
  {
    switch (this.unitClass)
    {
      case GameUtil.UnitClass.SimpleInteger:
        return GameUtil.GetFormattedInt(value, timeSlice);
      case GameUtil.UnitClass.Temperature:
        return GameUtil.GetFormattedTemperature(value, timeSlice, timeSlice == GameUtil.TimeSlice.None ? GameUtil.TemperatureInterpretation.Absolute : GameUtil.TemperatureInterpretation.Relative);
      case GameUtil.UnitClass.Mass:
        return GameUtil.GetFormattedMass(value, timeSlice);
      case GameUtil.UnitClass.Calories:
        return GameUtil.GetFormattedCalories(value, timeSlice);
      case GameUtil.UnitClass.Percent:
        return GameUtil.GetFormattedPercent(value, timeSlice);
      case GameUtil.UnitClass.Distance:
        return GameUtil.GetFormattedDistance(value);
      case GameUtil.UnitClass.Disease:
        return GameUtil.GetFormattedDiseaseAmount(Mathf.RoundToInt(value));
      default:
        return GameUtil.GetFormattedSimple(value, timeSlice);
    }
  }

  public virtual string GetTooltipDescription(Klei.AI.Attribute master, AttributeInstance instance) => master.Description;

  public virtual string GetTooltip(Klei.AI.Attribute master, AttributeInstance instance)
  {
    string str1 = this.GetTooltipDescription(master, instance) + string.Format((string) DUPLICANTS.ATTRIBUTES.TOTAL_VALUE, (object) this.GetFormattedValue(instance.GetTotalDisplayValue()), (object) instance.Name);
    if ((double) instance.GetBaseValue() != 0.0)
      str1 += string.Format((string) DUPLICANTS.ATTRIBUTES.BASE_VALUE, (object) instance.GetBaseValue());
    List<AttributeModifier> attributeModifierList = new List<AttributeModifier>();
    for (int i = 0; i < instance.Modifiers.Count; ++i)
      attributeModifierList.Add(instance.Modifiers[i]);
    attributeModifierList.Sort((Comparison<AttributeModifier>) ((p1, p2) => p2.Value.CompareTo(p1.Value)));
    for (int index = 0; index != attributeModifierList.Count; ++index)
    {
      AttributeModifier attributeModifier = attributeModifierList[index];
      string formattedString = attributeModifier.GetFormattedString(instance.gameObject);
      if (formattedString != null)
        str1 += string.Format((string) DUPLICANTS.ATTRIBUTES.MODIFIER_ENTRY, (object) attributeModifier.GetDescription(), (object) formattedString);
    }
    string str2 = "";
    AttributeConverters component = instance.gameObject.GetComponent<AttributeConverters>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null && master.converters.Count > 0)
    {
      foreach (AttributeConverterInstance converter in component.converters)
      {
        if (converter.converter.attribute == master)
        {
          string str3 = converter.DescriptionFromAttribute(converter.Evaluate(), converter.gameObject);
          if (str3 != null)
            str2 = str2 + "\n" + str3;
        }
      }
    }
    if (str2.Length > 0)
      str1 = str1 + "\n" + str2;
    return str1;
  }
}
