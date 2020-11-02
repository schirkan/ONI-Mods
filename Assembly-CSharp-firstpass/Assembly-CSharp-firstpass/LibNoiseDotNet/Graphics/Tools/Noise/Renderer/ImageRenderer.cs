// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Renderer.ImageRenderer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Renderer
{
  public class ImageRenderer : AbstractImageRenderer
  {
    protected Image _backgroundImage;
    protected GradientColor _gradient;
    private bool _lightEnabled;
    private bool _WrapEnabled;
    private float _lightAzimuth;
    private float _lightBrightness;
    private float _lightContrast;
    private float _lightElevation;
    private float _lightIntensity;
    private Color _lightColor;
    private bool _recalcLightValues;
    private float _cosAzimuth;
    private float _cosElevation;
    private float _sinAzimuth;
    private float _sinElevation;

    public bool LightEnabled
    {
      get => this._lightEnabled;
      set => this._lightEnabled = value;
    }

    public bool WrapEnabled
    {
      get => this._WrapEnabled;
      set => this._WrapEnabled = value;
    }

    public Image BackgroundImage
    {
      get => this._backgroundImage;
      set => this._backgroundImage = value;
    }

    public GradientColor Gradient
    {
      get => this._gradient;
      set => this._gradient = value;
    }

    public float LightAzimuth
    {
      get => this._lightAzimuth;
      set
      {
        this._lightAzimuth = value;
        this._recalcLightValues = true;
      }
    }

    public float LightBrightness
    {
      get => this._lightBrightness;
      set
      {
        this._lightBrightness = value;
        this._recalcLightValues = true;
      }
    }

    public float LightContrast
    {
      get => this._lightContrast;
      set
      {
        this._lightContrast = (double) value > 0.0 ? value : throw new ArgumentException("Contrast must be greater than 0");
        this._recalcLightValues = true;
      }
    }

    public float LightElevation
    {
      get => this._lightElevation;
      set
      {
        this._lightElevation = value;
        this._recalcLightValues = true;
      }
    }

    public float LightIntensity
    {
      get => this._lightIntensity;
      set
      {
        this._lightIntensity = (double) value >= 0.0 ? value : throw new ArgumentException("Intensity must be greater or equals to 0");
        this._recalcLightValues = true;
      }
    }

    public Color LightColor
    {
      get => this._lightColor;
      set => this._lightColor = value;
    }

    public ImageRenderer()
    {
      this._lightEnabled = false;
      this._WrapEnabled = false;
      this._lightAzimuth = 45f;
      this._lightBrightness = 1f;
      this._lightContrast = 1f;
      this._lightElevation = 45f;
      this._lightIntensity = 1f;
      this._lightColor = Color.WHITE;
      this._recalcLightValues = true;
    }

    public override void Render()
    {
      if (this._noiseMap == null)
        throw new ArgumentException("A noise map must be provided");
      if (this._image == null)
        throw new ArgumentException("An image map must be provided");
      if (this._noiseMap.Width <= 0 || this._noiseMap.Height <= 0)
        throw new ArgumentException("Incoherent noise map size (0,0)");
      if (this._gradient.CountGradientPoints() < 2)
        throw new ArgumentException("Not enought points in the gradient");
      int width = this._noiseMap.Width;
      int height = this._noiseMap.Height;
      int num1 = width - 1;
      int num2 = height - 1;
      int num3 = -num1;
      int num4 = -num2;
      if (this._backgroundImage != null && (this._backgroundImage.Width != width || this._backgroundImage.Height != height))
        throw new ArgumentException("Incoherent background image size");
      if (!this._image.Equals((object) this._backgroundImage))
        this._image.SetSize(width, height);
      IColor white = (IColor) Color.WHITE;
      for (int index = 0; index < height; ++index)
      {
        for (int x = 0; x < width; ++x)
        {
          IColor color = this._gradient.GetColor(this._noiseMap.GetValue(x, index));
          float lightValue;
          if (this._lightEnabled)
          {
            int num5;
            int num6;
            int num7;
            int num8;
            if (this._WrapEnabled)
            {
              if (x == 0)
              {
                num5 = num1;
                num6 = 1;
              }
              else if (x == num1)
              {
                num5 = -1;
                num6 = num3;
              }
              else
              {
                num5 = -1;
                num6 = 1;
              }
              if (index == 0)
              {
                num7 = num2;
                num8 = 1;
              }
              else if (index == num2)
              {
                num7 = -1;
                num8 = num4;
              }
              else
              {
                num7 = -1;
                num8 = 1;
              }
            }
            else
            {
              if (x == 0)
              {
                num5 = 0;
                num6 = 1;
              }
              else if (x == num1)
              {
                num5 = -1;
                num6 = 0;
              }
              else
              {
                num5 = -1;
                num6 = 1;
              }
              if (index == 0)
              {
                num7 = 0;
                num8 = 1;
              }
              else if (index == num2)
              {
                num7 = -1;
                num8 = 0;
              }
              else
              {
                num7 = -1;
                num8 = 1;
              }
            }
            lightValue = this.CalcLightIntensity(this._noiseMap.GetValue(x, index), this._noiseMap.GetValue(x + num5, index), this._noiseMap.GetValue(x + num6, index), this._noiseMap.GetValue(x, index + num7), this._noiseMap.GetValue(x, index + num8)) * this._lightBrightness;
          }
          else
            lightValue = 1f;
          if (this._backgroundImage != null)
            white = (IColor) this._backgroundImage.GetValue(x, index);
          this._image.SetValue(x, index, this.CalcDestColor(color, white, lightValue));
        }
        if (this._callBack != null)
          this._callBack(index);
      }
    }

    private IColor CalcDestColor(
      IColor sourceColor,
      IColor backgroundColor,
      float lightValue)
    {
      float num1 = (float) sourceColor.Red / (float) byte.MaxValue;
      float n1_1 = (float) sourceColor.Green / (float) byte.MaxValue;
      float n1_2 = (float) sourceColor.Blue / (float) byte.MaxValue;
      float a = (float) sourceColor.Alpha / (float) byte.MaxValue;
      double num2 = (double) backgroundColor.Red / (double) byte.MaxValue;
      float n0_1 = (float) backgroundColor.Green / (float) byte.MaxValue;
      float n0_2 = (float) backgroundColor.Blue / (float) byte.MaxValue;
      double num3 = (double) num1;
      double num4 = (double) a;
      float num5 = Libnoise.Lerp((float) num2, (float) num3, (float) num4);
      float num6 = Libnoise.Lerp(n0_1, n1_1, a);
      float num7 = Libnoise.Lerp(n0_2, n1_2, a);
      if (this._lightEnabled)
      {
        float num8 = (float) ((double) lightValue * (double) this._lightColor.Red / (double) byte.MaxValue);
        float num9 = (float) ((double) lightValue * (double) this._lightColor.Green / (double) byte.MaxValue);
        float num10 = (float) ((double) lightValue * (double) this._lightColor.Blue / (double) byte.MaxValue);
        num5 *= num8;
        num6 *= num9;
        num7 *= num10;
      }
      return (IColor) new Color((byte) ((uint) ((double) Libnoise.Clamp01(num5) * (double) byte.MaxValue) & (uint) byte.MaxValue), (byte) ((uint) ((double) Libnoise.Clamp01(num6) * (double) byte.MaxValue) & (uint) byte.MaxValue), (byte) ((uint) ((double) Libnoise.Clamp01(num7) * (double) byte.MaxValue) & (uint) byte.MaxValue), Math.Max(sourceColor.Alpha, backgroundColor.Alpha));
    }

    private float CalcLightIntensity(float center, float left, float right, float down, float up)
    {
      if (this._recalcLightValues)
      {
        this._cosAzimuth = (float) Math.Cos((double) this._lightAzimuth * (Math.PI / 180.0));
        this._sinAzimuth = (float) Math.Sin((double) this._lightAzimuth * (Math.PI / 180.0));
        this._cosElevation = (float) Math.Cos((double) this._lightElevation * (Math.PI / 180.0));
        this._sinElevation = (float) Math.Sin((double) this._lightElevation * (Math.PI / 180.0));
        this._recalcLightValues = false;
      }
      float num1 = (float) (1.41421353816986 * (double) this._sinElevation / 2.0);
      double num2 = (1.0 - (double) num1) * (double) this._lightContrast * 1.41421353816986 * (double) this._cosElevation * (double) this._cosAzimuth;
      float num3 = (float) ((1.0 - (double) num1) * (double) this._lightContrast * 1.41421353816986) * this._cosElevation * this._sinAzimuth;
      double num4 = (double) left - (double) right;
      float num5 = (float) (num2 * num4 + (double) num3 * ((double) down - (double) up)) + num1;
      if ((double) num5 < 0.0)
        num5 = 0.0f;
      return num5;
    }
  }
}
