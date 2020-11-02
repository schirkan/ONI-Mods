// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Model.Cylinder
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Model
{
  public class Cylinder : AbstractModel
  {
    public Cylinder()
    {
    }

    public Cylinder(IModule3D module)
      : base((IModule) module)
    {
    }

    public float GetValue(float angle, float height) => ((IModule3D) this._sourceModule).GetValue((float) Math.Cos((double) angle * (Math.PI / 180.0)), height, (float) Math.Sin((double) angle * (Math.PI / 180.0)));
  }
}
