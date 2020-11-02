// Decompiled with JetBrains decompiler
// Type: Database.TuneUpGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.IO;

namespace Database
{
  public class TuneUpGenerator : ColonyAchievementRequirement
  {
    private float numChoreseToComplete;
    private float choresCompleted;

    public TuneUpGenerator(float numChoreseToComplete) => this.numChoreseToComplete = numChoreseToComplete;

    public override bool Success()
    {
      float num = 0.0f;
      ReportManager.ReportEntry entry1 = ReportManager.Instance.TodaysReport.GetEntry(ReportManager.ReportType.ChoreStatus);
      for (int i = 0; i < entry1.contextEntries.Count; ++i)
      {
        ReportManager.ReportEntry contextEntry = entry1.contextEntries[i];
        if (contextEntry.context == Db.Get().ChoreTypes.PowerTinker.Name)
          num += contextEntry.Negative;
      }
      string name = Db.Get().ChoreTypes.PowerTinker.Name;
      int count1 = ReportManager.Instance.reports.Count;
      for (int index = 0; index < count1; ++index)
      {
        ReportManager.ReportEntry entry2 = ReportManager.Instance.reports[index].GetEntry(ReportManager.ReportType.ChoreStatus);
        int count2 = entry2.contextEntries.Count;
        for (int i = 0; i < count2; ++i)
        {
          ReportManager.ReportEntry contextEntry = entry2.contextEntries[i];
          if (contextEntry.context == name)
            num += contextEntry.Negative;
        }
      }
      this.choresCompleted = Math.Abs(num);
      return (double) Math.Abs(num) >= (double) this.numChoreseToComplete;
    }

    public override void Serialize(BinaryWriter writer) => writer.Write(this.numChoreseToComplete);

    public override void Deserialize(IReader reader) => this.numChoreseToComplete = reader.ReadSingle();

    public override string GetProgress(bool complete) => string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CHORES_OF_TYPE, (object) (float) (complete ? (double) this.numChoreseToComplete : (double) this.choresCompleted), (object) this.numChoreseToComplete, (object) Db.Get().ChoreTypes.PowerTinker.Name);
  }
}
