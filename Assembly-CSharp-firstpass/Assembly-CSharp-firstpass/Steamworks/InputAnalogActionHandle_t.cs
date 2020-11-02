// Decompiled with JetBrains decompiler
// Type: Steamworks.InputAnalogActionHandle_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [Serializable]
  public struct InputAnalogActionHandle_t : IEquatable<InputAnalogActionHandle_t>, IComparable<InputAnalogActionHandle_t>
  {
    public ulong m_InputAnalogActionHandle;

    public InputAnalogActionHandle_t(ulong value) => this.m_InputAnalogActionHandle = value;

    public override string ToString() => this.m_InputAnalogActionHandle.ToString();

    public override bool Equals(object other) => other is InputAnalogActionHandle_t analogActionHandleT && this == analogActionHandleT;

    public override int GetHashCode() => this.m_InputAnalogActionHandle.GetHashCode();

    public static bool operator ==(InputAnalogActionHandle_t x, InputAnalogActionHandle_t y) => (long) x.m_InputAnalogActionHandle == (long) y.m_InputAnalogActionHandle;

    public static bool operator !=(InputAnalogActionHandle_t x, InputAnalogActionHandle_t y) => !(x == y);

    public static explicit operator InputAnalogActionHandle_t(ulong value) => new InputAnalogActionHandle_t(value);

    public static explicit operator ulong(InputAnalogActionHandle_t that) => that.m_InputAnalogActionHandle;

    public bool Equals(InputAnalogActionHandle_t other) => (long) this.m_InputAnalogActionHandle == (long) other.m_InputAnalogActionHandle;

    public int CompareTo(InputAnalogActionHandle_t other) => this.m_InputAnalogActionHandle.CompareTo(other.m_InputAnalogActionHandle);
  }
}
