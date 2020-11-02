// Decompiled with JetBrains decompiler
// Type: Steamworks.RTime32
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [Serializable]
  public struct RTime32 : IEquatable<RTime32>, IComparable<RTime32>
  {
    public uint m_RTime32;

    public RTime32(uint value) => this.m_RTime32 = value;

    public override string ToString() => this.m_RTime32.ToString();

    public override bool Equals(object other) => other is RTime32 rtime32 && this == rtime32;

    public override int GetHashCode() => this.m_RTime32.GetHashCode();

    public static bool operator ==(RTime32 x, RTime32 y) => (int) x.m_RTime32 == (int) y.m_RTime32;

    public static bool operator !=(RTime32 x, RTime32 y) => !(x == y);

    public static explicit operator RTime32(uint value) => new RTime32(value);

    public static explicit operator uint(RTime32 that) => that.m_RTime32;

    public bool Equals(RTime32 other) => (int) this.m_RTime32 == (int) other.m_RTime32;

    public int CompareTo(RTime32 other) => this.m_RTime32.CompareTo(other.m_RTime32);
  }
}
