// Decompiled with JetBrains decompiler
// Type: Satsuma.DisjointSetSet`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Satsuma
{
  public struct DisjointSetSet<T> : IEquatable<DisjointSetSet<T>>
  {
    public T Representative { get; private set; }

    public DisjointSetSet(T representative)
      : this()
      => this.Representative = representative;

    public bool Equals(DisjointSetSet<T> other) => this.Representative.Equals((object) other.Representative);

    public override bool Equals(object obj) => obj is DisjointSetSet<T> other && this.Equals(other);

    public static bool operator ==(DisjointSetSet<T> a, DisjointSetSet<T> b) => a.Equals(b);

    public static bool operator !=(DisjointSetSet<T> a, DisjointSetSet<T> b) => !(a == b);

    public override int GetHashCode() => this.Representative.GetHashCode();

    public override string ToString() => "[DisjointSetSet:" + (object) this.Representative + "]";
  }
}
