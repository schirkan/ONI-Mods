// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Builder.NoiseMap
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using LibNoiseDotNet.Graphics.Tools.Noise.Utils;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Builder
{
  public class NoiseMap : DataMap<float>, IMap2D<float>
  {
    public NoiseMap()
    {
      this._hasMaxDimension = false;
      this._borderValue = 0.0f;
      this.AllocateBuffer();
    }

    public NoiseMap(int width, int height)
    {
      this._hasMaxDimension = false;
      this._borderValue = 0.0f;
      this.AllocateBuffer(width, height);
    }

    public NoiseMap(NoiseMap copy)
    {
      this._hasMaxDimension = false;
      this._borderValue = 0.0f;
      this.CopyFrom((DataMap<float>) copy);
    }

    public void MinMax(out float min, out float max)
    {
      min = max = 0.0f;
      if (this._data == null || this._data.Length == 0)
        return;
      min = max = this._data[0];
      for (int index = 1; index < this._data.Length; ++index)
      {
        if ((double) min > (double) this._data[index])
          min = this._data[index];
        else if ((double) max < (double) this._data[index])
          max = this._data[index];
      }
    }

    protected override int SizeofT() => 32;

    protected override float MaxvalofT() => float.MaxValue;

    protected override float MinvalofT() => float.MinValue;
  }
}
