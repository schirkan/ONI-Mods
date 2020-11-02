// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamInventoryResult_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [Serializable]
  public struct SteamInventoryResult_t : IEquatable<SteamInventoryResult_t>, IComparable<SteamInventoryResult_t>
  {
    public static readonly SteamInventoryResult_t Invalid = new SteamInventoryResult_t(-1);
    public int m_SteamInventoryResult;

    public SteamInventoryResult_t(int value) => this.m_SteamInventoryResult = value;

    public override string ToString() => this.m_SteamInventoryResult.ToString();

    public override bool Equals(object other) => other is SteamInventoryResult_t inventoryResultT && this == inventoryResultT;

    public override int GetHashCode() => this.m_SteamInventoryResult.GetHashCode();

    public static bool operator ==(SteamInventoryResult_t x, SteamInventoryResult_t y) => x.m_SteamInventoryResult == y.m_SteamInventoryResult;

    public static bool operator !=(SteamInventoryResult_t x, SteamInventoryResult_t y) => !(x == y);

    public static explicit operator SteamInventoryResult_t(int value) => new SteamInventoryResult_t(value);

    public static explicit operator int(SteamInventoryResult_t that) => that.m_SteamInventoryResult;

    public bool Equals(SteamInventoryResult_t other) => this.m_SteamInventoryResult == other.m_SteamInventoryResult;

    public int CompareTo(SteamInventoryResult_t other) => this.m_SteamInventoryResult.CompareTo(other.m_SteamInventoryResult);
  }
}
