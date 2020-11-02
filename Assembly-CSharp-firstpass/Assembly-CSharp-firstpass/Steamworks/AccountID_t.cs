// Decompiled with JetBrains decompiler
// Type: Steamworks.AccountID_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [Serializable]
  public struct AccountID_t : IEquatable<AccountID_t>, IComparable<AccountID_t>
  {
    public uint m_AccountID;

    public AccountID_t(uint value) => this.m_AccountID = value;

    public override string ToString() => this.m_AccountID.ToString();

    public override bool Equals(object other) => other is AccountID_t accountIdT && this == accountIdT;

    public override int GetHashCode() => this.m_AccountID.GetHashCode();

    public static bool operator ==(AccountID_t x, AccountID_t y) => (int) x.m_AccountID == (int) y.m_AccountID;

    public static bool operator !=(AccountID_t x, AccountID_t y) => !(x == y);

    public static explicit operator AccountID_t(uint value) => new AccountID_t(value);

    public static explicit operator uint(AccountID_t that) => that.m_AccountID;

    public bool Equals(AccountID_t other) => (int) this.m_AccountID == (int) other.m_AccountID;

    public int CompareTo(AccountID_t other) => this.m_AccountID.CompareTo(other.m_AccountID);
  }
}
