﻿// Decompiled with JetBrains decompiler
// Type: Pair`2
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

[Serializable]
public struct Pair<T, U> : IEquatable<Pair<T, U>>
{
  public T first;
  public U second;

  public Pair(T a, U b)
  {
    this.first = a;
    this.second = b;
  }

  public bool Equals(Pair<T, U> other) => this.first.Equals((object) other.first) && this.second.Equals((object) other.second);

  public override int GetHashCode() => this.first.GetHashCode() ^ this.second.GetHashCode();
}
