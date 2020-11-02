// Decompiled with JetBrains decompiler
// Type: Satsuma.Arc
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Satsuma
{
  public struct Arc : IEquatable<Arc>
  {
    public long Id { get; private set; }

    public Arc(long id)
      : this()
      => this.Id = id;

    public static Arc Invalid => new Arc(0L);

    public bool Equals(Arc other) => this.Id == other.Id;

    public override bool Equals(object obj) => obj is Arc other && this.Equals(other);

    public override int GetHashCode() => this.Id.GetHashCode();

    public override string ToString() => "|" + (object) this.Id;

    public static bool operator ==(Arc a, Arc b) => a.Equals(b);

    public static bool operator !=(Arc a, Arc b) => !(a == b);
  }
}
