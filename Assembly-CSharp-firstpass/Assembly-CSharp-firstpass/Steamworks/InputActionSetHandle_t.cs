// Decompiled with JetBrains decompiler
// Type: Steamworks.InputActionSetHandle_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [Serializable]
  public struct InputActionSetHandle_t : IEquatable<InputActionSetHandle_t>, IComparable<InputActionSetHandle_t>
  {
    public ulong m_InputActionSetHandle;

    public InputActionSetHandle_t(ulong value) => this.m_InputActionSetHandle = value;

    public override string ToString() => this.m_InputActionSetHandle.ToString();

    public override bool Equals(object other) => other is InputActionSetHandle_t actionSetHandleT && this == actionSetHandleT;

    public override int GetHashCode() => this.m_InputActionSetHandle.GetHashCode();

    public static bool operator ==(InputActionSetHandle_t x, InputActionSetHandle_t y) => (long) x.m_InputActionSetHandle == (long) y.m_InputActionSetHandle;

    public static bool operator !=(InputActionSetHandle_t x, InputActionSetHandle_t y) => !(x == y);

    public static explicit operator InputActionSetHandle_t(ulong value) => new InputActionSetHandle_t(value);

    public static explicit operator ulong(InputActionSetHandle_t that) => that.m_InputActionSetHandle;

    public bool Equals(InputActionSetHandle_t other) => (long) this.m_InputActionSetHandle == (long) other.m_InputActionSetHandle;

    public int CompareTo(InputActionSetHandle_t other) => this.m_InputActionSetHandle.CompareTo(other.m_InputActionSetHandle);
  }
}
