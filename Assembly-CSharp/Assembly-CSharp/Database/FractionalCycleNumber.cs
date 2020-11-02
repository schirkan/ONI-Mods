// Decompiled with JetBrains decompiler
// Type: Database.FractionalCycleNumber
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.IO;

namespace Database
{
  public class FractionalCycleNumber : ColonyAchievementRequirement
  {
    private float fractionalCycleNumber;

    public FractionalCycleNumber(float fractionalCycleNumber) => this.fractionalCycleNumber = fractionalCycleNumber;

    public override bool Success()
    {
      int fractionalCycleNumber = (int) this.fractionalCycleNumber;
      float num = this.fractionalCycleNumber - (float) fractionalCycleNumber;
      if ((double) (GameClock.Instance.GetCycle() + 1) > (double) this.fractionalCycleNumber)
        return true;
      return GameClock.Instance.GetCycle() + 1 == fractionalCycleNumber && (double) GameClock.Instance.GetCurrentCycleAsPercentage() >= (double) num;
    }

    public override void Serialize(BinaryWriter writer) => writer.Write(this.fractionalCycleNumber);

    public override void Deserialize(IReader reader) => this.fractionalCycleNumber = reader.ReadSingle();

    public override string GetProgress(bool complete)
    {
      float num = (float) GameClock.Instance.GetCycle() + GameClock.Instance.GetCurrentCycleAsPercentage();
      return string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.FRACTIONAL_CYCLE, (object) (float) (complete ? (double) this.fractionalCycleNumber : (double) num), (object) this.fractionalCycleNumber);
    }
  }
}
