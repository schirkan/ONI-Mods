// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Modifier.ControlPoint
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Modifier
{
  public struct ControlPoint : IEquatable<ControlPoint>
  {
    public float Input;
    public float Output;

    public ControlPoint(float input, float output)
    {
      this.Input = input;
      this.Output = output;
    }

    public bool Equals(ControlPoint other) => (double) this.Input == (double) other.Input;
  }
}
