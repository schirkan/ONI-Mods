// Decompiled with JetBrains decompiler
// Type: LinearDictionary`2
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

public class LinearDictionary<Key, Val> where Key : IEquatable<Key>
{
  private List<Key> keys = new List<Key>();
  private List<Val> values = new List<Val>();

  private int GetIdx(Key key)
  {
    int count = this.keys.Count;
    int num = -1;
    for (int index = 0; index < count; ++index)
    {
      if (this.keys[index].Equals(key))
      {
        num = index;
        break;
      }
    }
    return num;
  }

  public Val this[Key key]
  {
    get
    {
      Val val = default (Val);
      int idx = this.GetIdx(key);
      if (idx != -1)
        val = this.values[idx];
      return val;
    }
    set
    {
      int idx = this.GetIdx(key);
      if (idx != -1)
      {
        this.values[idx] = value;
      }
      else
      {
        this.keys.Add(key);
        this.values.Add(value);
      }
    }
  }
}
