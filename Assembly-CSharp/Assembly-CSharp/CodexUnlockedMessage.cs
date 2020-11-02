// Decompiled with JetBrains decompiler
// Type: CodexUnlockedMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

public class CodexUnlockedMessage : Message
{
  private string unlockMessage;
  private string lockId;

  public CodexUnlockedMessage()
  {
  }

  public CodexUnlockedMessage(string lock_id, string unlock_message)
  {
    this.lockId = lock_id;
    this.unlockMessage = unlock_message;
  }

  public string GetLockId() => this.lockId;

  public override string GetSound() => "AI_Notification_ResearchComplete";

  public override string GetMessageBody() => UI.CODEX.CODEX_DISCOVERED_MESSAGE.BODY.Replace("{codex}", this.unlockMessage);

  public override string GetTitle() => (string) UI.CODEX.CODEX_DISCOVERED_MESSAGE.TITLE;

  public override string GetTooltip() => this.GetMessageBody();

  public override bool IsValid() => true;
}
