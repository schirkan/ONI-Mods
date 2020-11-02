// Decompiled with JetBrains decompiler
// Type: Database.CureDisease
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.IO;

namespace Database
{
  public class CureDisease : ColonyAchievementRequirement
  {
    public override bool Success() => Game.Instance.savedInfo.curedDisease;

    public override void Serialize(BinaryWriter writer)
    {
    }

    public override void Deserialize(IReader reader)
    {
    }

    public override string GetProgress(bool complete) => (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CURED_DISEASE;
  }
}
