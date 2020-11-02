// Decompiled with JetBrains decompiler
// Type: BoundsCheck
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

internal static class BoundsCheck
{
  public static readonly int TOP = 1;
  public static readonly int BOTTOM = 2;
  public static readonly int LEFT = 4;
  public static readonly int RIGHT = 8;

  public static int Check(Vector2 point, Rect bounds)
  {
    int num = 0;
    if ((double) point.x == (double) bounds.xMin)
      num |= BoundsCheck.LEFT;
    if ((double) point.x == (double) bounds.xMax)
      num |= BoundsCheck.RIGHT;
    if ((double) point.y == (double) bounds.yMin)
      num |= BoundsCheck.TOP;
    if ((double) point.y == (double) bounds.yMax)
      num |= BoundsCheck.BOTTOM;
    return num;
  }
}
