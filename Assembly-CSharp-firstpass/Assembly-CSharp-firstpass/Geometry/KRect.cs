// Decompiled with JetBrains decompiler
// Type: Geometry.KRect
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

namespace Geometry
{
  public struct KRect
  {
    public Vector2 min;
    public Vector2 max;

    public KRect(Vector2 min, Vector2 max)
    {
      this.min = min;
      this.max = max;
    }

    public KRect(float x0, float y0, float x1, float y1)
    {
      this.min = new Vector2(x0, y0);
      this.max = new Vector2(x1, y1);
    }
  }
}
