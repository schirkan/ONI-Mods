// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Renderer.Heightmap16Renderer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace LibNoiseDotNet.Graphics.Tools.Noise.Renderer
{
  public class Heightmap16Renderer : AbstractHeightmapRenderer
  {
    protected Heightmap16 _heightmap;

    public Heightmap16 Heightmap
    {
      get => this._heightmap;
      set => this._heightmap = value;
    }

    protected override void SetHeightmapSize(int width, int height) => this._heightmap.SetSize(width, height);

    protected override bool CheckHeightmap() => this._heightmap != null;

    protected override void RenderHeight(int x, int y, float source, float boundDiff)
    {
      ushort num = (double) source > (double) this._lowerHeightBound ? ((double) source < (double) this._upperHeightBound ? (ushort) (((double) source - (double) this._lowerHeightBound) / (double) boundDiff * (double) ushort.MaxValue) : ushort.MaxValue) : (ushort) 0;
      this._heightmap.SetValue(x, y, num);
    }
  }
}
