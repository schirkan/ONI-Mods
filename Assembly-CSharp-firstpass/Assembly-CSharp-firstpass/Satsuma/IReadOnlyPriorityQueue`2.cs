// Decompiled with JetBrains decompiler
// Type: Satsuma.IReadOnlyPriorityQueue`2
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

namespace Satsuma
{
  public interface IReadOnlyPriorityQueue<TElement, TPriority>
  {
    int Count { get; }

    IEnumerable<KeyValuePair<TElement, TPriority>> Items { get; }

    bool Contains(TElement element);

    bool TryGetPriority(TElement element, out TPriority priority);

    TElement Peek();

    TElement Peek(out TPriority priority);
  }
}
