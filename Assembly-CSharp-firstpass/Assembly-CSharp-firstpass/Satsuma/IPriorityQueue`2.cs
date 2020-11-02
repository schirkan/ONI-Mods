﻿// Decompiled with JetBrains decompiler
// Type: Satsuma.IPriorityQueue`2
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Satsuma
{
  public interface IPriorityQueue<TElement, TPriority> : IReadOnlyPriorityQueue<TElement, TPriority>, IClearable
  {
    TPriority this[TElement element] { get; set; }

    bool Remove(TElement element);

    bool Pop();
  }
}
