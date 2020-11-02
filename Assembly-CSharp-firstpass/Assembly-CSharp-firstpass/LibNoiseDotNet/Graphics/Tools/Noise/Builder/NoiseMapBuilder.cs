// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Builder.NoiseMapBuilder
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Builder
{
  public abstract class NoiseMapBuilder
  {
    protected IModule _sourceModule;
    protected IMap2D<float> _noiseMap;
    protected NoiseMapBuilderCallback _callBack;
    protected int _width;
    protected int _height;
    protected IBuilderFilter _filter;

    public IModule SourceModule
    {
      get => this._sourceModule;
      set => this._sourceModule = value;
    }

    public IMap2D<float> NoiseMap
    {
      get => this._noiseMap;
      set => this._noiseMap = value;
    }

    public NoiseMapBuilderCallback CallBack
    {
      get => this._callBack;
      set => this._callBack = value;
    }

    public int Width => this._width;

    public int Height => this._height;

    public IBuilderFilter Filter
    {
      get => this._filter;
      set => this._filter = value;
    }

    public abstract void Build();

    public void SetSize(int width, int height)
    {
      this._height = width >= 0 && height >= 0 ? height : throw new ArgumentException("Dimension must be greater or equal 0");
      this._width = width;
    }
  }
}
