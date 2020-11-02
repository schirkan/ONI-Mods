// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Renderer.Heightmap8
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using LibNoiseDotNet.Graphics.Tools.Noise.Utils;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Renderer
{
  public class Heightmap8 : DataMap<byte>, IMap2D<byte>
  {
    public Heightmap8()
    {
      this._borderValue = (byte) 0;
      this.AllocateBuffer();
    }

    public Heightmap8(int width, int height)
    {
      this._borderValue = (byte) 0;
      this.AllocateBuffer(width, height);
    }

    public Heightmap8(Heightmap8 copy)
    {
      this._borderValue = (byte) 0;
      this.CopyFrom((DataMap<byte>) copy);
    }

    public void MinMax(out byte min, out byte max)
    {
      min = max = (byte) 0;
      if (this._data == null || this._data.Length == 0)
        return;
      min = max = this._data[0];
      for (int index = 0; index < this._data.Length; ++index)
      {
        if ((int) min > (int) this._data[index])
          min = this._data[index];
        else if ((int) max < (int) this._data[index])
          max = this._data[index];
      }
    }

    protected override int SizeofT() => 8;

    protected override byte MaxvalofT() => byte.MaxValue;

    protected override byte MinvalofT() => 0;
  }
}
