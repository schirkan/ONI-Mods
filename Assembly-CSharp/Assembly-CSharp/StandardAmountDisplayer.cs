// Decompiled with JetBrains decompiler
// Type: StandardAmountDisplayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using UnityEngine;

public class StandardAmountDisplayer : IAmountDisplayer
{
  protected StandardAttributeFormatter formatter;

  public IAttributeFormatter Formatter => (IAttributeFormatter) this.formatter;

  public GameUtil.TimeSlice DeltaTimeSlice
  {
    get => this.formatter.DeltaTimeSlice;
    set => this.formatter.DeltaTimeSlice = value;
  }

  public StandardAmountDisplayer(
    GameUtil.UnitClass unitClass,
    GameUtil.TimeSlice deltaTimeSlice,
    StandardAttributeFormatter formatter = null)
  {
    if (formatter != null)
      this.formatter = formatter;
    else
      this.formatter = new StandardAttributeFormatter(unitClass, deltaTimeSlice);
  }

  public virtual string GetValueString(Amount master, AmountInstance instance) => !master.showMax ? this.formatter.GetFormattedValue(instance.value, parent_instance: instance.gameObject) : string.Format("{0} / {1}", (object) this.formatter.GetFormattedValue(instance.value), (object) this.formatter.GetFormattedValue(instance.GetMax()));

  public virtual string GetDescription(Amount master, AmountInstance instance) => string.Format("{0}: {1}", (object) master.Name, (object) this.GetValueString(master, instance));

  public virtual string GetTooltip(Amount master, AmountInstance instance)
  {
    string str1 = "";
    string str2 = (master.description.IndexOf("{1}") <= -1 ? str1 + string.Format(master.description, (object) this.formatter.GetFormattedValue(instance.value)) : str1 + string.Format(master.description, (object) this.formatter.GetFormattedValue(instance.value), (object) GameUtil.GetIdentityDescriptor(instance.gameObject))) + "\n\n";
    if (this.formatter.DeltaTimeSlice == GameUtil.TimeSlice.PerCycle)
      str2 += string.Format((string) UI.CHANGEPERCYCLE, (object) this.formatter.GetFormattedValue(instance.deltaAttribute.GetTotalDisplayValue(), GameUtil.TimeSlice.PerCycle));
    else if (this.formatter.DeltaTimeSlice == GameUtil.TimeSlice.PerSecond)
      str2 += string.Format((string) UI.CHANGEPERSECOND, (object) this.formatter.GetFormattedValue(instance.deltaAttribute.GetTotalDisplayValue(), GameUtil.TimeSlice.PerSecond));
    for (int i = 0; i != instance.deltaAttribute.Modifiers.Count; ++i)
    {
      AttributeModifier modifier = instance.deltaAttribute.Modifiers[i];
      str2 = str2 + "\n" + string.Format((string) UI.MODIFIER_ITEM_TEMPLATE, (object) modifier.GetDescription(), (object) this.formatter.GetFormattedModifier(modifier, instance.gameObject));
    }
    return str2;
  }

  public string GetFormattedAttribute(AttributeInstance instance) => this.formatter.GetFormattedAttribute(instance);

  public string GetFormattedModifier(AttributeModifier modifier, GameObject parent_instance) => this.formatter.GetFormattedModifier(modifier, parent_instance);

  public string GetFormattedValue(
    float value,
    GameUtil.TimeSlice time_slice,
    GameObject parent_instance)
  {
    return this.formatter.GetFormattedValue(value, time_slice, parent_instance);
  }
}
