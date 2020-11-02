// Decompiled with JetBrains decompiler
// Type: Database.MonumentBuilt
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.IO;

namespace Database
{
  public class MonumentBuilt : VictoryColonyAchievementRequirement
  {
    public override string Name() => (string) COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.BUILT_MONUMENT;

    public override string Description() => (string) COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.BUILT_MONUMENT_DESCRIPTION;

    public override bool Success()
    {
      foreach (MonumentPart monumentPart in Components.MonumentParts)
      {
        if (monumentPart.IsMonumentCompleted())
        {
          Game.Instance.unlocks.Unlock("thriving");
          return true;
        }
      }
      return false;
    }

    public override void Deserialize(IReader reader)
    {
    }

    public override void Serialize(BinaryWriter writer)
    {
    }

    public override string GetProgress(bool complete) => this.Name();
  }
}
