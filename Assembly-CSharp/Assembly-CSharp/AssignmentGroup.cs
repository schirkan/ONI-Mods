﻿// Decompiled with JetBrains decompiler
// Type: AssignmentGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class AssignmentGroup : IAssignableIdentity
{
  private List<IAssignableIdentity> members = new List<IAssignableIdentity>();
  public List<Ownables> current_owners = new List<Ownables>();

  public string id { get; private set; }

  public string name { get; private set; }

  public AssignmentGroup(string id, IAssignableIdentity[] members, string name)
  {
    this.id = id;
    this.name = name;
    foreach (IAssignableIdentity member in members)
      this.members.Add(member);
  }

  public void AddMember(IAssignableIdentity member)
  {
    if (this.members.Contains(member))
      return;
    this.members.Add(member);
  }

  public void RemoveMember(IAssignableIdentity member) => this.members.Remove(member);

  public string GetProperName() => this.name;

  public bool HasMember(IAssignableIdentity member) => this.members.Contains(member);

  public bool IsNull() => false;

  public List<Ownables> GetOwners()
  {
    this.current_owners.Clear();
    foreach (IAssignableIdentity member in this.members)
      this.current_owners.AddRange((IEnumerable<Ownables>) member.GetOwners());
    return this.current_owners;
  }

  public Ownables GetSoleOwner()
  {
    if (this.members.Count == 1)
      return this.members[0] as Ownables;
    Debug.LogWarningFormat("GetSoleOwner called on AssignmentGroup with {0} members", (object) this.members.Count);
    return (Ownables) null;
  }
}
