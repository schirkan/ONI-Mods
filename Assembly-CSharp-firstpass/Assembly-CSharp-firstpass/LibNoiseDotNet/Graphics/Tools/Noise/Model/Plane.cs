// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Model.Plane
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace LibNoiseDotNet.Graphics.Tools.Noise.Model
{
  public class Plane : AbstractModel
  {
    public Plane()
    {
    }

    public Plane(IModule module)
      : base(module)
    {
    }

    public float GetValue(float x, float z) => ((IModule3D) this._sourceModule).GetValue(x, 0.0f, z);
  }
}
