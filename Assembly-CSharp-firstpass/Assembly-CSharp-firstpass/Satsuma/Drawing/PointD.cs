// Decompiled with JetBrains decompiler
// Type: Satsuma.Drawing.PointD
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Globalization;

namespace Satsuma.Drawing
{
  public struct PointD : IEquatable<PointD>
  {
    public double X { get; private set; }

    public double Y { get; private set; }

    public PointD(double x, double y)
      : this()
    {
      this.X = x;
      this.Y = y;
    }

    public bool Equals(PointD other) => this.X == other.X && this.Y == other.Y;

    public override bool Equals(object obj) => obj is PointD other && this.Equals(other);

    public static bool operator ==(PointD a, PointD b) => a.Equals(b);

    public static bool operator !=(PointD a, PointD b) => !(a == b);

    public override int GetHashCode()
    {
      double num1 = this.X;
      int num2 = num1.GetHashCode() * 17;
      num1 = this.Y;
      int hashCode = num1.GetHashCode();
      return num2 + hashCode;
    }

    public string ToString(IFormatProvider provider) => string.Format(provider, "({0} {1})", (object) this.X, (object) this.Y);

    public override string ToString() => this.ToString((IFormatProvider) CultureInfo.CurrentCulture);

    public static PointD operator +(PointD a, PointD b) => new PointD(a.X + b.X, a.Y + b.Y);

    public static PointD Add(PointD a, PointD b) => a + b;

    public double Distance(PointD other) => Math.Sqrt((this.X - other.X) * (this.X - other.X) + (this.Y - other.Y) * (this.Y - other.Y));
  }
}
