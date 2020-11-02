// Decompiled with JetBrains decompiler
// Type: Steamworks.PublishedFileUpdateHandle_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [Serializable]
  public struct PublishedFileUpdateHandle_t : IEquatable<PublishedFileUpdateHandle_t>, IComparable<PublishedFileUpdateHandle_t>
  {
    public static readonly PublishedFileUpdateHandle_t Invalid = new PublishedFileUpdateHandle_t(ulong.MaxValue);
    public ulong m_PublishedFileUpdateHandle;

    public PublishedFileUpdateHandle_t(ulong value) => this.m_PublishedFileUpdateHandle = value;

    public override string ToString() => this.m_PublishedFileUpdateHandle.ToString();

    public override bool Equals(object other) => other is PublishedFileUpdateHandle_t fileUpdateHandleT && this == fileUpdateHandleT;

    public override int GetHashCode() => this.m_PublishedFileUpdateHandle.GetHashCode();

    public static bool operator ==(PublishedFileUpdateHandle_t x, PublishedFileUpdateHandle_t y) => (long) x.m_PublishedFileUpdateHandle == (long) y.m_PublishedFileUpdateHandle;

    public static bool operator !=(PublishedFileUpdateHandle_t x, PublishedFileUpdateHandle_t y) => !(x == y);

    public static explicit operator PublishedFileUpdateHandle_t(
      ulong value)
    {
      return new PublishedFileUpdateHandle_t(value);
    }

    public static explicit operator ulong(PublishedFileUpdateHandle_t that) => that.m_PublishedFileUpdateHandle;

    public bool Equals(PublishedFileUpdateHandle_t other) => (long) this.m_PublishedFileUpdateHandle == (long) other.m_PublishedFileUpdateHandle;

    public int CompareTo(PublishedFileUpdateHandle_t other) => this.m_PublishedFileUpdateHandle.CompareTo(other.m_PublishedFileUpdateHandle);
  }
}
