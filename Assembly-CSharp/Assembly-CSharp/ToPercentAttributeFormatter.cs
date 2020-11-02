// Decompiled with JetBrains decompiler
// Type: ToPercentAttributeFormatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

public class ToPercentAttributeFormatter : StandardAttributeFormatter
{
  public float max = 1f;

  public ToPercentAttributeFormatter(float max, GameUtil.TimeSlice deltaTimeSlice = GameUtil.TimeSlice.None)
    : base(GameUtil.UnitClass.Percent, deltaTimeSlice)
    => this.max = max;

  public override string GetFormattedAttribute(AttributeInstance instance) => this.GetFormattedValue(instance.GetTotalDisplayValue(), this.DeltaTimeSlice, instance.gameObject);

  public override string GetFormattedModifier(
    AttributeModifier modifier,
    GameObject parent_instance)
  {
    return this.GetFormattedValue(modifier.Value, this.DeltaTimeSlice, parent_instance);
  }

  public override string GetFormattedValue(
    float value,
    GameUtil.TimeSlice timeSlice,
    GameObject parent_instance)
  {
    return GameUtil.GetFormattedPercent((float) ((double) value / (double) this.max * 100.0), timeSlice);
  }
}
