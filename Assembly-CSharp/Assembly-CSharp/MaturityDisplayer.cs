// Decompiled with JetBrains decompiler
// Type: MaturityDisplayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using UnityEngine;

public class MaturityDisplayer : AsPercentAmountDisplayer
{
  public MaturityDisplayer()
    : base(GameUtil.TimeSlice.PerCycle)
    => this.formatter = (StandardAttributeFormatter) new MaturityDisplayer.MaturityAttributeFormatter();

  public override string GetTooltipDescription(Amount master, AmountInstance instance)
  {
    string tooltipDescription = base.GetTooltipDescription(master, instance);
    Growing component = instance.gameObject.GetComponent<Growing>();
    string str;
    if (component.IsGrowing())
    {
      float seconds = (instance.GetMax() - instance.value) / instance.GetDelta();
      str = !((Object) component != (Object) null) || !component.IsGrowing() ? tooltipDescription + string.Format((string) CREATURES.STATS.MATURITY.TOOLTIP_GROWING, (object) GameUtil.GetFormattedCycles(seconds)) : tooltipDescription + string.Format((string) CREATURES.STATS.MATURITY.TOOLTIP_GROWING_CROP, (object) GameUtil.GetFormattedCycles(seconds), (object) GameUtil.GetFormattedCycles(component.TimeUntilNextHarvest()));
    }
    else
      str = !component.ReachedNextHarvest() ? tooltipDescription + (string) CREATURES.STATS.MATURITY.TOOLTIP_STALLED : tooltipDescription + (string) CREATURES.STATS.MATURITY.TOOLTIP_GROWN;
    return str;
  }

  public override string GetDescription(Amount master, AmountInstance instance)
  {
    Growing component = instance.gameObject.GetComponent<Growing>();
    return (Object) component != (Object) null && component.IsGrowing() ? string.Format((string) CREATURES.STATS.MATURITY.AMOUNT_DESC_FMT, (object) master.Name, (object) this.formatter.GetFormattedValue(this.ToPercent(instance.value, instance)), (object) GameUtil.GetFormattedCycles(component.TimeUntilNextHarvest())) : base.GetDescription(master, instance);
  }

  public class MaturityAttributeFormatter : StandardAttributeFormatter
  {
    public MaturityAttributeFormatter()
      : base(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.None)
    {
    }

    public override string GetFormattedModifier(
      AttributeModifier modifier,
      GameObject parent_instance)
    {
      float num = modifier.Value;
      GameUtil.TimeSlice timeSlice = this.DeltaTimeSlice;
      if (modifier.IsMultiplier)
      {
        num *= 100f;
        timeSlice = GameUtil.TimeSlice.None;
      }
      return this.GetFormattedValue(num, timeSlice, parent_instance);
    }
  }
}
