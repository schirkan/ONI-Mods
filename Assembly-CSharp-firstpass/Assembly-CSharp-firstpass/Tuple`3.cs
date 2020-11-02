// Decompiled with JetBrains decompiler
// Type: Tuple`3
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

public class Tuple<T, U, V> : IEquatable<Tuple<T, U, V>>
{
  public T first;
  public U second;
  public V third;

  public Tuple(T a, U b, V c)
  {
    this.first = a;
    this.second = b;
    this.third = c;
  }

  public bool Equals(Tuple<T, U, V> other) => this.first.Equals((object) other.first) && this.second.Equals((object) other.second) && this.third.Equals((object) other.third);

  public override int GetHashCode() => this.first.GetHashCode() ^ this.second.GetHashCode() ^ this.third.GetHashCode();
}
