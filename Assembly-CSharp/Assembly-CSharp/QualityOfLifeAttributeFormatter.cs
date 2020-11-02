﻿// Decompiled with JetBrains decompiler
// Type: QualityOfLifeAttributeFormatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;

public class QualityOfLifeAttributeFormatter : StandardAttributeFormatter
{
  public QualityOfLifeAttributeFormatter()
    : base(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None)
  {
  }

  public override string GetFormattedAttribute(AttributeInstance instance)
  {
    AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(instance.gameObject);
    return string.Format((string) DUPLICANTS.ATTRIBUTES.QUALITYOFLIFE.DESC_FORMAT, (object) this.GetFormattedValue(instance.GetTotalDisplayValue(), parent_instance: instance.gameObject), (object) this.GetFormattedValue(attributeInstance.GetTotalDisplayValue(), parent_instance: instance.gameObject));
  }

  public override string GetTooltip(Attribute master, AttributeInstance instance)
  {
    string tooltip = base.GetTooltip(master, instance);
    AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(instance.gameObject);
    string str = tooltip + "\n\n" + string.Format((string) DUPLICANTS.ATTRIBUTES.QUALITYOFLIFE.TOOLTIP_EXPECTATION, (object) this.GetFormattedValue(attributeInstance.GetTotalDisplayValue(), parent_instance: instance.gameObject));
    return (double) instance.GetTotalDisplayValue() - (double) attributeInstance.GetTotalDisplayValue() < 0.0 ? str + "\n\n" + (string) DUPLICANTS.ATTRIBUTES.QUALITYOFLIFE.TOOLTIP_EXPECTATION_UNDER : str + "\n\n" + (string) DUPLICANTS.ATTRIBUTES.QUALITYOFLIFE.TOOLTIP_EXPECTATION_OVER;
  }
}
