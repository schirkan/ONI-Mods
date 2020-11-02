// Decompiled with JetBrains decompiler
// Type: RoleSlotUnlock
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class RoleSlotUnlock
{
  public string id { get; protected set; }

  public string name { get; protected set; }

  public string description { get; protected set; }

  public List<Tuple<string, int>> slots { get; protected set; }

  public Func<bool> isSatisfied { get; protected set; }

  public RoleSlotUnlock(
    string id,
    string name,
    string description,
    List<Tuple<string, int>> slots,
    Func<bool> isSatisfied)
  {
    this.id = id;
    this.name = name;
    this.description = description;
    this.slots = slots;
    this.isSatisfied = isSatisfied;
  }
}
