// Decompiled with JetBrains decompiler
// Type: Geometry.HorizontalEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Geometry
{
  public struct HorizontalEvent
  {
    public float x;
    public Strip strip;
    public bool isStart;

    public HorizontalEvent(float x, Strip strip, bool isStart)
    {
      this.x = x;
      this.strip = strip;
      this.isStart = isStart;
    }
  }
}
