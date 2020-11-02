// Decompiled with JetBrains decompiler
// Type: ClipperLib.DoublePoint
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace ClipperLib
{
  public struct DoublePoint
  {
    public double X;
    public double Y;

    public DoublePoint(double x = 0.0, double y = 0.0)
    {
      this.X = x;
      this.Y = y;
    }

    public DoublePoint(DoublePoint dp)
    {
      this.X = dp.X;
      this.Y = dp.Y;
    }

    public DoublePoint(IntPoint ip)
    {
      this.X = (double) ip.X;
      this.Y = (double) ip.Y;
    }
  }
}
