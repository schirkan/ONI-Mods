// Decompiled with JetBrains decompiler
// Type: AssignableSlot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Diagnostics;
using UnityEngine;

[DebuggerDisplay("{Id}")]
[Serializable]
public class AssignableSlot : Resource
{
  public bool showInUI = true;

  public AssignableSlot(string id, string name, bool showInUI = true)
    : base(id, name)
    => this.showInUI = showInUI;

  public AssignableSlotInstance Lookup(GameObject go)
  {
    Assignables component = go.GetComponent<Assignables>();
    return (UnityEngine.Object) component != (UnityEngine.Object) null ? component.GetSlot(this) : (AssignableSlotInstance) null;
  }
}
