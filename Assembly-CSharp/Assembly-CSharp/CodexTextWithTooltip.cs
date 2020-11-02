// Decompiled with JetBrains decompiler
// Type: CodexTextWithTooltip
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class CodexTextWithTooltip : CodexWidget<CodexTextWithTooltip>
{
  public string text { get; set; }

  public string tooltip { get; set; }

  public CodexTextStyle style { get; set; }

  public string stringKey
  {
    set => this.text = (string) Strings.Get(value);
    get => "--> " + (this.text ?? "NULL");
  }

  public CodexTextWithTooltip() => this.style = CodexTextStyle.Body;

  public CodexTextWithTooltip(string text, string tooltip, CodexTextStyle style = CodexTextStyle.Body)
  {
    this.text = text;
    this.style = style;
    this.tooltip = tooltip;
  }

  public void ConfigureLabel(
    LocText label,
    Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
  {
    label.gameObject.SetActive(true);
    label.AllowLinks = this.style == CodexTextStyle.Body;
    label.textStyleSetting = textStyles[this.style];
    label.text = this.text;
    label.ApplySettings();
  }

  public void ConfigureTooltip(ToolTip tooltip) => tooltip.SetSimpleTooltip(this.tooltip);

  public override void Configure(
    GameObject contentGameObject,
    Transform displayPane,
    Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
  {
    this.ConfigureLabel(contentGameObject.GetComponent<LocText>(), textStyles);
    this.ConfigureTooltip(contentGameObject.GetComponent<ToolTip>());
    this.ConfigurePreferredLayout(contentGameObject);
  }
}
