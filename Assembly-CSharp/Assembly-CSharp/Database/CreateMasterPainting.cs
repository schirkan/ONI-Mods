// Decompiled with JetBrains decompiler
// Type: Database.CreateMasterPainting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.IO;
using UnityEngine;

namespace Database
{
  public class CreateMasterPainting : ColonyAchievementRequirement
  {
    public override bool Success()
    {
      foreach (Painting painting in Components.Paintings.Items)
      {
        if ((Object) painting != (Object) null && painting.CurrentStatus == Artable.Status.Great)
          return true;
      }
      return false;
    }

    public override void Deserialize(IReader reader)
    {
    }

    public override void Serialize(BinaryWriter writer)
    {
    }

    public override string GetProgress(bool complete) => (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CREATE_A_PAINTING;
  }
}
