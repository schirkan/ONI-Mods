// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamAPICall_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [Serializable]
  public struct SteamAPICall_t : IEquatable<SteamAPICall_t>, IComparable<SteamAPICall_t>
  {
    public static readonly SteamAPICall_t Invalid = new SteamAPICall_t(0UL);
    public ulong m_SteamAPICall;

    public SteamAPICall_t(ulong value) => this.m_SteamAPICall = value;

    public override string ToString() => this.m_SteamAPICall.ToString();

    public override bool Equals(object other) => other is SteamAPICall_t steamApiCallT && this == steamApiCallT;

    public override int GetHashCode() => this.m_SteamAPICall.GetHashCode();

    public static bool operator ==(SteamAPICall_t x, SteamAPICall_t y) => (long) x.m_SteamAPICall == (long) y.m_SteamAPICall;

    public static bool operator !=(SteamAPICall_t x, SteamAPICall_t y) => !(x == y);

    public static explicit operator SteamAPICall_t(ulong value) => new SteamAPICall_t(value);

    public static explicit operator ulong(SteamAPICall_t that) => that.m_SteamAPICall;

    public bool Equals(SteamAPICall_t other) => (long) this.m_SteamAPICall == (long) other.m_SteamAPICall;

    public int CompareTo(SteamAPICall_t other) => this.m_SteamAPICall.CompareTo(other.m_SteamAPICall);
  }
}
