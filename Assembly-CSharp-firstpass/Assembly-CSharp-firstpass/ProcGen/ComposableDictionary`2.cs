﻿// Decompiled with JetBrains decompiler
// Type: ProcGen.ComposableDictionary`2
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace ProcGen
{
  [Serializable]
  public class ComposableDictionary<Key, Value> : IMerge<ComposableDictionary<Key, Value>>
  {
    public Dictionary<Key, Value> add { get; private set; }

    public List<Key> remove { get; private set; }

    public ComposableDictionary()
    {
      this.add = new Dictionary<Key, Value>();
      this.remove = new List<Key>();
    }

    private void VerifyConsolidated() => DebugUtil.Assert(this.remove.Count == 0, "needs to be Consolidate()d before being used");

    public Value this[Key key]
    {
      get
      {
        this.VerifyConsolidated();
        return this.add[key];
      }
      set => this.add[key] = value;
    }

    public ICollection<Key> Keys
    {
      get
      {
        this.VerifyConsolidated();
        return (ICollection<Key>) this.add.Keys;
      }
    }

    public ICollection<Value> Values
    {
      get
      {
        this.VerifyConsolidated();
        return (ICollection<Value>) this.add.Values;
      }
    }

    public void Add(Key key, Value value) => this.add.Add(key, value);

    public void Add(KeyValuePair<Key, Value> pair) => this.Add(pair.Key, pair.Value);

    public bool Remove(Key key)
    {
      this.add.Remove(key);
      return true;
    }

    public void Clear() => this.add.Clear();

    public bool ContainsKey(Key key)
    {
      this.VerifyConsolidated();
      return this.add.ContainsKey(key);
    }

    public bool TryGetValue(Key key, out Value value)
    {
      this.VerifyConsolidated();
      return this.add.TryGetValue(key, out value);
    }

    public int Count
    {
      get
      {
        this.VerifyConsolidated();
        return this.add.Count;
      }
    }

    public bool IsReadOnly => false;

    public IEnumerator<KeyValuePair<Key, Value>> GetEnumerator()
    {
      this.VerifyConsolidated();
      return (IEnumerator<KeyValuePair<Key, Value>>) this.add.GetEnumerator();
    }

    public void Merge(ComposableDictionary<Key, Value> other)
    {
      this.VerifyConsolidated();
      foreach (Key key in other.remove)
        this.add.Remove(key);
      foreach (KeyValuePair<Key, Value> keyValuePair in other.add)
      {
        if (this.add.ContainsKey(keyValuePair.Key))
          DebugUtil.LogArgs((object) "Overwriting entry {0}", (object) keyValuePair.Key.ToString());
        this.add.Add(keyValuePair.Key, keyValuePair.Value);
      }
    }
  }
}
