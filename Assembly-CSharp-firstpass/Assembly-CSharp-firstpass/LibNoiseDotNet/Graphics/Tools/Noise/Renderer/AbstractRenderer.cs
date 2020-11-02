// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Renderer.AbstractRenderer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace LibNoiseDotNet.Graphics.Tools.Noise.Renderer
{
  public abstract class AbstractRenderer
  {
    protected RendererCallback _callBack;
    protected IMap2D<float> _noiseMap;

    public IMap2D<float> NoiseMap
    {
      get => this._noiseMap;
      set => this._noiseMap = value;
    }

    public RendererCallback CallBack
    {
      get => this._callBack;
      set => this._callBack = value;
    }

    public abstract void Render();
  }
}
