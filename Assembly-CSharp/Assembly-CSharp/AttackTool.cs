﻿// Decompiled with JetBrains decompiler
// Type: AttackTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class AttackTool : DragTool
{
  protected override void OnDragComplete(Vector3 downPos, Vector3 upPos) => AttackTool.MarkForAttack(this.GetRegularizedPos(Vector2.Min((Vector2) downPos, (Vector2) upPos), true), this.GetRegularizedPos(Vector2.Max((Vector2) downPos, (Vector2) upPos), false), true);

  public static void MarkForAttack(Vector2 min, Vector2 max, bool mark)
  {
    foreach (FactionAlignment factionAlignment in Components.FactionAlignments.Items)
    {
      Vector2 xy = (Vector2) Grid.PosToXY(factionAlignment.transform.GetPosition());
      if ((double) xy.x >= (double) min.x && (double) xy.x < (double) max.x && ((double) xy.y >= (double) min.y && (double) xy.y < (double) max.y))
      {
        if (mark)
        {
          if (FactionManager.Instance.GetDisposition(FactionManager.FactionID.Duplicant, factionAlignment.Alignment) != FactionManager.Disposition.Assist)
            factionAlignment.SetPlayerTargeted(true);
        }
        else
          factionAlignment.gameObject.Trigger(2127324410);
      }
    }
  }

  protected override void OnActivateTool()
  {
    base.OnActivateTool();
    ToolMenu.Instance.PriorityScreen.Show();
  }

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    base.OnDeactivateTool(new_tool);
    ToolMenu.Instance.PriorityScreen.Show(false);
  }
}
