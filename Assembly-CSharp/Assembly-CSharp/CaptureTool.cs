﻿// Decompiled with JetBrains decompiler
// Type: CaptureTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class CaptureTool : DragTool
{
  protected override void OnDragComplete(Vector3 downPos, Vector3 upPos) => CaptureTool.MarkForCapture(this.GetRegularizedPos(Vector2.Min((Vector2) downPos, (Vector2) upPos), true), this.GetRegularizedPos(Vector2.Max((Vector2) downPos, (Vector2) upPos), false), true);

  public static void MarkForCapture(Vector2 min, Vector2 max, bool mark)
  {
    foreach (Capturable capturable in Components.Capturables.Items)
    {
      Vector2 xy = (Vector2) Grid.PosToXY(capturable.transform.GetPosition());
      if ((double) xy.x >= (double) min.x && (double) xy.x < (double) max.x && ((double) xy.y >= (double) min.y && (double) xy.y < (double) max.y))
      {
        if (capturable.allowCapture)
        {
          PrioritySetting selectedPriority = ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority();
          capturable.MarkForCapture(mark, selectedPriority);
        }
        else if (mark)
          PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, (string) UI.TOOLS.CAPTURE.NOT_CAPTURABLE, (Transform) null, capturable.transform.GetPosition());
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
