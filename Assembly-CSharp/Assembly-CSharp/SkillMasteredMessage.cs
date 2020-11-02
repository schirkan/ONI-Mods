// Decompiled with JetBrains decompiler
// Type: SkillMasteredMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;

public class SkillMasteredMessage : Message
{
  [Serialize]
  private string minionName;

  public SkillMasteredMessage()
  {
  }

  public SkillMasteredMessage(MinionResume resume) => this.minionName = resume.GetProperName();

  public override string GetSound() => "AI_Notification_ResearchComplete";

  public override string GetMessageBody()
  {
    Debug.Assert(this.minionName != null);
    string str = string.Format((string) MISC.NOTIFICATIONS.SKILL_POINT_EARNED.LINE, (object) this.minionName);
    return string.Format((string) MISC.NOTIFICATIONS.SKILL_POINT_EARNED.MESSAGEBODY, (object) str);
  }

  public override string GetTitle() => (string) MISC.NOTIFICATIONS.SKILL_POINT_EARNED.NAME;

  public override string GetTooltip() => string.Format((string) MISC.NOTIFICATIONS.SKILL_POINT_EARNED.TOOLTIP, (object) "");

  public override bool IsValid() => this.minionName != null;
}
