// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Combiner.Substract
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace LibNoiseDotNet.Graphics.Tools.Noise.Combiner
{
  public class Substract : CombinerModule, IModule3D, IModule
  {
    public Substract()
    {
    }

    public Substract(IModule left, IModule right)
      : base(left, right)
    {
    }

    public float GetValue(float x, float y, float z) => ((IModule3D) this._leftModule).GetValue(x, y, z) - ((IModule3D) this._rightModule).GetValue(x, y, z);
  }
}
