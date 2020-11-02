// Decompiled with JetBrains decompiler
// Type: Database.NumberOfDupes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.IO;

namespace Database
{
  public class NumberOfDupes : VictoryColonyAchievementRequirement
  {
    private int numDupes;

    public override string Name() => string.Format((string) COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_DUPLICANTS, (object) this.numDupes);

    public override string Description() => string.Format((string) COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_DUPLICANTS_DESCRIPTION, (object) this.numDupes);

    public NumberOfDupes(int num) => this.numDupes = num;

    public override bool Success() => Components.LiveMinionIdentities.Items.Count >= this.numDupes;

    public override void Serialize(BinaryWriter writer) => writer.Write(this.numDupes);

    public override void Deserialize(IReader reader) => this.numDupes = reader.ReadInt32();

    public override string GetProgress(bool complete) => string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.POPULATION, (object) (complete ? this.numDupes : Components.LiveMinionIdentities.Items.Count), (object) this.numDupes);
  }
}
