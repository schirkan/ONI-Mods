// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Modifier.Invert
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace LibNoiseDotNet.Graphics.Tools.Noise.Modifier
{
  public class Invert : ModifierModule, IModule3D, IModule
  {
    public Invert()
    {
    }

    public Invert(IModule source)
      : base(source)
    {
    }

    public float GetValue(float x, float y, float z) => -((IModule3D) this._sourceModule).GetValue(x, y, z);
  }
}
