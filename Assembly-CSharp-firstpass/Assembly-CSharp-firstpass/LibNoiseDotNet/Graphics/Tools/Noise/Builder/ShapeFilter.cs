// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Builder.ShapeFilter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using LibNoiseDotNet.Graphics.Tools.Noise.Renderer;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Builder
{
  public class ShapeFilter : IBuilderFilter
  {
    public const float DEFAULT_VALUE = -0.5f;
    protected float _constant = -0.5f;
    protected IMap2D<IColor> _shape;
    protected ShapeFilter.LevelCache _cache = new ShapeFilter.LevelCache(-1, -1, (byte) 0);

    public IMap2D<IColor> Shape
    {
      get => this._shape;
      set => this._shape = value;
    }

    public float ConstantValue
    {
      get => this._constant;
      set => this._constant = value;
    }

    public FilterLevel IsFiltered(int x, int y)
    {
      switch (this.GetGreyscaleLevel(x, y))
      {
        case 0:
          return FilterLevel.Constant;
        case byte.MaxValue:
          return FilterLevel.Source;
        default:
          return FilterLevel.Filter;
      }
    }

    public float FilterValue(int x, int y, float source)
    {
      byte greyscaleLevel = this.GetGreyscaleLevel(x, y);
      switch (greyscaleLevel)
      {
        case 0:
          return this._constant;
        case byte.MaxValue:
          return source;
        default:
          return Libnoise.Lerp(this._constant, source, (float) greyscaleLevel / (float) byte.MaxValue);
      }
    }

    protected byte GetGreyscaleLevel(int x, int y)
    {
      if (!this._cache.IsCached(x, y))
        this._cache.Update(x, y, this._shape.GetValue(x, y).Red);
      return this._cache.level;
    }

    protected struct LevelCache
    {
      private int x;
      private int y;
      public byte level;

      public LevelCache(int x, int y, byte level)
      {
        this.x = x;
        this.y = y;
        this.level = level;
      }

      public bool IsCached(int x, int y) => this.x == x && this.y == y;

      public void Update(int x, int y, byte level)
      {
        this.x = x;
        this.y = y;
        this.level = level;
      }
    }
  }
}
