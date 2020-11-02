// Decompiled with JetBrains decompiler
// Type: Database.CycleNumber
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.IO;

namespace Database
{
  public class CycleNumber : VictoryColonyAchievementRequirement
  {
    private int cycleNumber;

    public override string Name() => string.Format((string) COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_CYCLE, (object) this.cycleNumber);

    public override string Description() => string.Format((string) COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_CYCLE_DESCRIPTION, (object) this.cycleNumber);

    public CycleNumber(int cycleNumber = 100) => this.cycleNumber = cycleNumber;

    public override bool Success() => GameClock.Instance.GetCycle() + 1 >= this.cycleNumber;

    public override void Serialize(BinaryWriter writer) => writer.Write(this.cycleNumber);

    public override void Deserialize(IReader reader) => this.cycleNumber = reader.ReadInt32();

    public override string GetProgress(bool complete) => string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CYCLE_NUMBER, (object) (complete ? this.cycleNumber : GameClock.Instance.GetCycle() + 1), (object) this.cycleNumber);
  }
}
