// Decompiled with JetBrains decompiler
// Type: Geometry.Strip
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Geometry
{
  public class Strip
  {
    public float yMin;
    public float yMax;
    public bool subtract;

    public Strip(float yMin, float yMax, bool subtract)
    {
      this.yMin = yMin;
      this.yMax = yMax;
      this.subtract = subtract;
    }
  }
}
