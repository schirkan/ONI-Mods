// Decompiled with JetBrains decompiler
// Type: HUSL.ColorConverter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace HUSL
{
  public class ColorConverter
  {
    protected static double[][] M = new double[3][]
    {
      new double[3]
      {
        3.24096994190452,
        -329.0 / 214.0,
        -0.498610760293
      },
      new double[3]
      {
        -0.96924363628087,
        1.87596750150772,
        0.041555057407175
      },
      new double[3]
      {
        0.055630079696993,
        -0.20397695888897,
        705.0 / 667.0
      }
    };
    protected static double[][] MInv = new double[3][]
    {
      new double[3]
      {
        0.41239079926595,
        0.35758433938387,
        0.18048078840183
      },
      new double[3]
      {
        0.21263900587151,
        0.71516867876775,
        0.072192315360733
      },
      new double[3]
      {
        0.019330818715591,
        0.11919477979462,
        0.95053215224966
      }
    };
    protected static double RefX = 0.95045592705167;
    protected static double RefY = 1.0;
    protected static double RefZ = 1.08905775075988;
    protected static double RefU = 0.19783000664283;
    protected static double RefV = 0.46831999493879;
    protected static double Kappa = 903.2962962;
    protected static double Epsilon = 0.0088564516;

    protected static IList<double[]> GetBounds(double L)
    {
      List<double[]> numArrayList = new List<double[]>();
      double num1 = Math.Pow(L + 16.0, 3.0) / 1560896.0;
      double num2 = num1 > ColorConverter.Epsilon ? num1 : L / ColorConverter.Kappa;
      for (int index1 = 0; index1 < 3; ++index1)
      {
        double num3 = ColorConverter.M[index1][0];
        double num4 = ColorConverter.M[index1][1];
        double num5 = ColorConverter.M[index1][2];
        for (int index2 = 0; index2 < 2; ++index2)
        {
          double num6 = (284517.0 * num3 - 94839.0 * num5) * num2;
          double num7 = (838422.0 * num5 + 769860.0 * num4 + 731718.0 * num3) * L * num2 - (double) (769860 * index2) * L;
          double num8 = (632260.0 * num5 - 126452.0 * num4) * num2 + (double) (126452 * index2);
          numArrayList.Add(new double[2]
          {
            num6 / num8,
            num7 / num8
          });
        }
      }
      return (IList<double[]>) numArrayList;
    }

    protected static double IntersectLineLine(IList<double> lineA, IList<double> lineB) => (lineA[1] - lineB[1]) / (lineB[0] - lineA[0]);

    protected static double DistanceFromPole(IList<double> point) => Math.Sqrt(Math.Pow(point[0], 2.0) + Math.Pow(point[1], 2.0));

    protected static bool LengthOfRayUntilIntersect(
      double theta,
      IList<double> line,
      out double length)
    {
      length = line[1] / (Math.Sin(theta) - line[0] * Math.Cos(theta));
      return length >= 0.0;
    }

    protected static double MaxSafeChromaForL(double L)
    {
      IList<double[]> bounds = ColorConverter.GetBounds(L);
      double val1 = double.MaxValue;
      for (int index = 0; index < 2; ++index)
      {
        double num1 = bounds[index][0];
        double num2 = bounds[index][1];
        double num3 = ColorConverter.IntersectLineLine((IList<double>) new double[2]
        {
          num1,
          num2
        }, (IList<double>) new double[2]
        {
          -1.0 / num1,
          0.0
        });
        double val2 = ColorConverter.DistanceFromPole((IList<double>) new double[2]
        {
          num3,
          num2 + num3 * num1
        });
        val1 = Math.Min(val1, val2);
      }
      return val1;
    }

    protected static double MaxChromaForLH(double L, double H)
    {
      double theta = H / 360.0 * Math.PI * 2.0;
      IList<double[]> bounds = ColorConverter.GetBounds(L);
      double val1 = double.MaxValue;
      foreach (double[] numArray in (IEnumerable<double[]>) bounds)
      {
        double length;
        if (ColorConverter.LengthOfRayUntilIntersect(theta, (IList<double>) numArray, out length))
          val1 = Math.Min(val1, length);
      }
      return val1;
    }

    protected static double DotProduct(IList<double> a, IList<double> b)
    {
      double num = 0.0;
      for (int index = 0; index < a.Count; ++index)
        num += a[index] * b[index];
      return num;
    }

    protected static double Round(double value, int places)
    {
      double num = Math.Pow(10.0, (double) places);
      return Math.Round(value * num) / num;
    }

    protected static double FromLinear(double c) => c <= 0.0031308 ? 12.92 * c : 1.055 * Math.Pow(c, 5.0 / 12.0) - 0.055;

    protected static double ToLinear(double c) => c > 0.04045 ? Math.Pow((c + 0.055) / 1.055, 2.4) : c / 12.92;

    protected static IList<int> RGBPrepare(IList<double> tuple)
    {
      for (int index = 0; index < tuple.Count; ++index)
        tuple[index] = ColorConverter.Round(tuple[index], 3);
      for (int index = 0; index < tuple.Count; ++index)
      {
        double num = tuple[index];
        if (num < -0.0001 || num > 1.0001)
          throw new Exception("Illegal rgb value: " + (object) num);
      }
      int[] numArray = new int[tuple.Count];
      for (int index = 0; index < tuple.Count; ++index)
        numArray[index] = (int) Math.Round(tuple[index] * (double) byte.MaxValue);
      return (IList<int>) numArray;
    }

    protected static double YToL(double Y) => Y <= ColorConverter.Epsilon ? Y / ColorConverter.RefY * ColorConverter.Kappa : 116.0 * Math.Pow(Y / ColorConverter.RefY, 1.0 / 3.0) - 16.0;

    protected static double LToY(double L) => L <= 8.0 ? ColorConverter.RefY * L / ColorConverter.Kappa : ColorConverter.RefY * Math.Pow((L + 16.0) / 116.0, 3.0);

    public static IList<double> XYZToRGB(IList<double> tuple) => (IList<double>) new double[3]
    {
      ColorConverter.FromLinear(ColorConverter.DotProduct((IList<double>) ColorConverter.M[0], tuple)),
      ColorConverter.FromLinear(ColorConverter.DotProduct((IList<double>) ColorConverter.M[1], tuple)),
      ColorConverter.FromLinear(ColorConverter.DotProduct((IList<double>) ColorConverter.M[2], tuple))
    };

    public static IList<double> RGBToXYZ(IList<double> tuple)
    {
      double[] numArray = new double[3]
      {
        ColorConverter.ToLinear(tuple[0]),
        ColorConverter.ToLinear(tuple[1]),
        ColorConverter.ToLinear(tuple[2])
      };
      return (IList<double>) new double[3]
      {
        ColorConverter.DotProduct((IList<double>) ColorConverter.MInv[0], (IList<double>) numArray),
        ColorConverter.DotProduct((IList<double>) ColorConverter.MInv[1], (IList<double>) numArray),
        ColorConverter.DotProduct((IList<double>) ColorConverter.MInv[2], (IList<double>) numArray)
      };
    }

    public static IList<double> XYZToLUV(IList<double> tuple)
    {
      double num1 = tuple[0];
      double Y = tuple[1];
      double num2 = tuple[2];
      double num3 = 4.0 * num1 / (num1 + 15.0 * Y + 3.0 * num2);
      double num4 = 9.0 * Y / (num1 + 15.0 * Y + 3.0 * num2);
      double num5 = ColorConverter.YToL(Y);
      if (num5 == 0.0)
        return (IList<double>) new double[3];
      double num6 = 13.0 * num5 * (num3 - ColorConverter.RefU);
      double num7 = 13.0 * num5 * (num4 - ColorConverter.RefV);
      return (IList<double>) new double[3]
      {
        num5,
        num6,
        num7
      };
    }

    public static IList<double> LUVToXYZ(IList<double> tuple)
    {
      double L = tuple[0];
      double num1 = tuple[1];
      double num2 = tuple[2];
      if (L == 0.0)
        return (IList<double>) new double[3];
      double num3 = num1 / (13.0 * L) + ColorConverter.RefU;
      double num4 = num2 / (13.0 * L) + ColorConverter.RefV;
      double num5 = ColorConverter.LToY(L);
      double num6 = 0.0 - 9.0 * num5 * num3 / ((num3 - 4.0) * num4 - num3 * num4);
      double num7 = (9.0 * num5 - 15.0 * num4 * num5 - num4 * num6) / (3.0 * num4);
      return (IList<double>) new double[3]
      {
        num6,
        num5,
        num7
      };
    }

    public static IList<double> LUVToLCH(IList<double> tuple)
    {
      double num1 = tuple[0];
      double x = tuple[1];
      double num2 = tuple[2];
      double num3 = Math.Pow(Math.Pow(x, 2.0) + Math.Pow(num2, 2.0), 0.5);
      double num4 = Math.Atan2(num2, x) * 180.0 / Math.PI;
      if (num4 < 0.0)
        num4 = 360.0 + num4;
      return (IList<double>) new double[3]
      {
        num1,
        num3,
        num4
      };
    }

    public static IList<double> LCHToLUV(IList<double> tuple)
    {
      double num1 = tuple[0];
      double num2 = tuple[1];
      double num3 = tuple[2] / 360.0 * 2.0 * Math.PI;
      double num4 = Math.Cos(num3) * num2;
      double num5 = Math.Sin(num3) * num2;
      return (IList<double>) new double[3]
      {
        num1,
        num4,
        num5
      };
    }

    public static IList<double> HUSLToLCH(IList<double> tuple)
    {
      double H = tuple[0];
      double num1 = tuple[1];
      double L = tuple[2];
      if (L > 99.9999999)
        return (IList<double>) new double[3]
        {
          100.0,
          0.0,
          H
        };
      if (L < 1E-08)
        return (IList<double>) new double[3]
        {
          0.0,
          0.0,
          H
        };
      double num2 = ColorConverter.MaxChromaForLH(L, H) / 100.0 * num1;
      return (IList<double>) new double[3]
      {
        L,
        num2,
        H
      };
    }

    public static IList<double> LCHToHUSL(IList<double> tuple)
    {
      double L = tuple[0];
      double num1 = tuple[1];
      double H = tuple[2];
      if (L > 99.9999999)
        return (IList<double>) new double[3]
        {
          H,
          0.0,
          100.0
        };
      if (L < 1E-08)
        return (IList<double>) new double[3]
        {
          H,
          0.0,
          0.0
        };
      double num2 = ColorConverter.MaxChromaForLH(L, H);
      double num3 = num1 / num2 * 100.0;
      return (IList<double>) new double[3]
      {
        H,
        num3,
        L
      };
    }

    public static IList<double> HUSLPToLCH(IList<double> tuple)
    {
      double num1 = tuple[0];
      double num2 = tuple[1];
      double L = tuple[2];
      if (L > 99.9999999)
        return (IList<double>) new double[3]
        {
          100.0,
          0.0,
          num1
        };
      if (L < 1E-08)
        return (IList<double>) new double[3]
        {
          0.0,
          0.0,
          num1
        };
      double num3 = ColorConverter.MaxSafeChromaForL(L) / 100.0 * num2;
      return (IList<double>) new double[3]
      {
        L,
        num3,
        num1
      };
    }

    public static IList<double> LCHToHUSLP(IList<double> tuple)
    {
      double L = tuple[0];
      double num1 = tuple[1];
      double num2 = tuple[2];
      if (L > 99.9999999)
        return (IList<double>) new double[3]
        {
          num2,
          0.0,
          100.0
        };
      if (L < 1E-08)
        return (IList<double>) new double[3]
        {
          num2,
          0.0,
          0.0
        };
      double num3 = ColorConverter.MaxSafeChromaForL(L);
      double num4 = num1 / num3 * 100.0;
      return (IList<double>) new double[3]
      {
        num2,
        num4,
        L
      };
    }

    public static string RGBToHex(IList<double> tuple)
    {
      IList<int> intList = ColorConverter.RGBPrepare(tuple);
      int num = intList[0];
      string str1 = num.ToString("x2");
      num = intList[1];
      string str2 = num.ToString("x2");
      num = intList[2];
      string str3 = num.ToString("x2");
      return string.Format("#{0}{1}{2}", (object) str1, (object) str2, (object) str3);
    }

    public static IList<double> HexToRGB(string hex) => (IList<double>) new double[3]
    {
      (double) int.Parse(hex.Substring(1, 2), NumberStyles.HexNumber) / (double) byte.MaxValue,
      (double) int.Parse(hex.Substring(3, 2), NumberStyles.HexNumber) / (double) byte.MaxValue,
      (double) int.Parse(hex.Substring(5, 2), NumberStyles.HexNumber) / (double) byte.MaxValue
    };

    public static IList<double> LCHToRGB(IList<double> tuple) => ColorConverter.XYZToRGB(ColorConverter.LUVToXYZ(ColorConverter.LCHToLUV(tuple)));

    public static IList<double> RGBToLCH(IList<double> tuple) => ColorConverter.LUVToLCH(ColorConverter.XYZToLUV(ColorConverter.RGBToXYZ(tuple)));

    public static IList<double> HUSLToRGB(IList<double> tuple) => ColorConverter.LCHToRGB(ColorConverter.HUSLToLCH(tuple));

    public static IList<double> RGBToHUSL(IList<double> tuple) => ColorConverter.LCHToHUSL(ColorConverter.RGBToLCH(tuple));

    public static IList<double> HUSLPToRGB(IList<double> tuple) => ColorConverter.LCHToRGB(ColorConverter.HUSLPToLCH(tuple));

    public static IList<double> RGBToHUSLP(IList<double> tuple) => ColorConverter.LCHToHUSLP(ColorConverter.RGBToLCH(tuple));

    public static string HUSLToHex(IList<double> tuple) => ColorConverter.RGBToHex(ColorConverter.HUSLToRGB(tuple));

    public static string HUSLPToHex(IList<double> tuple) => ColorConverter.RGBToHex(ColorConverter.HUSLPToRGB(tuple));

    public static IList<double> HexToHUSL(string s) => ColorConverter.RGBToHUSL(ColorConverter.HexToRGB(s));

    public static IList<double> HexToHUSLP(string s) => ColorConverter.RGBToHUSLP(ColorConverter.HexToRGB(s));

    public static Color HUSLToColor(float h, float s, float l)
    {
      IList<double> rgb = ColorConverter.HUSLToRGB((IList<double>) new List<double>((IEnumerable<double>) new double[3]
      {
        (double) h,
        (double) s,
        (double) l
      }));
      return new Color((float) rgb[0], (float) rgb[1], (float) rgb[2]);
    }

    public static Color HUSLPToColor(float h, float s, float l)
    {
      IList<double> rgb = ColorConverter.HUSLPToRGB((IList<double>) new List<double>((IEnumerable<double>) new double[3]
      {
        (double) h,
        (double) s,
        (double) l
      }));
      return new Color((float) rgb[0], (float) rgb[1], (float) rgb[2]);
    }
  }
}
