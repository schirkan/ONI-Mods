// Decompiled with JetBrains decompiler
// Type: DuplicantTemperatureDeltaAsEnergyAmountDisplayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;

public class DuplicantTemperatureDeltaAsEnergyAmountDisplayer : StandardAmountDisplayer
{
  public DuplicantTemperatureDeltaAsEnergyAmountDisplayer(
    GameUtil.UnitClass unitClass,
    GameUtil.TimeSlice timeSlice)
    : base(unitClass, timeSlice)
  {
  }

  public override string GetTooltip(Amount master, AmountInstance instance)
  {
    string str1 = string.Format(master.description, (object) this.formatter.GetFormattedValue(instance.value), (object) this.formatter.GetFormattedValue(310.15f));
    float num = (float) ((double) ElementLoader.FindElementByHash(SimHashes.Creature).specificHeatCapacity * 30.0 * 1000.0);
    string str2 = str1 + "\n\n";
    string str3 = this.formatter.DeltaTimeSlice != GameUtil.TimeSlice.PerCycle ? str2 + string.Format((string) UI.CHANGEPERSECOND, (object) this.formatter.GetFormattedValue(instance.deltaAttribute.GetTotalDisplayValue(), GameUtil.TimeSlice.PerSecond)) + "\n" + string.Format((string) UI.CHANGEPERSECOND, (object) GameUtil.GetFormattedJoules(instance.deltaAttribute.GetTotalDisplayValue() * num)) : str2 + string.Format((string) UI.CHANGEPERCYCLE, (object) this.formatter.GetFormattedValue(instance.deltaAttribute.GetTotalDisplayValue(), GameUtil.TimeSlice.PerCycle));
    for (int i = 0; i != instance.deltaAttribute.Modifiers.Count; ++i)
    {
      AttributeModifier modifier = instance.deltaAttribute.Modifiers[i];
      str3 = str3 + "\n" + string.Format((string) UI.MODIFIER_ITEM_TEMPLATE, (object) modifier.GetDescription(), (object) GameUtil.GetFormattedHeatEnergyRate((float) ((double) modifier.Value * (double) num * 1.0)));
    }
    return str3;
  }
}
