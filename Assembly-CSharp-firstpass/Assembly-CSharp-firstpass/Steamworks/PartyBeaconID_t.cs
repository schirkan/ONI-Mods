// Decompiled with JetBrains decompiler
// Type: Steamworks.PartyBeaconID_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [Serializable]
  public struct PartyBeaconID_t : IEquatable<PartyBeaconID_t>, IComparable<PartyBeaconID_t>
  {
    public static readonly PartyBeaconID_t Invalid = new PartyBeaconID_t(0UL);
    public ulong m_PartyBeaconID;

    public PartyBeaconID_t(ulong value) => this.m_PartyBeaconID = value;

    public override string ToString() => this.m_PartyBeaconID.ToString();

    public override bool Equals(object other) => other is PartyBeaconID_t partyBeaconIdT && this == partyBeaconIdT;

    public override int GetHashCode() => this.m_PartyBeaconID.GetHashCode();

    public static bool operator ==(PartyBeaconID_t x, PartyBeaconID_t y) => (long) x.m_PartyBeaconID == (long) y.m_PartyBeaconID;

    public static bool operator !=(PartyBeaconID_t x, PartyBeaconID_t y) => !(x == y);

    public static explicit operator PartyBeaconID_t(ulong value) => new PartyBeaconID_t(value);

    public static explicit operator ulong(PartyBeaconID_t that) => that.m_PartyBeaconID;

    public bool Equals(PartyBeaconID_t other) => (long) this.m_PartyBeaconID == (long) other.m_PartyBeaconID;

    public int CompareTo(PartyBeaconID_t other) => this.m_PartyBeaconID.CompareTo(other.m_PartyBeaconID);
  }
}
