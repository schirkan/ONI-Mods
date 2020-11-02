// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Renderer.Heightmap32Renderer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace LibNoiseDotNet.Graphics.Tools.Noise.Renderer
{
  public class Heightmap32Renderer : AbstractHeightmapRenderer
  {
    protected Heightmap32 _heightmap;

    public Heightmap32 Heightmap
    {
      get => this._heightmap;
      set => this._heightmap = value;
    }

    protected override void SetHeightmapSize(int width, int height) => this._heightmap.SetSize(width, height);

    protected override bool CheckHeightmap() => this._heightmap != null;

    protected override void RenderHeight(int x, int y, float source, float boundDiff)
    {
      float num = (double) source > (double) this._lowerHeightBound ? ((double) source < (double) this._upperHeightBound ? source : this._upperHeightBound) : this._lowerHeightBound;
      this._heightmap.SetValue(x, y, num);
    }
  }
}
