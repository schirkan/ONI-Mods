// Decompiled with JetBrains decompiler
// Type: MoveToLocationToolHoverTextCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class MoveToLocationToolHoverTextCard : HoverTextConfiguration
{
  public override void UpdateHoverElements(List<KSelectable> selected)
  {
    int cell = Grid.PosToCell(Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()));
    if (!Grid.IsValidCell(cell))
      return;
    HoverTextDrawer drawer = HoverTextScreen.Instance.BeginDrawing();
    drawer.BeginShadowBar();
    this.DrawTitle(HoverTextScreen.Instance, drawer);
    this.DrawInstructions(HoverTextScreen.Instance, drawer);
    if (!MoveToLocationTool.Instance.CanMoveTo(cell))
    {
      drawer.NewLine();
      drawer.DrawText((string) UI.TOOLS.MOVETOLOCATION.UNREACHABLE, this.HoverTextStyleSettings[1]);
    }
    drawer.EndShadowBar();
    drawer.EndDrawing();
  }
}
