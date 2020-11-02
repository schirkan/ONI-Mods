// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Model.Sphere
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace LibNoiseDotNet.Graphics.Tools.Noise.Model
{
  public class Sphere : AbstractModel
  {
    public Sphere()
    {
    }

    public Sphere(IModule3D module)
      : base((IModule) module)
    {
    }

    public float GetValue(float lat, float lon)
    {
      float x = 0.0f;
      float y = 0.0f;
      float z = 0.0f;
      Libnoise.LatLonToXYZ(lat, lon, ref x, ref y, ref z);
      return ((IModule3D) this._sourceModule).GetValue(x, y, z);
    }
  }
}
