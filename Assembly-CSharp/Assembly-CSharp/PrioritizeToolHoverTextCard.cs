﻿// Decompiled with JetBrains decompiler
// Type: PrioritizeToolHoverTextCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class PrioritizeToolHoverTextCard : HoverTextConfiguration
{
  public override void UpdateHoverElements(List<KSelectable> selected)
  {
    if ((Object) ToolMenu.Instance.PriorityScreen == (Object) null)
      return;
    HoverTextScreen instance = HoverTextScreen.Instance;
    HoverTextDrawer drawer = instance.BeginDrawing();
    drawer.BeginShadowBar();
    this.DrawTitle(instance, drawer);
    this.DrawInstructions(HoverTextScreen.Instance, drawer);
    drawer.NewLine();
    drawer.DrawText(string.Format((string) UI.TOOLS.PRIORITIZE.SPECIFIC_PRIORITY, (object) ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority().priority_value.ToString()), this.Styles_Title.Standard);
    string lastEnabledFilter = ToolMenu.Instance.toolParameterMenu.GetLastEnabledFilter();
    if (lastEnabledFilter != null && lastEnabledFilter != "ALL")
      this.ConfigureTitle(instance);
    drawer.EndShadowBar();
    drawer.EndDrawing();
  }

  protected override void ConfigureTitle(HoverTextScreen screen)
  {
    string lastEnabledFilter = ToolMenu.Instance.toolParameterMenu.GetLastEnabledFilter();
    if (string.IsNullOrEmpty(this.ToolName) || lastEnabledFilter == "ALL")
      this.ToolName = Strings.Get(this.ToolNameStringKey).String.ToUpper();
    if (lastEnabledFilter == null || !(lastEnabledFilter != "ALL"))
      return;
    this.ToolName = Strings.Get(this.ToolNameStringKey).String.ToUpper() + string.Format((string) UI.TOOLS.FILTER_HOVERCARD_HEADER, (object) Strings.Get("STRINGS.UI.TOOLS.FILTERLAYERS." + lastEnabledFilter).String.ToUpper());
  }
}
