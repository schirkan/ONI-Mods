// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.FakeList`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace YamlDotNet.Core
{
  public class FakeList<T>
  {
    private readonly IEnumerator<T> collection;
    private int currentIndex = -1;

    public FakeList(IEnumerator<T> collection) => this.collection = collection;

    public FakeList(IEnumerable<T> collection)
      : this(collection.GetEnumerator())
    {
    }

    public T this[int index]
    {
      get
      {
        if (index < this.currentIndex)
        {
          this.collection.Reset();
          this.currentIndex = -1;
        }
        for (; this.currentIndex < index; ++this.currentIndex)
        {
          if (!this.collection.MoveNext())
            throw new ArgumentOutOfRangeException(nameof (index));
        }
        return this.collection.Current;
      }
    }
  }
}
