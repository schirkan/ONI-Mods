// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Renderer.Heightmap32
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using LibNoiseDotNet.Graphics.Tools.Noise.Builder;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Renderer
{
  public class Heightmap32 : NoiseMap
  {
    public Heightmap32()
    {
    }

    public Heightmap32(int width, int height)
      : base(width, height)
    {
    }

    public Heightmap32(Heightmap32 copy)
      : base((NoiseMap) copy)
    {
    }
  }
}
