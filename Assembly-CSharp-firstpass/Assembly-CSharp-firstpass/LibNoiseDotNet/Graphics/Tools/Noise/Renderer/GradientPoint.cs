// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Renderer.GradientPoint
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Renderer
{
  public struct GradientPoint : IEquatable<GradientPoint>
  {
    public IColor Color;
    public float Position;
    private int _hashcode;

    public GradientPoint(float position, IColor color)
    {
      this.Color = color;
      this.Position = position;
      this._hashcode = (int) this.Position ^ this.Color.GetHashCode();
    }

    public bool Equals(GradientPoint other) => (double) this.Position == (double) other.Position;

    public override int GetHashCode() => this._hashcode;
  }
}
