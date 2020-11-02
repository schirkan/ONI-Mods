// Decompiled with JetBrains decompiler
// Type: DefHandle
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct DefHandle
{
  [SerializeField]
  private int defIdx;
  private static List<object> defs = new List<object>();

  public bool IsValid() => this.defIdx > 0;

  public DefType Get<DefType>() where DefType : class, new()
  {
    if (this.defIdx == 0)
    {
      DefHandle.defs.Add((object) new DefType());
      this.defIdx = DefHandle.defs.Count;
    }
    return DefHandle.defs[this.defIdx - 1] as DefType;
  }

  public void Set<DefType>(DefType value) where DefType : class, new()
  {
    DefHandle.defs.Add((object) value);
    this.defIdx = DefHandle.defs.Count;
  }
}
