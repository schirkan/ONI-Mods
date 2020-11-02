// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Renderer.GradientColor
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Renderer
{
  public class GradientColor
  {
    protected List<GradientPoint> _gradientPoints = new List<GradientPoint>(10);

    public static GradientColor GRAYSCALE
    {
      get
      {
        GradientColor gradientColor = new GradientColor();
        gradientColor.AddGradientPoint(-1f, (IColor) Color.BLACK);
        gradientColor.AddGradientPoint(1f, (IColor) Color.WHITE);
        return gradientColor;
      }
    }

    public static GradientColor EMPTY
    {
      get
      {
        GradientColor gradientColor = new GradientColor();
        gradientColor.AddGradientPoint(-1f, (IColor) Color.TRANSPARENT);
        gradientColor.AddGradientPoint(1f, (IColor) Color.TRANSPARENT);
        return gradientColor;
      }
    }

    public static GradientColor TERRAIN
    {
      get
      {
        GradientColor gradientColor = new GradientColor();
        gradientColor.AddGradientPoint(-1f, (IColor) new Color((byte) 0, (byte) 0, (byte) 128, byte.MaxValue));
        gradientColor.AddGradientPoint(-0.25f, (IColor) new Color((byte) 0, (byte) 0, byte.MaxValue, byte.MaxValue));
        gradientColor.AddGradientPoint(0.0f, (IColor) new Color((byte) 0, (byte) 128, byte.MaxValue, byte.MaxValue));
        gradientColor.AddGradientPoint(1f / 16f, (IColor) new Color((byte) 240, (byte) 240, (byte) 64, byte.MaxValue));
        gradientColor.AddGradientPoint(0.125f, (IColor) new Color((byte) 32, (byte) 160, (byte) 0, byte.MaxValue));
        gradientColor.AddGradientPoint(0.375f, (IColor) new Color((byte) 224, (byte) 224, (byte) 0, byte.MaxValue));
        gradientColor.AddGradientPoint(0.75f, (IColor) new Color((byte) 128, (byte) 128, (byte) 128, byte.MaxValue));
        gradientColor.AddGradientPoint(1f, (IColor) new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
        return gradientColor;
      }
    }

    public GradientColor()
    {
    }

    public GradientColor(IColor color)
    {
      this.AddGradientPoint(-1f, color);
      this.AddGradientPoint(1f, color);
    }

    public GradientColor(IColor start, IColor end)
    {
      this.AddGradientPoint(-1f, start);
      this.AddGradientPoint(1f, end);
    }

    public void AddGradientPoint(float position, IColor color) => this.AddGradientPoint(new GradientPoint(position, color));

    public void AddGradientPoint(GradientPoint point)
    {
      if (this._gradientPoints.Contains(point))
        throw new ArgumentException(string.Format("Cannont insert GradientPoint({0}, {1}) : Each GradientPoint is required to contain a unique position", (object) point.Position, (object) point.Color));
      this._gradientPoints.Add(point);
      this._gradientPoints.Sort((Comparison<GradientPoint>) ((p1, p2) =>
      {
        if ((double) p1.Position > (double) p2.Position)
          return 1;
        return (double) p1.Position < (double) p2.Position ? -1 : 0;
      }));
    }

    public void Clear() => this._gradientPoints.Clear();

    public IColor GetColor(float position)
    {
      int index1 = 0;
      while (index1 < this._gradientPoints.Count && (double) position >= (double) this._gradientPoints[index1].Position)
        ++index1;
      int index2 = Libnoise.Clamp(index1 - 1, 0, this._gradientPoints.Count - 1);
      int index3 = Libnoise.Clamp(index1, 0, this._gradientPoints.Count - 1);
      if (index2 == index3)
        return this._gradientPoints[index3].Color;
      float position1 = this._gradientPoints[index2].Position;
      float position2 = this._gradientPoints[index3].Position;
      float t = (float) (((double) position - (double) position1) / ((double) position2 - (double) position1));
      return Color.Lerp(this._gradientPoints[index2].Color, this._gradientPoints[index3].Color, t);
    }

    public int CountGradientPoints() => this._gradientPoints.Count;

    public IList<GradientPoint> getGradientPoints() => (IList<GradientPoint>) this._gradientPoints.AsReadOnly();
  }
}
