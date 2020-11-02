// Decompiled with JetBrains decompiler
// Type: Steamworks.ControllerAnalogActionHandle_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [Serializable]
  public struct ControllerAnalogActionHandle_t : IEquatable<ControllerAnalogActionHandle_t>, IComparable<ControllerAnalogActionHandle_t>
  {
    public ulong m_ControllerAnalogActionHandle;

    public ControllerAnalogActionHandle_t(ulong value) => this.m_ControllerAnalogActionHandle = value;

    public override string ToString() => this.m_ControllerAnalogActionHandle.ToString();

    public override bool Equals(object other) => other is ControllerAnalogActionHandle_t analogActionHandleT && this == analogActionHandleT;

    public override int GetHashCode() => this.m_ControllerAnalogActionHandle.GetHashCode();

    public static bool operator ==(
      ControllerAnalogActionHandle_t x,
      ControllerAnalogActionHandle_t y)
    {
      return (long) x.m_ControllerAnalogActionHandle == (long) y.m_ControllerAnalogActionHandle;
    }

    public static bool operator !=(
      ControllerAnalogActionHandle_t x,
      ControllerAnalogActionHandle_t y)
    {
      return !(x == y);
    }

    public static explicit operator ControllerAnalogActionHandle_t(
      ulong value)
    {
      return new ControllerAnalogActionHandle_t(value);
    }

    public static explicit operator ulong(ControllerAnalogActionHandle_t that) => that.m_ControllerAnalogActionHandle;

    public bool Equals(ControllerAnalogActionHandle_t other) => (long) this.m_ControllerAnalogActionHandle == (long) other.m_ControllerAnalogActionHandle;

    public int CompareTo(ControllerAnalogActionHandle_t other) => this.m_ControllerAnalogActionHandle.CompareTo(other.m_ControllerAnalogActionHandle);
  }
}
