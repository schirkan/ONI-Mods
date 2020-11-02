// Decompiled with JetBrains decompiler
// Type: Geometry.VerticalEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Geometry
{
  public struct VerticalEvent
  {
    public float y;
    public bool isStart;
    public bool subtract;

    public VerticalEvent(float y, bool isStart, bool subtract)
    {
      this.y = y;
      this.isStart = isStart;
      this.subtract = subtract;
    }
  }
}
