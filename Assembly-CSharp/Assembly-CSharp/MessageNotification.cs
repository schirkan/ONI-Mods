// Decompiled with JetBrains decompiler
// Type: MessageNotification
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class MessageNotification : Notification
{
  public Message message;

  private string OnToolTip(List<Notification> notifications, string tooltipText) => tooltipText;

  public MessageNotification(Message m)
    : base(m.GetTitle(), NotificationType.Messages, HashedString.Invalid, expires: false)
  {
    MessageNotification messageNotification = this;
    this.message = m;
    if (!this.message.PlayNotificationSound())
      this.playSound = false;
    this.ToolTip = (Func<List<Notification>, object, string>) ((notifications, data) => messageNotification.OnToolTip(notifications, m.GetTooltip()));
    this.clickFocus = (Transform) null;
  }
}
