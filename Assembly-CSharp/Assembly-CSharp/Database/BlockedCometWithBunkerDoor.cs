// Decompiled with JetBrains decompiler
// Type: Database.BlockedCometWithBunkerDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.IO;

namespace Database
{
  public class BlockedCometWithBunkerDoor : ColonyAchievementRequirement
  {
    public override bool Success() => Game.Instance.savedInfo.blockedCometWithBunkerDoor;

    public override void Serialize(BinaryWriter writer)
    {
    }

    public override void Deserialize(IReader reader)
    {
    }

    public override string GetProgress(bool complete) => (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BLOCKED_A_COMET;
  }
}
