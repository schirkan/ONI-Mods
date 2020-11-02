// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamLeaderboardEntries_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [Serializable]
  public struct SteamLeaderboardEntries_t : IEquatable<SteamLeaderboardEntries_t>, IComparable<SteamLeaderboardEntries_t>
  {
    public ulong m_SteamLeaderboardEntries;

    public SteamLeaderboardEntries_t(ulong value) => this.m_SteamLeaderboardEntries = value;

    public override string ToString() => this.m_SteamLeaderboardEntries.ToString();

    public override bool Equals(object other) => other is SteamLeaderboardEntries_t leaderboardEntriesT && this == leaderboardEntriesT;

    public override int GetHashCode() => this.m_SteamLeaderboardEntries.GetHashCode();

    public static bool operator ==(SteamLeaderboardEntries_t x, SteamLeaderboardEntries_t y) => (long) x.m_SteamLeaderboardEntries == (long) y.m_SteamLeaderboardEntries;

    public static bool operator !=(SteamLeaderboardEntries_t x, SteamLeaderboardEntries_t y) => !(x == y);

    public static explicit operator SteamLeaderboardEntries_t(ulong value) => new SteamLeaderboardEntries_t(value);

    public static explicit operator ulong(SteamLeaderboardEntries_t that) => that.m_SteamLeaderboardEntries;

    public bool Equals(SteamLeaderboardEntries_t other) => (long) this.m_SteamLeaderboardEntries == (long) other.m_SteamLeaderboardEntries;

    public int CompareTo(SteamLeaderboardEntries_t other) => this.m_SteamLeaderboardEntries.CompareTo(other.m_SteamLeaderboardEntries);
  }
}
