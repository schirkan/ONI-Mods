// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Renderer.Heightmap16
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using LibNoiseDotNet.Graphics.Tools.Noise.Utils;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Renderer
{
  public class Heightmap16 : DataMap<ushort>, IMap2D<ushort>
  {
    public Heightmap16()
    {
      this._borderValue = (ushort) 0;
      this.AllocateBuffer();
    }

    public Heightmap16(int width, int height)
    {
      this._borderValue = (ushort) 0;
      this.AllocateBuffer(width, height);
    }

    public Heightmap16(Heightmap16 copy)
    {
      this._borderValue = (ushort) 0;
      this.CopyFrom((DataMap<ushort>) copy);
    }

    public void MinMax(out ushort min, out ushort max)
    {
      min = max = (ushort) 0;
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

    protected override int SizeofT() => 16;

    protected override ushort MaxvalofT() => ushort.MaxValue;

    protected override ushort MinvalofT() => 0;
  }
}
