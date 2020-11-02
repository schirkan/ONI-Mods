// Decompiled with JetBrains decompiler
// Type: Steamworks.HTTPRequestHandle
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [Serializable]
  public struct HTTPRequestHandle : IEquatable<HTTPRequestHandle>, IComparable<HTTPRequestHandle>
  {
    public static readonly HTTPRequestHandle Invalid = new HTTPRequestHandle(0U);
    public uint m_HTTPRequestHandle;

    public HTTPRequestHandle(uint value) => this.m_HTTPRequestHandle = value;

    public override string ToString() => this.m_HTTPRequestHandle.ToString();

    public override bool Equals(object other) => other is HTTPRequestHandle httpRequestHandle && this == httpRequestHandle;

    public override int GetHashCode() => this.m_HTTPRequestHandle.GetHashCode();

    public static bool operator ==(HTTPRequestHandle x, HTTPRequestHandle y) => (int) x.m_HTTPRequestHandle == (int) y.m_HTTPRequestHandle;

    public static bool operator !=(HTTPRequestHandle x, HTTPRequestHandle y) => !(x == y);

    public static explicit operator HTTPRequestHandle(uint value) => new HTTPRequestHandle(value);

    public static explicit operator uint(HTTPRequestHandle that) => that.m_HTTPRequestHandle;

    public bool Equals(HTTPRequestHandle other) => (int) this.m_HTTPRequestHandle == (int) other.m_HTTPRequestHandle;

    public int CompareTo(HTTPRequestHandle other) => this.m_HTTPRequestHandle.CompareTo(other.m_HTTPRequestHandle);
  }
}
