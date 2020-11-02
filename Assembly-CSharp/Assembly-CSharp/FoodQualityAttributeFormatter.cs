// Decompiled with JetBrains decompiler
// Type: FoodQualityAttributeFormatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

public class FoodQualityAttributeFormatter : StandardAttributeFormatter
{
  public FoodQualityAttributeFormatter()
    : base(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None)
  {
  }

  public override string GetFormattedAttribute(AttributeInstance instance) => this.GetFormattedValue(instance.GetTotalDisplayValue(), parent_instance: instance.gameObject);

  public override string GetFormattedModifier(
    AttributeModifier modifier,
    GameObject parent_instance)
  {
    return GameUtil.GetFormattedInt(modifier.Value);
  }

  public override string GetFormattedValue(
    float value,
    GameUtil.TimeSlice timeSlice,
    GameObject parent_instance)
  {
    return Util.StripTextFormatting(GameUtil.GetFormattedFoodQuality((int) value));
  }
}
