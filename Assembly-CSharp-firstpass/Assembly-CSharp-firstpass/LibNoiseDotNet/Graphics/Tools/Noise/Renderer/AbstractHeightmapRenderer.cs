// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Renderer.AbstractHeightmapRenderer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Renderer
{
  public abstract class AbstractHeightmapRenderer : AbstractRenderer
  {
    protected float _lowerHeightBound;
    protected float _upperHeightBound;
    protected bool _WrapEnabled;

    public float LowerHeightBound => this._lowerHeightBound;

    public float UpperHeightBound => this._upperHeightBound;

    public bool WrapEnabled
    {
      get => this._WrapEnabled;
      set => this._WrapEnabled = value;
    }

    public AbstractHeightmapRenderer() => this._WrapEnabled = false;

    public void SetBounds(float lowerBound, float upperBound)
    {
      this._lowerHeightBound = (double) lowerBound != (double) upperBound && (double) lowerBound <= (double) upperBound ? lowerBound : throw new ArgumentException("Incoherent bounds : lowerBound == upperBound or lowerBound > upperBound");
      this._upperHeightBound = upperBound;
    }

    public void ExactFit() => this._noiseMap.MinMax(out this._lowerHeightBound, out this._upperHeightBound);

    public override void Render()
    {
      if (this._noiseMap == null)
        throw new ArgumentException("A noise map must be provided");
      if (!this.CheckHeightmap())
        throw new ArgumentException("An heightmap must be provided");
      if (this._noiseMap.Width <= 0 || this._noiseMap.Height <= 0)
        throw new ArgumentException("Incoherent noise map size (0,0)");
      if ((double) this._lowerHeightBound == (double) this._upperHeightBound || (double) this._lowerHeightBound > (double) this._upperHeightBound)
        throw new ArgumentException("Incoherent bounds : lowerBound == upperBound or lowerBound > upperBound");
      int width = this._noiseMap.Width;
      int height = this._noiseMap.Height;
      int num1 = width - 1;
      int num2 = height - 1;
      int num3 = 0;
      int num4 = 0;
      this.SetHeightmapSize(width, height);
      float boundDiff = this._upperHeightBound - this._lowerHeightBound;
      for (int index = 0; index < height; ++index)
      {
        for (int x1 = 0; x1 < width; ++x1)
        {
          float num5 = this._noiseMap.GetValue(x1, index);
          if (this._WrapEnabled)
          {
            int x2 = x1 != num1 ? (x1 != num3 ? x1 : num1) : num3;
            int y = index != num2 ? (index != num4 ? index : num2) : num4;
            if (x2 != x1 || y != index)
            {
              float n1 = this._noiseMap.GetValue(x2, y);
              num5 = Libnoise.Lerp(num5, n1, 0.5f);
            }
          }
          this.RenderHeight(x1, index, num5, boundDiff);
        }
        if (this._callBack != null)
          this._callBack(index);
      }
    }

    protected abstract bool CheckHeightmap();

    protected abstract void SetHeightmapSize(int width, int height);

    protected abstract void RenderHeight(int x, int y, float source, float boundDiff);
  }
}
