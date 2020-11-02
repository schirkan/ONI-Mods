// Decompiled with JetBrains decompiler
// Type: Database.SkillGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;

namespace Database
{
  public class SkillGroup : Resource, IListableOption
  {
    public string choreGroupID;
    public List<Attribute> relevantAttributes;
    public List<string> requiredChoreGroups;
    public string choreGroupIcon;
    public string archetypeIcon;

    string IListableOption.GetProperName() => (string) Strings.Get("STRINGS.DUPLICANTS.SKILLGROUPS." + this.Id.ToUpper() + ".NAME");

    public SkillGroup(
      string id,
      string choreGroupID,
      string name,
      string icon,
      string archetype_icon)
      : base(id, name)
    {
      this.choreGroupID = choreGroupID;
      this.choreGroupIcon = icon;
      this.archetypeIcon = archetype_icon;
    }
  }
}
