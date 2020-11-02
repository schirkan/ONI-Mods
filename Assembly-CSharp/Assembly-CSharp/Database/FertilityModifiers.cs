// Decompiled with JetBrains decompiler
// Type: Database.FertilityModifiers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;

namespace Database
{
  public class FertilityModifiers : ResourceSet<FertilityModifier>
  {
    public List<FertilityModifier> GetForTag(Tag searchTag)
    {
      List<FertilityModifier> fertilityModifierList = new List<FertilityModifier>();
      foreach (FertilityModifier resource in this.resources)
      {
        if (resource.TargetTag == searchTag)
          fertilityModifierList.Add(resource);
      }
      return fertilityModifierList;
    }
  }
}
