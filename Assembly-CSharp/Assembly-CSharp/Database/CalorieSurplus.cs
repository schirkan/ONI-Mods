// Decompiled with JetBrains decompiler
// Type: Database.CalorieSurplus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using System.IO;

namespace Database
{
  public class CalorieSurplus : ColonyAchievementRequirement
  {
    private double surplusAmount;

    public CalorieSurplus(float surplusAmount) => this.surplusAmount = (double) surplusAmount;

    public override bool Success() => (double) RationTracker.Get().CountRations((Dictionary<string, float>) null) / 1000.0 >= this.surplusAmount;

    public override bool Fail() => !this.Success();

    public override void Serialize(BinaryWriter writer) => writer.Write(this.surplusAmount);

    public override void Deserialize(IReader reader) => this.surplusAmount = reader.ReadDouble();

    public override string GetProgress(bool complete) => string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CALORIE_SURPLUS, (object) GameUtil.GetFormattedCalories(complete ? (float) this.surplusAmount : RationTracker.Get().CountRations((Dictionary<string, float>) null)), (object) GameUtil.GetFormattedCalories((float) this.surplusAmount));
  }
}
