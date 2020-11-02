﻿// Decompiled with JetBrains decompiler
// Type: TutorialMessageDialog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TutorialMessageDialog : MessageDialog
{
  [SerializeField]
  private LocText description;
  private TutorialMessage message;
  [SerializeField]
  private GameObject videoWidgetPrefab;
  private VideoWidget videoWidget;

  public override bool CanDontShowAgain => true;

  public override bool CanDisplay(Message message) => typeof (TutorialMessage).IsAssignableFrom(message.GetType());

  public override void SetMessage(Message base_message)
  {
    this.message = base_message as TutorialMessage;
    this.description.text = this.message.GetMessageBody();
    if (string.IsNullOrEmpty(this.message.videoClipId))
      return;
    this.SetVideo(Assets.GetVideo(this.message.videoClipId), this.message.videoOverlayName, this.message.videoTitleText);
  }

  public void SetVideo(VideoClip clip, string overlayName, string titleText)
  {
    if ((Object) this.videoWidget == (Object) null)
    {
      this.videoWidget = Util.KInstantiateUI(this.videoWidgetPrefab, this.transform.gameObject, true).GetComponent<VideoWidget>();
      this.videoWidget.transform.SetAsFirstSibling();
    }
    this.videoWidget.SetClip(clip, overlayName, new List<string>()
    {
      titleText,
      (string) VIDEOS.TUTORIAL_HEADER
    });
  }

  public override void OnClickAction()
  {
  }

  public override void OnDontShowAgain() => Tutorial.Instance.HideTutorialMessage(this.message.messageId);
}
