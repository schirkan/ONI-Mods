// Decompiled with JetBrains decompiler
// Type: Steamworks.InputHandle_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [Serializable]
  public struct InputHandle_t : IEquatable<InputHandle_t>, IComparable<InputHandle_t>
  {
    public ulong m_InputHandle;

    public InputHandle_t(ulong value) => this.m_InputHandle = value;

    public override string ToString() => this.m_InputHandle.ToString();

    public override bool Equals(object other) => other is InputHandle_t inputHandleT && this == inputHandleT;

    public override int GetHashCode() => this.m_InputHandle.GetHashCode();

    public static bool operator ==(InputHandle_t x, InputHandle_t y) => (long) x.m_InputHandle == (long) y.m_InputHandle;

    public static bool operator !=(InputHandle_t x, InputHandle_t y) => !(x == y);

    public static explicit operator InputHandle_t(ulong value) => new InputHandle_t(value);

    public static explicit operator ulong(InputHandle_t that) => that.m_InputHandle;

    public bool Equals(InputHandle_t other) => (long) this.m_InputHandle == (long) other.m_InputHandle;

    public int CompareTo(InputHandle_t other) => this.m_InputHandle.CompareTo(other.m_InputHandle);
  }
}
