// Decompiled with JetBrains decompiler
// Type: Steamworks.SNetListenSocket_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [Serializable]
  public struct SNetListenSocket_t : IEquatable<SNetListenSocket_t>, IComparable<SNetListenSocket_t>
  {
    public uint m_SNetListenSocket;

    public SNetListenSocket_t(uint value) => this.m_SNetListenSocket = value;

    public override string ToString() => this.m_SNetListenSocket.ToString();

    public override bool Equals(object other) => other is SNetListenSocket_t snetListenSocketT && this == snetListenSocketT;

    public override int GetHashCode() => this.m_SNetListenSocket.GetHashCode();

    public static bool operator ==(SNetListenSocket_t x, SNetListenSocket_t y) => (int) x.m_SNetListenSocket == (int) y.m_SNetListenSocket;

    public static bool operator !=(SNetListenSocket_t x, SNetListenSocket_t y) => !(x == y);

    public static explicit operator SNetListenSocket_t(uint value) => new SNetListenSocket_t(value);

    public static explicit operator uint(SNetListenSocket_t that) => that.m_SNetListenSocket;

    public bool Equals(SNetListenSocket_t other) => (int) this.m_SNetListenSocket == (int) other.m_SNetListenSocket;

    public int CompareTo(SNetListenSocket_t other) => this.m_SNetListenSocket.CompareTo(other.m_SNetListenSocket);
  }
}
