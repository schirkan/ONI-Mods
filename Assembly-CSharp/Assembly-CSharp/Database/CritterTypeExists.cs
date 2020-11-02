﻿// Decompiled with JetBrains decompiler
// Type: Database.CritterTypeExists
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Database
{
  public class CritterTypeExists : ColonyAchievementRequirement
  {
    private List<Tag> critterTypes = new List<Tag>();

    public CritterTypeExists(List<Tag> critterTypes) => this.critterTypes = critterTypes;

    public override bool Success()
    {
      foreach (Component cmp in Components.Capturables.Items)
      {
        if (this.critterTypes.Contains(cmp.PrefabID()))
          return true;
      }
      return false;
    }

    public override void Serialize(BinaryWriter writer)
    {
      writer.Write(this.critterTypes.Count);
      foreach (Tag critterType in this.critterTypes)
        writer.WriteKleiString(critterType.ToString());
    }

    public override void Deserialize(IReader reader)
    {
      int capacity = reader.ReadInt32();
      this.critterTypes = new List<Tag>(capacity);
      for (int index = 0; index < capacity; ++index)
        this.critterTypes.Add(new Tag(reader.ReadKleiString()));
    }

    public override string GetProgress(bool complete) => (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.HATCH_A_MORPH;
  }
}
