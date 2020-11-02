// Decompiled with JetBrains decompiler
// Type: MIConvexHull.SimpleList`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace MIConvexHull
{
  internal class SimpleList<T>
  {
    private int capacity;
    public int Count;
    private T[] items;

    public T this[int i]
    {
      get => this.items[i];
      set => this.items[i] = value;
    }

    private void EnsureCapacity()
    {
      if (this.capacity == 0)
      {
        this.capacity = 32;
        this.items = new T[32];
      }
      else
      {
        T[] objArray = new T[this.capacity * 2];
        Array.Copy((Array) this.items, (Array) objArray, this.capacity);
        this.capacity = 2 * this.capacity;
        this.items = objArray;
      }
    }

    public void Add(T item)
    {
      if (this.Count + 1 > this.capacity)
        this.EnsureCapacity();
      this.items[this.Count++] = item;
    }

    public void Push(T item)
    {
      if (this.Count + 1 > this.capacity)
        this.EnsureCapacity();
      this.items[this.Count++] = item;
    }

    public T Pop() => this.items[--this.Count];

    public void Clear() => this.Count = 0;
  }
}
