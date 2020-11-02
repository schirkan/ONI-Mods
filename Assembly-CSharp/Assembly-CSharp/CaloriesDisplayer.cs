// Decompiled with JetBrains decompiler
// Type: CaloriesDisplayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

public class CaloriesDisplayer : StandardAmountDisplayer
{
  public CaloriesDisplayer()
    : base(GameUtil.UnitClass.Calories, GameUtil.TimeSlice.PerCycle)
    => this.formatter = (StandardAttributeFormatter) new CaloriesDisplayer.CaloriesAttributeFormatter();

  public class CaloriesAttributeFormatter : StandardAttributeFormatter
  {
    public CaloriesAttributeFormatter()
      : base(GameUtil.UnitClass.Calories, GameUtil.TimeSlice.PerCycle)
    {
    }

    public override string GetFormattedModifier(
      AttributeModifier modifier,
      GameObject parent_instance)
    {
      return modifier.IsMultiplier ? GameUtil.GetFormattedPercent((float) (-(double) modifier.Value * 100.0)) : base.GetFormattedModifier(modifier, parent_instance);
    }

    public override string GetTooltip(Attribute master, AttributeInstance instance) => "TEST";

    public override string GetTooltipDescription(Attribute master, AttributeInstance instance) => "TEST";
  }
}
