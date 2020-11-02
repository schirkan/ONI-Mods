// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.InsertionQueue`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace YamlDotNet.Core
{
  [Serializable]
  public class InsertionQueue<T>
  {
    private readonly IList<T> items = (IList<T>) new List<T>();

    public int Count => this.items.Count;

    public void Enqueue(T item) => this.items.Add(item);

    public T Dequeue()
    {
      if (this.Count == 0)
        throw new InvalidOperationException("The queue is empty");
      T obj = this.items[0];
      this.items.RemoveAt(0);
      return obj;
    }

    public void Insert(int index, T item) => this.items.Insert(index, item);
  }
}
