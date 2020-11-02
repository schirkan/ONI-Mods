// Decompiled with JetBrains decompiler
// Type: Database.EatXCalories
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.IO;

namespace Database
{
  public class EatXCalories : ColonyAchievementRequirement
  {
    private int numCalories;

    public EatXCalories(int numCalories) => this.numCalories = numCalories;

    public override bool Success() => (double) RationTracker.Get().GetCaloriesConsumed() / 1000.0 > (double) this.numCalories;

    public override void Deserialize(IReader reader) => this.numCalories = reader.ReadInt32();

    public override void Serialize(BinaryWriter writer) => writer.Write(this.numCalories);

    public override string GetProgress(bool complete) => string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CONSUME_CALORIES, (object) GameUtil.GetFormattedCalories(complete ? (float) this.numCalories * 1000f : RationTracker.Get().GetCaloriesConsumed()), (object) GameUtil.GetFormattedCalories((float) this.numCalories * 1000f));
  }
}
