// Decompiled with JetBrains decompiler
// Type: LogicAlarm
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/LogicAlarm")]
public class LogicAlarm : KMonoBehaviour, ISaveLoadable
{
  [Serialize]
  public string notificationName;
  [Serialize]
  public string notificationTooltip;
  [Serialize]
  public NotificationType notificationType;
  [Serialize]
  public bool pauseOnNotify;
  [Serialize]
  public bool zoomOnNotify;
  [Serialize]
  public float cooldown;
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;
  private bool wasOn;
  private Notifier notifier;
  private Notification notification;
  private Notification lastNotificationCreated;
  private static readonly EventSystem.IntraObjectHandler<LogicAlarm> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicAlarm>((System.Action<LogicAlarm, object>) ((component, data) => component.OnCopySettings(data)));
  private static readonly EventSystem.IntraObjectHandler<LogicAlarm> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicAlarm>((System.Action<LogicAlarm, object>) ((component, data) => component.OnLogicValueChanged(data)));
  public static readonly HashedString INPUT_PORT_ID = new HashedString("LogicAlarmInput");
  protected static readonly HashedString[] ON_ANIMS = new HashedString[2]
  {
    (HashedString) "on_pre",
    (HashedString) "on_loop"
  };
  protected static readonly HashedString[] OFF_ANIMS = new HashedString[2]
  {
    (HashedString) "on_pst",
    (HashedString) "off"
  };

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<LogicAlarm>(-905833192, LogicAlarm.OnCopySettingsDelegate);
  }

  private void OnCopySettings(object data)
  {
    LogicAlarm component = ((GameObject) data).GetComponent<LogicAlarm>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.notificationName = component.notificationName;
    this.notificationType = component.notificationType;
    this.pauseOnNotify = component.pauseOnNotify;
    this.zoomOnNotify = component.zoomOnNotify;
    this.cooldown = component.cooldown;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.notifier = this.gameObject.AddComponent<Notifier>();
    this.Subscribe<LogicAlarm>(-801688580, LogicAlarm.OnLogicValueChangedDelegate);
    if (string.IsNullOrEmpty(this.notificationName))
      this.notificationName = (string) UI.UISIDESCREENS.LOGICALARMSIDESCREEN.NAME_DEFAULT;
    if (string.IsNullOrEmpty(this.notificationTooltip))
      this.notificationTooltip = (string) UI.UISIDESCREENS.LOGICALARMSIDESCREEN.TOOLTIP_DEFAULT;
    this.UpdateVisualState();
    this.UpdateNotification(false);
  }

  private void UpdateVisualState() => this.GetComponent<KBatchedAnimController>().Play(this.wasOn ? LogicAlarm.ON_ANIMS : LogicAlarm.OFF_ANIMS);

  public void OnLogicValueChanged(object data)
  {
    LogicValueChanged logicValueChanged = (LogicValueChanged) data;
    if (logicValueChanged.portID != LogicAlarm.INPUT_PORT_ID)
      return;
    if (LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue))
    {
      if (this.wasOn)
        return;
      this.PushNotification();
      this.wasOn = true;
      if (this.pauseOnNotify && !SpeedControlScreen.Instance.IsPaused)
        SpeedControlScreen.Instance.Pause(false);
      if (this.zoomOnNotify)
        CameraController.Instance.SetTargetPos(this.transform.position, 8f, true);
      this.UpdateVisualState();
    }
    else
    {
      if (!this.wasOn)
        return;
      this.wasOn = false;
      this.UpdateVisualState();
    }
  }

  private void PushNotification()
  {
    this.notification.Clear();
    this.notifier.Add(this.notification);
  }

  public void UpdateNotification(bool clear)
  {
    if (this.notification != null & clear)
    {
      this.notification.Clear();
      this.lastNotificationCreated = (Notification) null;
    }
    if (this.notification == this.lastNotificationCreated && this.lastNotificationCreated != null)
      return;
    this.notification = this.CreateNotification();
  }

  public Notification CreateNotification()
  {
    this.GetComponent<KSelectable>();
    Notification notification = new Notification(this.notificationName, this.notificationType, HashedString.Invalid, (Func<List<Notification>, object, string>) ((n, d) => this.notificationTooltip), volume_attenuation: false);
    this.lastNotificationCreated = notification;
    return notification;
  }
}
