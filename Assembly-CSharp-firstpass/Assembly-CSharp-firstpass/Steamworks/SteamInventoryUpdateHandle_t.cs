// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamInventoryUpdateHandle_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [Serializable]
  public struct SteamInventoryUpdateHandle_t : IEquatable<SteamInventoryUpdateHandle_t>, IComparable<SteamInventoryUpdateHandle_t>
  {
    public static readonly SteamInventoryUpdateHandle_t Invalid = new SteamInventoryUpdateHandle_t(ulong.MaxValue);
    public ulong m_SteamInventoryUpdateHandle;

    public SteamInventoryUpdateHandle_t(ulong value) => this.m_SteamInventoryUpdateHandle = value;

    public override string ToString() => this.m_SteamInventoryUpdateHandle.ToString();

    public override bool Equals(object other) => other is SteamInventoryUpdateHandle_t inventoryUpdateHandleT && this == inventoryUpdateHandleT;

    public override int GetHashCode() => this.m_SteamInventoryUpdateHandle.GetHashCode();

    public static bool operator ==(SteamInventoryUpdateHandle_t x, SteamInventoryUpdateHandle_t y) => (long) x.m_SteamInventoryUpdateHandle == (long) y.m_SteamInventoryUpdateHandle;

    public static bool operator !=(SteamInventoryUpdateHandle_t x, SteamInventoryUpdateHandle_t y) => !(x == y);

    public static explicit operator SteamInventoryUpdateHandle_t(
      ulong value)
    {
      return new SteamInventoryUpdateHandle_t(value);
    }

    public static explicit operator ulong(SteamInventoryUpdateHandle_t that) => that.m_SteamInventoryUpdateHandle;

    public bool Equals(SteamInventoryUpdateHandle_t other) => (long) this.m_SteamInventoryUpdateHandle == (long) other.m_SteamInventoryUpdateHandle;

    public int CompareTo(SteamInventoryUpdateHandle_t other) => this.m_SteamInventoryUpdateHandle.CompareTo(other.m_SteamInventoryUpdateHandle);
  }
}
