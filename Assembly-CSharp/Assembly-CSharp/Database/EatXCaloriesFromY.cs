// Decompiled with JetBrains decompiler
// Type: Database.EatXCaloriesFromY
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System.Collections.Generic;
using System.IO;

namespace Database
{
  public class EatXCaloriesFromY : ColonyAchievementRequirement
  {
    private int numCalories;
    private List<string> fromFoodType = new List<string>();

    public EatXCaloriesFromY(int numCalories, List<string> fromFoodType)
    {
      this.numCalories = numCalories;
      this.fromFoodType = fromFoodType;
    }

    public override bool Success() => (double) RationTracker.Get().GetCaloiresConsumedByFood(this.fromFoodType) / 1000.0 > (double) this.numCalories;

    public override void Deserialize(IReader reader)
    {
      this.numCalories = reader.ReadInt32();
      int capacity = reader.ReadInt32();
      this.fromFoodType = new List<string>(capacity);
      for (int index = 0; index < capacity; ++index)
        this.fromFoodType.Add(reader.ReadKleiString());
    }

    public override void Serialize(BinaryWriter writer)
    {
      writer.Write(this.numCalories);
      writer.Write(this.fromFoodType.Count);
      for (int index = 0; index < this.fromFoodType.Count; ++index)
        writer.WriteKleiString(this.fromFoodType[index]);
    }

    public override string GetProgress(bool complete) => string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CALORIES_FROM_MEAT, (object) GameUtil.GetFormattedCalories(complete ? (float) this.numCalories * 1000f : RationTracker.Get().GetCaloiresConsumedByFood(this.fromFoodType)), (object) GameUtil.GetFormattedCalories((float) this.numCalories * 1000f));
  }
}
