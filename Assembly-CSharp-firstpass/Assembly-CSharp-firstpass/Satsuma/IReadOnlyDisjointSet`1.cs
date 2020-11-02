// Decompiled with JetBrains decompiler
// Type: Satsuma.IReadOnlyDisjointSet`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

namespace Satsuma
{
  public interface IReadOnlyDisjointSet<T>
  {
    DisjointSetSet<T> WhereIs(T element);

    IEnumerable<T> Elements(DisjointSetSet<T> aSet);
  }
}
