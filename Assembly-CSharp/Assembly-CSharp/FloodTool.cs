// Decompiled with JetBrains decompiler
// Type: FloodTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class FloodTool : InterfaceTool
{
  public Func<int, bool> floodCriteria;
  public System.Action<HashSet<int>> paintArea;
  protected Color32 areaColour = (Color32) new Color(0.5f, 0.7f, 0.5f, 0.2f);
  protected int mouseCell = -1;

  public HashSet<int> Flood(int startCell)
  {
    HashSet<int> intSet1 = new HashSet<int>();
    HashSet<int> intSet2 = new HashSet<int>();
    GameUtil.FloodFillConditional(startCell, this.floodCriteria, (ICollection<int>) intSet1, (ICollection<int>) intSet2);
    return intSet2;
  }

  public override void OnLeftClickDown(Vector3 cursor_pos)
  {
    base.OnLeftClickDown(cursor_pos);
    this.paintArea(this.Flood(Grid.PosToCell(cursor_pos)));
  }

  public override void OnMouseMove(Vector3 cursor_pos)
  {
    base.OnMouseMove(cursor_pos);
    this.mouseCell = Grid.PosToCell(cursor_pos);
  }
}
