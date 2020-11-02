// Decompiled with JetBrains decompiler
// Type: Steamworks.InputDigitalActionHandle_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [Serializable]
  public struct InputDigitalActionHandle_t : IEquatable<InputDigitalActionHandle_t>, IComparable<InputDigitalActionHandle_t>
  {
    public ulong m_InputDigitalActionHandle;

    public InputDigitalActionHandle_t(ulong value) => this.m_InputDigitalActionHandle = value;

    public override string ToString() => this.m_InputDigitalActionHandle.ToString();

    public override bool Equals(object other) => other is InputDigitalActionHandle_t digitalActionHandleT && this == digitalActionHandleT;

    public override int GetHashCode() => this.m_InputDigitalActionHandle.GetHashCode();

    public static bool operator ==(InputDigitalActionHandle_t x, InputDigitalActionHandle_t y) => (long) x.m_InputDigitalActionHandle == (long) y.m_InputDigitalActionHandle;

    public static bool operator !=(InputDigitalActionHandle_t x, InputDigitalActionHandle_t y) => !(x == y);

    public static explicit operator InputDigitalActionHandle_t(ulong value) => new InputDigitalActionHandle_t(value);

    public static explicit operator ulong(InputDigitalActionHandle_t that) => that.m_InputDigitalActionHandle;

    public bool Equals(InputDigitalActionHandle_t other) => (long) this.m_InputDigitalActionHandle == (long) other.m_InputDigitalActionHandle;

    public int CompareTo(InputDigitalActionHandle_t other) => this.m_InputDigitalActionHandle.CompareTo(other.m_InputDigitalActionHandle);
  }
}
