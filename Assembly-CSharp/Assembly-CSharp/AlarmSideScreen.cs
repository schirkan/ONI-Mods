// Decompiled with JetBrains decompiler
// Type: AlarmSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlarmSideScreen : SideScreenContent
{
  public LogicAlarm targetAlarm;
  [SerializeField]
  private KInputField nameInputField;
  [SerializeField]
  private KInputField tooltipInputField;
  [SerializeField]
  private KToggle pauseToggle;
  [SerializeField]
  private Image pauseCheckmark;
  [SerializeField]
  private KToggle zoomToggle;
  [SerializeField]
  private Image zoomCheckmark;
  [SerializeField]
  private GameObject typeButtonPrefab;
  private List<NotificationType> validTypes = new List<NotificationType>()
  {
    NotificationType.Bad,
    NotificationType.Neutral,
    NotificationType.DuplicantThreatening
  };
  private Dictionary<NotificationType, MultiToggle> toggles_by_type = new Dictionary<NotificationType, MultiToggle>();

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.nameInputField.onEndEdit += new System.Action(this.OnEndEditName);
    this.nameInputField.field.characterLimit = 30;
    this.tooltipInputField.onEndEdit += new System.Action(this.OnEndEditTooltip);
    this.tooltipInputField.field.characterLimit = 90;
    this.pauseToggle.onClick += new System.Action(this.TogglePause);
    this.zoomToggle.onClick += new System.Action(this.ToggleZoom);
    this.InitializeToggles();
  }

  private void OnEndEditName()
  {
    this.targetAlarm.notificationName = this.nameInputField.field.text;
    this.UpdateNotification(true);
  }

  private void OnEndEditTooltip()
  {
    this.targetAlarm.notificationTooltip = this.tooltipInputField.field.text;
    this.UpdateNotification(true);
  }

  private void TogglePause()
  {
    this.targetAlarm.pauseOnNotify = !this.targetAlarm.pauseOnNotify;
    this.pauseCheckmark.enabled = this.targetAlarm.pauseOnNotify;
    this.UpdateNotification(true);
  }

  private void ToggleZoom()
  {
    this.targetAlarm.zoomOnNotify = !this.targetAlarm.zoomOnNotify;
    this.zoomCheckmark.enabled = this.targetAlarm.zoomOnNotify;
    this.UpdateNotification(true);
  }

  private void SelectType(NotificationType type)
  {
    this.targetAlarm.notificationType = type;
    this.UpdateNotification(true);
    this.RefreshToggles();
  }

  private void InitializeToggles()
  {
    if (this.toggles_by_type.Count != 0)
      return;
    foreach (NotificationType validType in this.validTypes)
    {
      NotificationType type = validType;
      GameObject gameObject = Util.KInstantiateUI(this.typeButtonPrefab, this.typeButtonPrefab.transform.parent.gameObject, true);
      gameObject.name = "TypeButton: " + (object) type;
      HierarchyReferences component1 = gameObject.GetComponent<HierarchyReferences>();
      Color notificationBgColour = NotificationScreen.Instance.GetNotificationBGColour(type);
      Color notificationColour = NotificationScreen.Instance.GetNotificationColour(type);
      notificationBgColour.a = 1f;
      notificationColour.a = 1f;
      component1.GetReference<KImage>("bg").color = notificationBgColour;
      component1.GetReference<KImage>("icon").color = notificationColour;
      component1.GetReference<KImage>("icon").sprite = NotificationScreen.Instance.GetNotificationIcon(type);
      ToolTip component2 = gameObject.GetComponent<ToolTip>();
      switch (type)
      {
        case NotificationType.Bad:
          component2.SetSimpleTooltip((string) STRINGS.UI.UISIDESCREENS.LOGICALARMSIDESCREEN.TOOLTIPS.BAD);
          break;
        case NotificationType.Neutral:
          component2.SetSimpleTooltip((string) STRINGS.UI.UISIDESCREENS.LOGICALARMSIDESCREEN.TOOLTIPS.NEUTRAL);
          break;
        case NotificationType.DuplicantThreatening:
          component2.SetSimpleTooltip((string) STRINGS.UI.UISIDESCREENS.LOGICALARMSIDESCREEN.TOOLTIPS.DUPLICANT_THREATENING);
          break;
      }
      if (!this.toggles_by_type.ContainsKey(type))
        this.toggles_by_type.Add(type, gameObject.GetComponent<MultiToggle>());
      this.toggles_by_type[type].onClick = (System.Action) (() => this.SelectType(type));
      for (int index = 0; index < this.toggles_by_type[type].states.Length; ++index)
        this.toggles_by_type[type].states[index].on_click_override_sound_path = NotificationScreen.Instance.GetNotificationSound(type);
    }
  }

  private void RefreshToggles()
  {
    this.InitializeToggles();
    foreach (KeyValuePair<NotificationType, MultiToggle> keyValuePair in this.toggles_by_type)
    {
      if (this.targetAlarm.notificationType == keyValuePair.Key)
        keyValuePair.Value.ChangeState(0);
      else
        keyValuePair.Value.ChangeState(1);
    }
  }

  public override bool IsValidForTarget(GameObject target) => (UnityEngine.Object) target.GetComponent<LogicAlarm>() != (UnityEngine.Object) null;

  public override void SetTarget(GameObject target)
  {
    base.SetTarget(target);
    this.targetAlarm = target.GetComponent<LogicAlarm>();
    this.RefreshToggles();
    this.UpdateVisuals();
  }

  private void UpdateNotification(bool clear) => this.targetAlarm.UpdateNotification(clear);

  private void UpdateVisuals()
  {
    this.nameInputField.SetDisplayValue(this.targetAlarm.notificationName);
    this.tooltipInputField.SetDisplayValue(this.targetAlarm.notificationTooltip);
    this.pauseCheckmark.enabled = this.targetAlarm.pauseOnNotify;
    this.zoomCheckmark.enabled = this.targetAlarm.zoomOnNotify;
  }
}
