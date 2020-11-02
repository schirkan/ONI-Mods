// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Libnoise
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace LibNoiseDotNet.Graphics.Tools.Noise
{
  public static class Libnoise
  {
    public const string VERSION = "1.0.0 B";
    public const float PI = 3.141593f;
    public const float SQRT_2 = 1.414214f;
    public const float SQRT_3 = 1.732051f;
    public const float SQRT_5 = 2.236068f;
    public const float DEG2RAD = 0.01745329f;
    public const float RAD2DEG = 57.29578f;

    public static void LatLonToXYZ(float lat, float lon, ref float x, ref float y, ref float z)
    {
      float num = (float) Math.Cos(Math.PI / 180.0 * (double) lat);
      x = num * (float) Math.Cos(Math.PI / 180.0 * (double) lon);
      y = (float) Math.Sin(Math.PI / 180.0 * (double) lat);
      z = num * (float) Math.Sin(Math.PI / 180.0 * (double) lon);
    }

    public static byte Lerp(byte n0, byte n1, float a)
    {
      float num1 = (float) n0 / (float) byte.MaxValue;
      float num2 = (float) n1 / (float) byte.MaxValue;
      return (byte) (((double) num1 + (double) a * ((double) num2 - (double) num1)) * (double) byte.MaxValue);
    }

    public static float Lerp(float n0, float n1, float a) => n0 + a * (n1 - n0);

    public static float Cerp(float n0, float n1, float n2, float n3, float a)
    {
      float num1 = (float) ((double) n3 - (double) n2 - ((double) n0 - (double) n1));
      float num2 = n0 - n1 - num1;
      float num3 = n2 - n0;
      float num4 = n1;
      return (float) ((double) num1 * (double) a * (double) a * (double) a + (double) num2 * (double) a * (double) a + (double) num3 * (double) a) + num4;
    }

    public static float SCurve3(float a) => (float) ((double) a * (double) a * (3.0 - 2.0 * (double) a));

    public static float SCurve5(float a) => (float) ((double) a * (double) a * (double) a * ((double) a * ((double) a * 6.0 - 15.0) + 10.0));

    public static int Clamp(int value, int lowerBound, int upperBound)
    {
      if (value < lowerBound)
        return lowerBound;
      return value > upperBound ? upperBound : value;
    }

    public static float Clamp(float value, float lowerBound, float upperBound)
    {
      if ((double) value < (double) lowerBound)
        return lowerBound;
      return (double) value > (double) upperBound ? upperBound : value;
    }

    public static double Clamp(double value, double lowerBound, double upperBound)
    {
      if (value < lowerBound)
        return lowerBound;
      return value > upperBound ? upperBound : value;
    }

    public static int Clamp01(int value) => Libnoise.Clamp(value, 0, 1);

    public static float Clamp01(float value) => Libnoise.Clamp(value, 0.0f, 1f);

    public static double Clamp01(double value) => Libnoise.Clamp(value, 0.0, 1.0);

    public static void SwapValues<T>(ref T a, ref T b)
    {
      T obj = a;
      a = b;
      b = obj;
    }

    public static void SwapValues(ref double a, ref double b) => Libnoise.SwapValues<double>(ref a, ref b);

    public static void SwapValues(ref int a, ref int b) => Libnoise.SwapValues<int>(ref a, ref b);

    public static void SwapValues(ref float a, ref float b) => Libnoise.SwapValues<float>(ref a, ref b);

    public static double ToInt32Range(double value)
    {
      if (value >= 1073741824.0)
        return 2.0 * Math.IEEERemainder(value, 1073741824.0) - 1073741824.0;
      return value <= -1073741824.0 ? 2.0 * Math.IEEERemainder(value, 1073741824.0) + 1073741824.0 : value;
    }

    public static byte[] UnpackBigUint32(int value, ref byte[] buffer)
    {
      if (buffer.Length < 4)
        Array.Resize<byte>(ref buffer, 4);
      buffer[0] = (byte) (value >> 24);
      buffer[1] = (byte) (value >> 16);
      buffer[2] = (byte) (value >> 8);
      buffer[3] = (byte) value;
      return buffer;
    }

    public static byte[] UnpackBigFloat(float value, ref byte[] buffer) => throw new NotImplementedException();

    public static byte[] UnpackBigUint16(short value, ref byte[] buffer)
    {
      if (buffer.Length < 2)
        Array.Resize<byte>(ref buffer, 2);
      buffer[0] = (byte) ((uint) value >> 8);
      buffer[1] = (byte) value;
      return buffer;
    }

    public static byte[] UnpackLittleUint16(short value, ref byte[] buffer)
    {
      if (buffer.Length < 2)
        Array.Resize<byte>(ref buffer, 2);
      buffer[0] = (byte) ((uint) value & (uint) byte.MaxValue);
      buffer[1] = (byte) (((int) value & 65280) >> 8);
      return buffer;
    }

    public static byte[] UnpackLittleUint32(int value, ref byte[] buffer)
    {
      if (buffer.Length < 4)
        Array.Resize<byte>(ref buffer, 4);
      buffer[0] = (byte) (value & (int) byte.MaxValue);
      buffer[1] = (byte) ((value & 65280) >> 8);
      buffer[2] = (byte) ((value & 16711680) >> 16);
      buffer[3] = (byte) (((long) value & 4278190080L) >> 24);
      return buffer;
    }

    public static byte[] UnpackLittleFloat(float value, ref byte[] buffer) => throw new NotImplementedException();

    public static int FastFloor(double x) => x < 0.0 ? (int) x - 1 : (int) x;

    public static int FastFloor(float x) => (double) x < 0.0 ? (int) x - 1 : (int) x;
  }
}
