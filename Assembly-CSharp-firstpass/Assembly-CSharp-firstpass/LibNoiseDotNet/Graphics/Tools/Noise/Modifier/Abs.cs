// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Modifier.Abs
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Modifier
{
  public class Abs : ModifierModule, IModule3D, IModule
  {
    public Abs()
    {
    }

    public Abs(IModule source)
      : base(source)
    {
    }

    public float GetValue(float x, float y, float z) => Math.Abs(((IModule3D) this._sourceModule).GetValue(x, y, z));
  }
}
