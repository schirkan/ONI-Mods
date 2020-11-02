// Decompiled with JetBrains decompiler
// Type: Database.ActivateLorePOI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.IO;
using UnityEngine;

namespace Database
{
  public class ActivateLorePOI : ColonyAchievementRequirement
  {
    public override void Deserialize(IReader reader)
    {
    }

    public override void Serialize(BinaryWriter writer)
    {
    }

    public override bool Success()
    {
      foreach (BuildingComplete buildingComplete in Components.TemplateBuildings.Items)
      {
        if (!((Object) buildingComplete == (Object) null))
        {
          Unsealable component = buildingComplete.GetComponent<Unsealable>();
          if ((Object) component != (Object) null && component.unsealed)
            return true;
        }
      }
      return false;
    }

    public override string GetProgress(bool complete) => (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.INVESTIGATE_A_POI;
  }
}
