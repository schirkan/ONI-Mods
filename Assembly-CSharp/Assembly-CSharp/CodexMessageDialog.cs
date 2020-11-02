// Decompiled with JetBrains decompiler
// Type: CodexMessageDialog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class CodexMessageDialog : MessageDialog
{
  [SerializeField]
  private LocText description;
  private CodexUnlockedMessage message;

  public override bool CanDisplay(Message message) => typeof (CodexUnlockedMessage).IsAssignableFrom(message.GetType());

  public override void SetMessage(Message base_message)
  {
    this.message = (CodexUnlockedMessage) base_message;
    this.description.text = this.message.GetMessageBody();
  }

  public override void OnClickAction()
  {
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    this.message.OnCleanUp();
  }
}
