// Decompiled with JetBrains decompiler
// Type: Satsuma.IDisjointSet`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Satsuma
{
  public interface IDisjointSet<T> : IReadOnlyDisjointSet<T>, IClearable
  {
    DisjointSetSet<T> Union(DisjointSetSet<T> a, DisjointSetSet<T> b);
  }
}
