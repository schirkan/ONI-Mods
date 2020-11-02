// Decompiled with JetBrains decompiler
// Type: Satsuma.IFlow`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace Satsuma
{
  public interface IFlow<TCapacity>
  {
    IGraph Graph { get; }

    Func<Arc, TCapacity> Capacity { get; }

    Node Source { get; }

    Node Target { get; }

    TCapacity FlowSize { get; }

    IEnumerable<KeyValuePair<Arc, TCapacity>> NonzeroArcs { get; }

    TCapacity Flow(Arc arc);
  }
}
