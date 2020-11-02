// Decompiled with JetBrains decompiler
// Type: Klei.AI.ModifierGroup`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

namespace Klei.AI
{
  public class ModifierGroup<T> : Resource
  {
    public List<T> modifiers = new List<T>();

    public IEnumerator<T> GetEnumerator() => (IEnumerator<T>) this.modifiers.GetEnumerator();

    public T this[int idx] => this.modifiers[idx];

    public int Count => this.modifiers.Count;

    public ModifierGroup(string id, string name)
      : base(id, name)
    {
    }

    public void Add(T modifier) => this.modifiers.Add(modifier);
  }
}
