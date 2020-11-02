// Decompiled with JetBrains decompiler
// Type: AchievementEarnedMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

public class AchievementEarnedMessage : Message
{
  public override bool ShowDialog() => false;

  public override string GetSound() => "AI_Notification_ResearchComplete";

  public override string GetMessageBody() => "";

  public override string GetTitle() => (string) MISC.NOTIFICATIONS.COLONY_ACHIEVEMENT_EARNED.NAME;

  public override string GetTooltip() => (string) MISC.NOTIFICATIONS.COLONY_ACHIEVEMENT_EARNED.TOOLTIP;

  public override bool IsValid() => true;

  public override void OnClick()
  {
    RetireColonyUtility.SaveColonySummaryData();
    MainMenu.ActivateRetiredColoniesScreenFromData(PauseScreen.Instance.transform.parent.gameObject, RetireColonyUtility.GetCurrentColonyRetiredColonyData());
  }
}
