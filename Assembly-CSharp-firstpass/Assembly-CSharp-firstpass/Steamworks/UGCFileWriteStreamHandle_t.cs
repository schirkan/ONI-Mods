// Decompiled with JetBrains decompiler
// Type: Steamworks.UGCFileWriteStreamHandle_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [Serializable]
  public struct UGCFileWriteStreamHandle_t : IEquatable<UGCFileWriteStreamHandle_t>, IComparable<UGCFileWriteStreamHandle_t>
  {
    public static readonly UGCFileWriteStreamHandle_t Invalid = new UGCFileWriteStreamHandle_t(ulong.MaxValue);
    public ulong m_UGCFileWriteStreamHandle;

    public UGCFileWriteStreamHandle_t(ulong value) => this.m_UGCFileWriteStreamHandle = value;

    public override string ToString() => this.m_UGCFileWriteStreamHandle.ToString();

    public override bool Equals(object other) => other is UGCFileWriteStreamHandle_t writeStreamHandleT && this == writeStreamHandleT;

    public override int GetHashCode() => this.m_UGCFileWriteStreamHandle.GetHashCode();

    public static bool operator ==(UGCFileWriteStreamHandle_t x, UGCFileWriteStreamHandle_t y) => (long) x.m_UGCFileWriteStreamHandle == (long) y.m_UGCFileWriteStreamHandle;

    public static bool operator !=(UGCFileWriteStreamHandle_t x, UGCFileWriteStreamHandle_t y) => !(x == y);

    public static explicit operator UGCFileWriteStreamHandle_t(ulong value) => new UGCFileWriteStreamHandle_t(value);

    public static explicit operator ulong(UGCFileWriteStreamHandle_t that) => that.m_UGCFileWriteStreamHandle;

    public bool Equals(UGCFileWriteStreamHandle_t other) => (long) this.m_UGCFileWriteStreamHandle == (long) other.m_UGCFileWriteStreamHandle;

    public int CompareTo(UGCFileWriteStreamHandle_t other) => this.m_UGCFileWriteStreamHandle.CompareTo(other.m_UGCFileWriteStreamHandle);
  }
}
