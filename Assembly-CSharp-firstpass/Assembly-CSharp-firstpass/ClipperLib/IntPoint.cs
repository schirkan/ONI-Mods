// Decompiled with JetBrains decompiler
// Type: ClipperLib.IntPoint
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace ClipperLib
{
  public struct IntPoint
  {
    public long X;
    public long Y;

    public IntPoint(long X, long Y)
    {
      this.X = X;
      this.Y = Y;
    }

    public IntPoint(double x, double y)
    {
      this.X = (long) x;
      this.Y = (long) y;
    }

    public IntPoint(IntPoint pt)
    {
      this.X = pt.X;
      this.Y = pt.Y;
    }

    public static bool operator ==(IntPoint a, IntPoint b) => a.X == b.X && a.Y == b.Y;

    public static bool operator !=(IntPoint a, IntPoint b) => a.X != b.X || a.Y != b.Y;

    public override bool Equals(object obj) => obj != null && obj is IntPoint intPoint && this.X == intPoint.X && this.Y == intPoint.Y;

    public override int GetHashCode() => base.GetHashCode();
  }
}
