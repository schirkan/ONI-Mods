// Decompiled with JetBrains decompiler
// Type: TutorialMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

public class TutorialMessage : GenericMessage
{
  [Serialize]
  public Tutorial.TutorialMessages messageId;
  public string videoClipId;
  public string videoOverlayName;
  public string videoTitleText;
  public string icon;

  public TutorialMessage()
  {
  }

  public TutorialMessage(
    Tutorial.TutorialMessages messageId,
    string title,
    string body,
    string tooltip,
    string videoClipId = null,
    string videoOverlayName = null,
    string videoTitleText = null,
    string icon = "")
    : base(title, body, tooltip)
  {
    this.messageId = messageId;
    this.videoClipId = videoClipId;
    this.videoOverlayName = videoOverlayName;
    this.videoTitleText = videoTitleText;
    this.icon = icon;
  }
}
