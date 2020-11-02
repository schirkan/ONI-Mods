﻿// Decompiled with JetBrains decompiler
// Type: Database.SkillBranchComplete
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System.Collections.Generic;
using System.IO;

namespace Database
{
  public class SkillBranchComplete : ColonyAchievementRequirement
  {
    private List<Skill> skillsToMaster;

    public SkillBranchComplete(List<Skill> skillsToMaster) => this.skillsToMaster = skillsToMaster;

    public override bool Success()
    {
      foreach (MinionResume minionResume in Components.MinionResumes.Items)
      {
        foreach (Skill skill in this.skillsToMaster)
        {
          if (minionResume.HasMasteredSkill(skill.Id))
            return true;
        }
      }
      return false;
    }

    public override void Serialize(BinaryWriter writer)
    {
      writer.Write(this.skillsToMaster.Count);
      foreach (Skill skill in this.skillsToMaster)
        writer.WriteKleiString(skill.Id);
    }

    public override void Deserialize(IReader reader)
    {
      this.skillsToMaster = new List<Skill>();
      int num = reader.ReadInt32();
      for (int index = 0; index < num; ++index)
      {
        string id = reader.ReadKleiString();
        this.skillsToMaster.Add(Db.Get().Skills.Get(id));
      }
    }

    public override string GetProgress(bool complete) => (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.SKILL_BRANCH;
  }
}
