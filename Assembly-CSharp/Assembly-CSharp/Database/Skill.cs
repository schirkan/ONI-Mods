// Decompiled with JetBrains decompiler
// Type: Database.Skill
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;

namespace Database
{
  public class Skill : Resource
  {
    public string description;
    public string skillGroup;
    public string hat;
    public string badge;
    public int tier;
    public List<SkillPerk> perks;
    public List<string> priorSkills;

    public Skill(
      string id,
      string name,
      string description,
      int tier,
      string hat,
      string badge,
      string skillGroup)
      : base(id, name)
    {
      this.description = description;
      this.tier = tier;
      this.hat = hat;
      this.badge = badge;
      this.skillGroup = skillGroup;
      this.perks = new List<SkillPerk>();
      this.priorSkills = new List<string>();
    }

    public int GetMoraleExpectation() => SKILLS.SKILL_TIER_MORALE_COST[this.tier];

    public bool GivesPerk(SkillPerk perk) => this.perks.Contains(perk);

    public bool GivesPerk(HashedString perkId)
    {
      foreach (Resource perk in this.perks)
      {
        if (perk.IdHash == perkId)
          return true;
      }
      return false;
    }
  }
}
