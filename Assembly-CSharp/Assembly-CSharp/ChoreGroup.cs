// Decompiled with JetBrains decompiler
// Type: ChoreGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using System.Diagnostics;

[DebuggerDisplay("{IdHash}")]
public class ChoreGroup : Resource
{
  public List<ChoreType> choreTypes = new List<ChoreType>();
  public Attribute attribute;
  public string description;
  public string sprite;
  private int defaultPersonalPriority;

  public int DefaultPersonalPriority => this.defaultPersonalPriority;

  public ChoreGroup(
    string id,
    string name,
    Attribute attribute,
    string sprite,
    int default_personal_priority)
    : base(id, name)
  {
    this.attribute = attribute;
    this.description = Strings.Get("STRINGS.DUPLICANTS.CHOREGROUPS." + id.ToUpper() + ".DESC").String;
    this.sprite = sprite;
    this.defaultPersonalPriority = default_personal_priority;
  }
}
