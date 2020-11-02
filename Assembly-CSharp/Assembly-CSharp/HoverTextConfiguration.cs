// Decompiled with JetBrains decompiler
// Type: HoverTextConfiguration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/HoverTextConfiguration")]
public class HoverTextConfiguration : KMonoBehaviour
{
  public TextStyleSetting[] HoverTextStyleSettings;
  public string ToolNameStringKey = "";
  public string ActionStringKey = "";
  [HideInInspector]
  public string ActionName = "";
  [HideInInspector]
  public string ToolName;
  protected string backStr;
  public TextStyleSetting ToolTitleTextStyle;
  public HoverTextConfiguration.TextStylePair Styles_Title;
  public HoverTextConfiguration.TextStylePair Styles_BodyText;
  public HoverTextConfiguration.TextStylePair Styles_Instruction;
  public HoverTextConfiguration.ValuePropertyTextStyles Styles_Values;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.ConfigureHoverScreen();
  }

  protected virtual void ConfigureTitle(HoverTextScreen screen)
  {
    if (!string.IsNullOrEmpty(this.ToolName))
      return;
    this.ToolName = Strings.Get(this.ToolNameStringKey).String.ToUpper();
  }

  protected void DrawTitle(HoverTextScreen screen, HoverTextDrawer drawer) => drawer.DrawText(this.ToolName, this.ToolTitleTextStyle);

  protected void DrawInstructions(HoverTextScreen screen, HoverTextDrawer drawer)
  {
    TextStyleSetting standard = this.Styles_Instruction.Standard;
    drawer.NewLine();
    drawer.DrawIcon(screen.GetSprite("icon_mouse_left"), 20);
    drawer.DrawText(this.ActionName, standard);
    drawer.AddIndent(8);
    drawer.DrawIcon(screen.GetSprite("icon_mouse_right"), 20);
    drawer.DrawText(this.backStr, standard);
  }

  public virtual void ConfigureHoverScreen()
  {
    if (!string.IsNullOrEmpty(this.ActionStringKey))
      this.ActionName = (string) Strings.Get(this.ActionStringKey);
    this.ConfigureTitle(HoverTextScreen.Instance);
    this.backStr = UI.TOOLS.GENERIC.BACK.ToString().ToUpper();
  }

  public virtual void UpdateHoverElements(List<KSelectable> hover_objects)
  {
    HoverTextScreen instance = HoverTextScreen.Instance;
    HoverTextDrawer drawer = instance.BeginDrawing();
    drawer.BeginShadowBar();
    this.DrawTitle(instance, drawer);
    this.DrawInstructions(HoverTextScreen.Instance, drawer);
    drawer.EndShadowBar();
    drawer.EndDrawing();
  }

  [Serializable]
  public struct TextStylePair
  {
    public TextStyleSetting Standard;
    public TextStyleSetting Selected;
  }

  [Serializable]
  public struct ValuePropertyTextStyles
  {
    public HoverTextConfiguration.TextStylePair Property;
    public HoverTextConfiguration.TextStylePair Property_Decimal;
    public HoverTextConfiguration.TextStylePair Property_Unit;
  }
}
