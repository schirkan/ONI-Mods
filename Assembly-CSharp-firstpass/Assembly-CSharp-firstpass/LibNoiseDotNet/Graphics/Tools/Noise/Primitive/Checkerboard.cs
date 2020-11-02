// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Primitive.Checkerboard
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace LibNoiseDotNet.Graphics.Tools.Noise.Primitive
{
  public class Checkerboard : PrimitiveModule, IModule3D, IModule
  {
    public float GetValue(float x, float y, float z) => (((double) x > 0.0 ? (int) x : (int) x - 1) & 1 ^ ((double) y > 0.0 ? (int) y : (int) y - 1) & 1 ^ ((double) z > 0.0 ? (int) z : (int) z - 1) & 1) == 0 ? 1f : -1f;
  }
}
