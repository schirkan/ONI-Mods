// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.IMap2D`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace LibNoiseDotNet.Graphics.Tools.Noise
{
  public interface IMap2D<T>
  {
    int Width { get; }

    int Height { get; }

    T BorderValue { get; set; }

    T GetValue(int x, int y);

    void SetValue(int x, int y, T value);

    void SetSize(int width, int height);

    void Reset();

    void Clear(T value);

    void Clear();

    void MinMax(out T min, out T max);
  }
}
