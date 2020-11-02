// Decompiled with JetBrains decompiler
// Type: CodexWidget`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class CodexWidget<SubClass> : ICodexWidget
{
  public int preferredWidth { get; set; }

  public int preferredHeight { get; set; }

  protected CodexWidget()
  {
    this.preferredWidth = -1;
    this.preferredHeight = -1;
  }

  protected CodexWidget(int preferredWidth, int preferredHeight)
  {
    this.preferredWidth = preferredWidth;
    this.preferredHeight = preferredHeight;
  }

  public abstract void Configure(
    GameObject contentGameObject,
    Transform displayPane,
    Dictionary<CodexTextStyle, TextStyleSetting> textStyles);

  protected void ConfigurePreferredLayout(GameObject contentGameObject)
  {
    LayoutElement componentInChildren = contentGameObject.GetComponentInChildren<LayoutElement>();
    componentInChildren.preferredHeight = (float) this.preferredHeight;
    componentInChildren.preferredWidth = (float) this.preferredWidth;
  }
}
