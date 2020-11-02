// Decompiled with JetBrains decompiler
// Type: DecorDisplayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;

public class DecorDisplayer : StandardAmountDisplayer
{
  public DecorDisplayer()
    : base(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.PerCycle)
    => this.formatter = (StandardAttributeFormatter) new DecorDisplayer.DecorAttributeFormatter();

  public override string GetTooltip(Amount master, AmountInstance instance)
  {
    string str1 = string.Format(master.description, (object) this.formatter.GetFormattedValue(instance.value));
    int cell = Grid.PosToCell(instance.gameObject);
    if (Grid.IsValidCell(cell))
      str1 += string.Format((string) DUPLICANTS.STATS.DECOR.TOOLTIP_CURRENT, (object) GameUtil.GetDecorAtCell(cell));
    string str2 = str1 + "\n";
    DecorMonitor.Instance smi = instance.gameObject.GetSMI<DecorMonitor.Instance>();
    if (smi != null)
      str2 = str2 + string.Format((string) DUPLICANTS.STATS.DECOR.TOOLTIP_AVERAGE_TODAY, (object) this.formatter.GetFormattedValue(smi.GetTodaysAverageDecor())) + string.Format((string) DUPLICANTS.STATS.DECOR.TOOLTIP_AVERAGE_YESTERDAY, (object) this.formatter.GetFormattedValue(smi.GetYesterdaysAverageDecor()));
    return str2;
  }

  public class DecorAttributeFormatter : StandardAttributeFormatter
  {
    public DecorAttributeFormatter()
      : base(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.PerCycle)
    {
    }
  }
}
