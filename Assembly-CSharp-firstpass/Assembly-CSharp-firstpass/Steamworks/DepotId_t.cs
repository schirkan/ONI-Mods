// Decompiled with JetBrains decompiler
// Type: Steamworks.DepotId_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [Serializable]
  public struct DepotId_t : IEquatable<DepotId_t>, IComparable<DepotId_t>
  {
    public static readonly DepotId_t Invalid = new DepotId_t(0U);
    public uint m_DepotId;

    public DepotId_t(uint value) => this.m_DepotId = value;

    public override string ToString() => this.m_DepotId.ToString();

    public override bool Equals(object other) => other is DepotId_t depotIdT && this == depotIdT;

    public override int GetHashCode() => this.m_DepotId.GetHashCode();

    public static bool operator ==(DepotId_t x, DepotId_t y) => (int) x.m_DepotId == (int) y.m_DepotId;

    public static bool operator !=(DepotId_t x, DepotId_t y) => !(x == y);

    public static explicit operator DepotId_t(uint value) => new DepotId_t(value);

    public static explicit operator uint(DepotId_t that) => that.m_DepotId;

    public bool Equals(DepotId_t other) => (int) this.m_DepotId == (int) other.m_DepotId;

    public int CompareTo(DepotId_t other) => this.m_DepotId.CompareTo(other.m_DepotId);
  }
}
