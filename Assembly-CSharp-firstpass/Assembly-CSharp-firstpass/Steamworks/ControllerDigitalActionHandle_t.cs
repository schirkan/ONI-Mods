// Decompiled with JetBrains decompiler
// Type: Steamworks.ControllerDigitalActionHandle_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [Serializable]
  public struct ControllerDigitalActionHandle_t : IEquatable<ControllerDigitalActionHandle_t>, IComparable<ControllerDigitalActionHandle_t>
  {
    public ulong m_ControllerDigitalActionHandle;

    public ControllerDigitalActionHandle_t(ulong value) => this.m_ControllerDigitalActionHandle = value;

    public override string ToString() => this.m_ControllerDigitalActionHandle.ToString();

    public override bool Equals(object other) => other is ControllerDigitalActionHandle_t digitalActionHandleT && this == digitalActionHandleT;

    public override int GetHashCode() => this.m_ControllerDigitalActionHandle.GetHashCode();

    public static bool operator ==(
      ControllerDigitalActionHandle_t x,
      ControllerDigitalActionHandle_t y)
    {
      return (long) x.m_ControllerDigitalActionHandle == (long) y.m_ControllerDigitalActionHandle;
    }

    public static bool operator !=(
      ControllerDigitalActionHandle_t x,
      ControllerDigitalActionHandle_t y)
    {
      return !(x == y);
    }

    public static explicit operator ControllerDigitalActionHandle_t(
      ulong value)
    {
      return new ControllerDigitalActionHandle_t(value);
    }

    public static explicit operator ulong(ControllerDigitalActionHandle_t that) => that.m_ControllerDigitalActionHandle;

    public bool Equals(ControllerDigitalActionHandle_t other) => (long) this.m_ControllerDigitalActionHandle == (long) other.m_ControllerDigitalActionHandle;

    public int CompareTo(ControllerDigitalActionHandle_t other) => this.m_ControllerDigitalActionHandle.CompareTo(other.m_ControllerDigitalActionHandle);
  }
}
