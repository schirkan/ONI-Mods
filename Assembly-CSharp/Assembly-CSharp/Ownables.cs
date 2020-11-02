﻿// Decompiled with JetBrains decompiler
// Type: Ownables
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class Ownables : Assignables
{
  protected override void OnSpawn() => base.OnSpawn();

  public void UnassignAll()
  {
    foreach (AssignableSlotInstance slot in this.slots)
    {
      if ((Object) slot.assignable != (Object) null)
        slot.assignable.Unassign();
    }
  }
}
