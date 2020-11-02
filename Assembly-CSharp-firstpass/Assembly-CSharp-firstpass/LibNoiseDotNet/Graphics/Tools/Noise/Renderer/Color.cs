// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Renderer.Color
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Renderer
{
  public class Color : IEquatable<Color>, IColor
  {
    protected byte _red;
    protected byte _green;
    protected byte _blue;
    protected byte _alpha = byte.MaxValue;
    protected int _hashcode;
    private static Random _rnd = new Random(666);

    public byte Red
    {
      get => this._red;
      set => this._red = value;
    }

    public byte Green
    {
      get => this._green;
      set => this._green = value;
    }

    public byte Blue
    {
      get => this._blue;
      set => this._blue = value;
    }

    public byte Alpha
    {
      get => this._alpha;
      set => this._alpha = value;
    }

    public static Color BLACK => new Color((byte) 0, (byte) 0, (byte) 0, byte.MaxValue);

    public static Color WHITE => new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

    public static Color RED => new Color(byte.MaxValue, (byte) 0, (byte) 0, byte.MaxValue);

    public static Color GREEN => new Color((byte) 0, byte.MaxValue, (byte) 0, byte.MaxValue);

    public static Color BLUE => new Color((byte) 0, (byte) 0, byte.MaxValue, byte.MaxValue);

    public static Color TRANSPARENT => new Color((byte) 0, (byte) 0, (byte) 0, (byte) 0);

    public Color() => this._hashcode = (int) this._red + (int) this._green + (int) this._blue ^ Color._rnd.Next();

    public Color(byte r, byte g, byte b, byte a)
      : this()
    {
      this._red = r;
      this._green = g;
      this._blue = b;
      this._alpha = a;
    }

    public Color(byte r, byte g, byte b)
      : this()
    {
      this._red = r;
      this._green = g;
      this._blue = b;
      this._alpha = byte.MaxValue;
    }

    public bool Equals(Color other) => (int) this._red == (int) other.Red && (int) this._green == (int) other.Green && (int) this._blue == (int) other.Blue && (int) this._alpha == (int) other.Alpha;

    public static IColor Lerp(IColor color0, IColor color1, float t, bool withAlphaChannel)
    {
      IColor instance = (IColor) Activator.CreateInstance(color0.GetType());
      instance.Red = Libnoise.Lerp(color0.Red, color1.Red, t);
      instance.Green = Libnoise.Lerp(color0.Green, color1.Green, t);
      instance.Blue = Libnoise.Lerp(color0.Blue, color1.Blue, t);
      instance.Alpha = withAlphaChannel ? Libnoise.Lerp(color0.Alpha, color1.Alpha, t) : byte.MaxValue;
      return instance;
    }

    public static IColor Lerp(IColor color0, IColor color1, float t) => Color.Lerp(color0, color1, t, true);

    public static IColor Lerp32(IColor color0, IColor color1, float t) => Color.Lerp(color0, color1, t, true);

    public static IColor Lerp24(IColor color0, IColor color1, float t) => Color.Lerp(color0, color1, t, false);

    public static IColor Grayscale(IColor color)
    {
      IColor instance = (IColor) Activator.CreateInstance(color.GetType());
      int num1;
      byte num2 = (byte) (num1 = (int) Color.GrayscaleLuminosityStrategy(color));
      instance.Blue = (byte) num1;
      int num3;
      byte num4 = (byte) (num3 = (int) num2);
      instance.Green = (byte) num3;
      instance.Red = num4;
      instance.Alpha = byte.MaxValue;
      return instance;
    }

    public static IColor Grayscale(IColor color, Color.GrayscaleStrategy Strategy)
    {
      IColor instance = (IColor) Activator.CreateInstance(color.GetType());
      int num1;
      byte num2 = (byte) (num1 = (int) Strategy(color));
      instance.Blue = (byte) num1;
      int num3;
      byte num4 = (byte) (num3 = (int) num2);
      instance.Green = (byte) num3;
      instance.Red = num4;
      instance.Alpha = byte.MaxValue;
      return instance;
    }

    public static byte GrayscaleLightnessStrategy(IColor color) => (byte) (((int) Math.Max(color.Red, Math.Max(color.Green, color.Blue)) + (int) Math.Min(color.Red, Math.Max(color.Green, color.Blue))) / 2);

    public static byte GrayscaleAverageStrategy(IColor color) => (byte) (((int) color.Red + (int) color.Green + (int) color.Blue) / 3);

    public static byte GrayscaleLuminosityStrategy(IColor color) => (byte) (0.209999993443489 * (double) color.Red + 0.709999978542328 * (double) color.Green + 0.0700000002980232 * (double) color.Blue);

    public override string ToString() => string.Format("Color({0},{1},{2},{3})", (object) this.Red, (object) this.Green, (object) this.Blue, (object) this.Alpha);

    public override bool Equals(object other) => other is IColor && (int) this._red == (int) ((IColor) other).Red && ((int) this._green == (int) ((IColor) other).Green && (int) this._blue == (int) ((IColor) other).Blue) && (int) this._alpha == (int) ((IColor) other).Alpha;

    public override int GetHashCode() => this._hashcode;

    public static bool operator ==(Color a, IColor b) => a.Equals((object) b);

    public static bool operator !=(Color a, IColor b) => !a.Equals((object) b);

    public static bool operator >(Color a, IColor b) => (int) a._red > (int) b.Red && (int) a._green > (int) b.Green && (int) a._blue > (int) b.Blue && (int) a._alpha > (int) b.Alpha;

    public static bool operator <(Color a, IColor b) => (int) a._red < (int) b.Red && (int) a._green < (int) b.Green && (int) a._blue < (int) b.Blue && (int) a._alpha < (int) b.Alpha;

    public static bool operator >=(Color a, IColor b) => a > b || a == b;

    public static bool operator <=(Color a, IColor b) => a < b || a == b;

    public delegate byte GrayscaleStrategy(IColor color);
  }
}
