// Decompiled with JetBrains decompiler
// Type: Delaunay.Geo.Circle
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

namespace Delaunay.Geo
{
  public sealed class Circle
  {
    public Vector2 center;
    public float radius;

    public Circle(float centerX, float centerY, float radius)
    {
      this.center = new Vector2(centerX, centerY);
      this.radius = radius;
    }

    public override string ToString() => "Circle (center: " + this.center.ToString() + "; radius: " + this.radius.ToString() + ")";
  }
}
