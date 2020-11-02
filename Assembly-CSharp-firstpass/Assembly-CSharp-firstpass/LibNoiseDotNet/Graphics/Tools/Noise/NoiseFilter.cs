// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.NoiseFilter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace LibNoiseDotNet.Graphics.Tools.Noise
{
  public enum NoiseFilter : byte
  {
    Pipe = 0,
    SumFractal = 1,
    SinFractal = 2,
    Billow = 19, // 0x13
    MultiFractal = 20, // 0x14
    HeterogeneousMultiFractal = 21, // 0x15
    HybridMultiFractal = 22, // 0x16
    RidgedMultiFractal = 23, // 0x17
    Voronoi = 30, // 0x1E
  }
}
