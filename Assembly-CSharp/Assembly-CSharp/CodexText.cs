// Decompiled with JetBrains decompiler
// Type: CodexText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class CodexText : CodexWidget<CodexText>
{
  public string text { get; set; }

  public CodexTextStyle style { get; set; }

  public string stringKey
  {
    set => this.text = (string) Strings.Get(value);
    get => "--> " + (this.text ?? "NULL");
  }

  public CodexText() => this.style = CodexTextStyle.Body;

  public CodexText(string text, CodexTextStyle style = CodexTextStyle.Body)
  {
    this.text = text;
    this.style = style;
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

  public override void Configure(
    GameObject contentGameObject,
    Transform displayPane,
    Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
  {
    this.ConfigureLabel(contentGameObject.GetComponent<LocText>(), textStyles);
    this.ConfigurePreferredLayout(contentGameObject);
  }
}
