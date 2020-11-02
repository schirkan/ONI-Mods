// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Renderer.Image
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using LibNoiseDotNet.Graphics.Tools.Noise.Utils;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Renderer
{
  public class Image : DataMap<Color>, IMap2D<Color>
  {
    public const int RASTER_MAX_WIDTH = 32767;
    public const int RASTER_MAX_HEIGHT = 32767;

    public Image()
    {
      this._hasMaxDimension = true;
      this._maxHeight = (int) short.MaxValue;
      this._maxWidth = (int) short.MaxValue;
      this._borderValue = Color.TRANSPARENT;
      this.AllocateBuffer();
    }

    public Image(int width, int height)
    {
      this._hasMaxDimension = true;
      this._maxHeight = (int) short.MaxValue;
      this._maxWidth = (int) short.MaxValue;
      this._borderValue = Color.WHITE;
      this.AllocateBuffer(width, height);
    }

    public Image(Image copy)
    {
      this._hasMaxDimension = true;
      this._maxHeight = (int) short.MaxValue;
      this._maxWidth = (int) short.MaxValue;
      this._borderValue = Color.WHITE;
      this.CopyFrom((DataMap<Color>) copy);
    }

    public void MinMax(out Color min, out Color max)
    {
      min = max = this.MinvalofT();
      if (this._data == null || this._data.Length == 0)
        return;
      min = max = this._data[0];
      for (int index = 0; index < this._data.Length; ++index)
      {
        if (min > (IColor) this._data[index])
          min = this._data[index];
        else if (max < (IColor) this._data[index])
          max = this._data[index];
      }
    }

    protected override int SizeofT() => 64;

    protected override Color MaxvalofT() => Color.WHITE;

    protected override Color MinvalofT() => Color.BLACK;
  }
}
