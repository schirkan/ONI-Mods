// Decompiled with JetBrains decompiler
// Type: Steamworks.SiteId_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [Serializable]
  public struct SiteId_t : IEquatable<SiteId_t>, IComparable<SiteId_t>
  {
    public static readonly SiteId_t Invalid = new SiteId_t(0UL);
    public ulong m_SiteId;

    public SiteId_t(ulong value) => this.m_SiteId = value;

    public override string ToString() => this.m_SiteId.ToString();

    public override bool Equals(object other) => other is SiteId_t siteIdT && this == siteIdT;

    public override int GetHashCode() => this.m_SiteId.GetHashCode();

    public static bool operator ==(SiteId_t x, SiteId_t y) => (long) x.m_SiteId == (long) y.m_SiteId;

    public static bool operator !=(SiteId_t x, SiteId_t y) => !(x == y);

    public static explicit operator SiteId_t(ulong value) => new SiteId_t(value);

    public static explicit operator ulong(SiteId_t that) => that.m_SiteId;

    public bool Equals(SiteId_t other) => (long) this.m_SiteId == (long) other.m_SiteId;

    public int CompareTo(SiteId_t other) => this.m_SiteId.CompareTo(other.m_SiteId);
  }
}
