﻿// Decompiled with JetBrains decompiler
// Type: CancelToolHoverTextCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;

public class CancelToolHoverTextCard : HoverTextConfiguration
{
  private string lastUpdatedFilter;

  public override void UpdateHoverElements(List<KSelectable> selected)
  {
    string lastEnabledFilter = ToolMenu.Instance.toolParameterMenu.GetLastEnabledFilter();
    HoverTextScreen instance = HoverTextScreen.Instance;
    HoverTextDrawer drawer = instance.BeginDrawing();
    drawer.BeginShadowBar();
    this.DrawTitle(instance, drawer);
    this.DrawInstructions(HoverTextScreen.Instance, drawer);
    if (lastEnabledFilter != null && lastEnabledFilter != this.lastUpdatedFilter)
      this.ConfigureTitle(instance);
    drawer.EndShadowBar();
    drawer.EndDrawing();
  }

  protected override void ConfigureTitle(HoverTextScreen screen)
  {
    string lastEnabledFilter = ToolMenu.Instance.toolParameterMenu.GetLastEnabledFilter();
    if (string.IsNullOrEmpty(this.ToolName) || lastEnabledFilter == "ALL")
      this.ToolName = Strings.Get(this.ToolNameStringKey).String.ToUpper();
    if (lastEnabledFilter != null && lastEnabledFilter != "ALL")
      this.ToolName = Strings.Get(this.ToolNameStringKey).String.ToUpper() + string.Format((string) UI.TOOLS.FILTER_HOVERCARD_HEADER, (object) Strings.Get("STRINGS.UI.TOOLS.FILTERLAYERS." + lastEnabledFilter).String.ToUpper());
    this.lastUpdatedFilter = lastEnabledFilter;
  }
}
