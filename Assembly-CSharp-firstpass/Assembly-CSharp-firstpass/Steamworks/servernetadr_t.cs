// Decompiled with JetBrains decompiler
// Type: Steamworks.servernetadr_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [Serializable]
  public struct servernetadr_t
  {
    private ushort m_usConnectionPort;
    private ushort m_usQueryPort;
    private uint m_unIP;

    public void Init(uint ip, ushort usQueryPort, ushort usConnectionPort)
    {
      this.m_unIP = ip;
      this.m_usQueryPort = usQueryPort;
      this.m_usConnectionPort = usConnectionPort;
    }

    public ushort GetQueryPort() => this.m_usQueryPort;

    public void SetQueryPort(ushort usPort) => this.m_usQueryPort = usPort;

    public ushort GetConnectionPort() => this.m_usConnectionPort;

    public void SetConnectionPort(ushort usPort) => this.m_usConnectionPort = usPort;

    public uint GetIP() => this.m_unIP;

    public void SetIP(uint unIP) => this.m_unIP = unIP;

    public string GetConnectionAddressString() => servernetadr_t.ToString(this.m_unIP, this.m_usConnectionPort);

    public string GetQueryAddressString() => servernetadr_t.ToString(this.m_unIP, this.m_usQueryPort);

    public static string ToString(uint unIP, ushort usPort) => string.Format("{0}.{1}.{2}.{3}:{4}", (object) (ulong) ((long) (unIP >> 24) & (long) byte.MaxValue), (object) (ulong) ((long) (unIP >> 16) & (long) byte.MaxValue), (object) (ulong) ((long) (unIP >> 8) & (long) byte.MaxValue), (object) (ulong) ((long) unIP & (long) byte.MaxValue), (object) usPort);

    public static bool operator <(servernetadr_t x, servernetadr_t y)
    {
      if (x.m_unIP < y.m_unIP)
        return true;
      return (int) x.m_unIP == (int) y.m_unIP && (int) x.m_usQueryPort < (int) y.m_usQueryPort;
    }

    public static bool operator >(servernetadr_t x, servernetadr_t y)
    {
      if (x.m_unIP > y.m_unIP)
        return true;
      return (int) x.m_unIP == (int) y.m_unIP && (int) x.m_usQueryPort > (int) y.m_usQueryPort;
    }

    public override bool Equals(object other) => other is servernetadr_t servernetadrT && this == servernetadrT;

    public override int GetHashCode() => this.m_unIP.GetHashCode() + this.m_usQueryPort.GetHashCode() + this.m_usConnectionPort.GetHashCode();

    public static bool operator ==(servernetadr_t x, servernetadr_t y) => (int) x.m_unIP == (int) y.m_unIP && (int) x.m_usQueryPort == (int) y.m_usQueryPort && (int) x.m_usConnectionPort == (int) y.m_usConnectionPort;

    public static bool operator !=(servernetadr_t x, servernetadr_t y) => !(x == y);

    public bool Equals(servernetadr_t other) => (int) this.m_unIP == (int) other.m_unIP && (int) this.m_usQueryPort == (int) other.m_usQueryPort && (int) this.m_usConnectionPort == (int) other.m_usConnectionPort;

    public int CompareTo(servernetadr_t other) => this.m_unIP.CompareTo(other.m_unIP) + this.m_usQueryPort.CompareTo(other.m_usQueryPort) + this.m_usConnectionPort.CompareTo(other.m_usConnectionPort);
  }
}
