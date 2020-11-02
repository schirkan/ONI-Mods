// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Renderer.NormalMapRenderer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Renderer
{
  public class NormalMapRenderer : AbstractImageRenderer
  {
    protected bool _WrapEnabled;
    protected float _bumpHeight;

    public bool WrapEnabled
    {
      get => this._WrapEnabled;
      set => this._WrapEnabled = value;
    }

    public float BumpHeight
    {
      get => this._bumpHeight;
      set => this._bumpHeight = value;
    }

    public NormalMapRenderer()
    {
      this._WrapEnabled = false;
      this._bumpHeight = 1f;
    }

    public override void Render()
    {
      if (this._noiseMap == null)
        throw new ArgumentException("A noise map must be provided");
      if (this._image == null)
        throw new ArgumentException("An image map must be provided");
      if (this._noiseMap.Width <= 0 || this._noiseMap.Height <= 0)
        throw new ArgumentException("Incoherent noise map size (0,0)");
      int width = this._noiseMap.Width;
      int height = this._noiseMap.Height;
      int num1 = width - 1;
      int num2 = height - 1;
      int num3 = -num1;
      int num4 = -num2;
      this._image.SetSize(width, height);
      for (int index = 0; index < height; ++index)
      {
        for (int x = 0; x < width; ++x)
        {
          int num5;
          int num6;
          if (this._WrapEnabled)
          {
            num5 = x != num1 ? 1 : num3;
            num6 = index != num2 ? 1 : num4;
          }
          else
          {
            num5 = x != num1 ? 1 : 0;
            num6 = index != num2 ? 1 : 0;
          }
          float nc = this._noiseMap.GetValue(x, index);
          float nr = this._noiseMap.GetValue(x + num5, index);
          float nu = this._noiseMap.GetValue(x, index + num6);
          this._image.SetValue(x, index, this.CalcNormalColor(nc, nr, nu, this._bumpHeight));
        }
        if (this._callBack != null)
          this._callBack(index);
      }
    }

    private IColor CalcNormalColor(float nc, float nr, float nu, float bumpHeight)
    {
      nc *= bumpHeight;
      nr *= bumpHeight;
      nu *= bumpHeight;
      float num1 = nc - nr;
      double num2 = (double) nc - (double) nu;
      float num3 = (float) Math.Sqrt(num2 * num2 + (double) num1 * (double) num1 + 1.0);
      double num4 = ((double) nc - (double) nr) / (double) num3;
      float num5 = (nc - nu) / num3;
      float num6 = 1f / num3;
      int num7 = (int) (byte) (Libnoise.FastFloor((float) ((num4 + 1.0) * 127.5)) & (int) byte.MaxValue);
      byte num8 = (byte) (Libnoise.FastFloor((float) (((double) num5 + 1.0) * 127.5)) & (int) byte.MaxValue);
      byte num9 = (byte) (Libnoise.FastFloor((float) (((double) num6 + 1.0) * 127.5)) & (int) byte.MaxValue);
      int num10 = (int) num8;
      int num11 = (int) num9;
      return (IColor) new Color((byte) num7, (byte) num10, (byte) num11, byte.MaxValue);
    }
  }
}
