// Decompiled with JetBrains decompiler
// Type: PrebuildToolHoverTextCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;

public class PrebuildToolHoverTextCard : HoverTextConfiguration
{
  public PlanScreen.RequirementsState currentReqState;
  public BuildingDef currentDef;

  public override void UpdateHoverElements(List<KSelectable> selected)
  {
    HoverTextScreen instance = HoverTextScreen.Instance;
    HoverTextDrawer hoverTextDrawer = instance.BeginDrawing();
    hoverTextDrawer.BeginShadowBar();
    switch (this.currentReqState)
    {
      case PlanScreen.RequirementsState.Tech:
        Tech parentTech = Db.Get().TechItems.Get(this.currentDef.PrefabID).parentTech;
        hoverTextDrawer.DrawText(string.Format((string) UI.PRODUCTINFO_RESEARCHREQUIRED, (object) parentTech.Name).ToUpper(), this.HoverTextStyleSettings[0]);
        break;
      case PlanScreen.RequirementsState.Materials:
      case PlanScreen.RequirementsState.Complete:
        hoverTextDrawer.DrawText(UI.TOOLTIPS.NOMATERIAL.text.ToUpper(), this.HoverTextStyleSettings[0]);
        hoverTextDrawer.NewLine();
        hoverTextDrawer.DrawText((string) UI.TOOLTIPS.SELECTAMATERIAL, this.HoverTextStyleSettings[1]);
        break;
    }
    hoverTextDrawer.NewLine();
    hoverTextDrawer.DrawIcon(instance.GetSprite("icon_mouse_right"));
    hoverTextDrawer.DrawText(this.backStr, this.Styles_Instruction.Standard);
    hoverTextDrawer.EndShadowBar();
    hoverTextDrawer.EndDrawing();
  }
}
