// Decompiled with JetBrains decompiler
// Type: CodexVideo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class CodexVideo : CodexWidget<CodexVideo>
{
  public string name { get; set; }

  public string videoName
  {
    set => this.name = value;
    get => "--> " + (this.name ?? "NULL");
  }

  public string overlayName { get; set; }

  public List<string> overlayTexts { get; set; }

  public void ConfigureVideo(
    VideoWidget videoWidget,
    string clipName,
    string overlayName = null,
    List<string> overlayTexts = null)
  {
    videoWidget.SetClip(Assets.GetVideo(clipName), overlayName, overlayTexts);
  }

  public override void Configure(
    GameObject contentGameObject,
    Transform displayPane,
    Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
  {
    this.ConfigureVideo(contentGameObject.GetComponent<VideoWidget>(), this.name, this.overlayName, this.overlayTexts);
    this.ConfigurePreferredLayout(contentGameObject);
  }
}
