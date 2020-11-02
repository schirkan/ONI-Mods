// Decompiled with JetBrains decompiler
// Type: ClipperLib.Int128
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace ClipperLib
{
  internal struct Int128
  {
    private long hi;
    private ulong lo;

    public Int128(long _lo)
    {
      this.lo = (ulong) _lo;
      if (_lo < 0L)
        this.hi = -1L;
      else
        this.hi = 0L;
    }

    public Int128(long _hi, ulong _lo)
    {
      this.lo = _lo;
      this.hi = _hi;
    }

    public Int128(Int128 val)
    {
      this.hi = val.hi;
      this.lo = val.lo;
    }

    public bool IsNegative() => this.hi < 0L;

    public static bool operator ==(Int128 val1, Int128 val2)
    {
      if ((ValueType) val1 == (ValueType) val2)
        return true;
      return (ValueType) val1 != null && (ValueType) val2 != null && val1.hi == val2.hi && (long) val1.lo == (long) val2.lo;
    }

    public static bool operator !=(Int128 val1, Int128 val2) => !(val1 == val2);

    public override bool Equals(object obj) => obj != null && obj is Int128 int128 && int128.hi == this.hi && (long) int128.lo == (long) this.lo;

    public override int GetHashCode() => this.hi.GetHashCode() ^ this.lo.GetHashCode();

    public static bool operator >(Int128 val1, Int128 val2) => val1.hi != val2.hi ? val1.hi > val2.hi : val1.lo > val2.lo;

    public static bool operator <(Int128 val1, Int128 val2) => val1.hi != val2.hi ? val1.hi < val2.hi : val1.lo < val2.lo;

    public static Int128 operator +(Int128 lhs, Int128 rhs)
    {
      lhs.hi += rhs.hi;
      lhs.lo += rhs.lo;
      if (lhs.lo < rhs.lo)
        ++lhs.hi;
      return lhs;
    }

    public static Int128 operator -(Int128 lhs, Int128 rhs) => lhs + -rhs;

    public static Int128 operator -(Int128 val) => val.lo == 0UL ? new Int128(-val.hi, 0UL) : new Int128(~val.hi, ~val.lo + 1UL);

    public static explicit operator double(Int128 val)
    {
      if (val.hi >= 0L)
        return (double) val.lo + (double) val.hi * 1.84467440737096E+19;
      return val.lo == 0UL ? (double) val.hi * 1.84467440737096E+19 : -((double) ~val.lo + (double) ~val.hi * 1.84467440737096E+19);
    }

    public static Int128 Int128Mul(long lhs, long rhs)
    {
      int num1 = lhs < 0L != rhs < 0L ? 1 : 0;
      if (lhs < 0L)
        lhs = -lhs;
      if (rhs < 0L)
        rhs = -rhs;
      long num2 = (long) ((ulong) lhs >> 32);
      ulong num3 = (ulong) lhs & (ulong) uint.MaxValue;
      ulong num4 = (ulong) rhs >> 32;
      ulong num5 = (ulong) rhs & (ulong) uint.MaxValue;
      ulong num6 = (ulong) num2 * num4;
      ulong num7 = num3 * num5;
      ulong num8 = (ulong) (num2 * (long) num5 + (long) num3 * (long) num4);
      long _hi = (long) num6 + (long) (num8 >> 32);
      ulong _lo = (num8 << 32) + num7;
      if (_lo < num7)
        ++_hi;
      Int128 int128 = new Int128(_hi, _lo);
      return num1 == 0 ? int128 : -int128;
    }
  }
}
