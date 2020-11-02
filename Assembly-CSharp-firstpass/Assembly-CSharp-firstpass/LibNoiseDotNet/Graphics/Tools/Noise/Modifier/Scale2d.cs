// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Modifier.Scale2d
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Modifier
{
  public class Scale2d : ModifierModule, IModule3D, IModule
  {
    public const float DEFAULT_SCALE = 1f;
    protected Vector2 _scale = Vector2.one * 1f;

    public Vector2 Scale
    {
      get => this._scale;
      set => this._scale = value;
    }

    public Scale2d()
    {
    }

    public Scale2d(IModule source)
      : base(source)
    {
    }

    public Scale2d(IModule source, Vector2 scale)
      : base(source)
      => this._scale = scale;

    public float GetValue(float x, float y, float z) => ((IModule3D) this._sourceModule).GetValue(x * this._scale.x, y, z * this._scale.y);
  }
}
