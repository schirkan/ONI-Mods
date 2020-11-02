// Decompiled with JetBrains decompiler
// Type: AsPercentAmountDisplayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using UnityEngine;

public class AsPercentAmountDisplayer : IAmountDisplayer
{
  protected StandardAttributeFormatter formatter;

  public IAttributeFormatter Formatter => (IAttributeFormatter) this.formatter;

  public GameUtil.TimeSlice DeltaTimeSlice
  {
    get => this.formatter.DeltaTimeSlice;
    set => this.formatter.DeltaTimeSlice = value;
  }

  public AsPercentAmountDisplayer(GameUtil.TimeSlice deltaTimeSlice) => this.formatter = new StandardAttributeFormatter(GameUtil.UnitClass.Percent, deltaTimeSlice);

  public string GetValueString(Amount master, AmountInstance instance) => this.formatter.GetFormattedValue(this.ToPercent(instance.value, instance));

  public virtual string GetDescription(Amount master, AmountInstance instance) => string.Format("{0}: {1}", (object) master.Name, (object) this.formatter.GetFormattedValue(this.ToPercent(instance.value, instance)));

  public virtual string GetTooltipDescription(Amount master, AmountInstance instance) => string.Format(master.description, (object) this.formatter.GetFormattedValue(instance.value));

  public virtual string GetTooltip(Amount master, AmountInstance instance)
  {
    string str1 = string.Format(master.description, (object) this.formatter.GetFormattedValue(instance.value)) + "\n\n";
    string str2 = this.formatter.DeltaTimeSlice != GameUtil.TimeSlice.PerCycle ? str1 + string.Format((string) UI.CHANGEPERSECOND, (object) this.formatter.GetFormattedValue(this.ToPercent(instance.deltaAttribute.GetTotalDisplayValue(), instance), GameUtil.TimeSlice.PerSecond)) : str1 + string.Format((string) UI.CHANGEPERCYCLE, (object) this.formatter.GetFormattedValue(this.ToPercent(instance.deltaAttribute.GetTotalDisplayValue(), instance), GameUtil.TimeSlice.PerCycle));
    for (int i = 0; i != instance.deltaAttribute.Modifiers.Count; ++i)
    {
      AttributeModifier modifier = instance.deltaAttribute.Modifiers[i];
      float modifierContribution = instance.deltaAttribute.GetModifierContribution(modifier);
      str2 = str2 + "\n" + string.Format((string) UI.MODIFIER_ITEM_TEMPLATE, (object) modifier.GetDescription(), (object) this.formatter.GetFormattedValue(this.ToPercent(modifierContribution, instance), this.formatter.DeltaTimeSlice));
    }
    return str2;
  }

  public string GetFormattedAttribute(AttributeInstance instance) => this.formatter.GetFormattedAttribute(instance);

  public string GetFormattedModifier(AttributeModifier modifier, GameObject parent_instance) => modifier.IsMultiplier ? GameUtil.GetFormattedPercent(modifier.Value * 100f) : this.formatter.GetFormattedModifier(modifier, parent_instance);

  public string GetFormattedValue(
    float value,
    GameUtil.TimeSlice timeSlice,
    GameObject parent_instance)
  {
    return this.formatter.GetFormattedValue(value, timeSlice, parent_instance);
  }

  protected float ToPercent(float value, AmountInstance instance) => 100f * value / instance.GetMax();
}
