// Decompiled with JetBrains decompiler
// Type: NotificationScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NotificationScreen : KScreen
{
  public float lifetime;
  public bool dirty;
  public GameObject LabelPrefab;
  public GameObject LabelsFolder;
  public GameObject MessagesPrefab;
  public GameObject MessagesFolder;
  private MessageDialogFrame messageDialog;
  private float initTime;
  private int notificationIncrement;
  [MyCmpAdd]
  private Notifier notifier;
  [SerializeField]
  private List<MessageDialog> dialogPrefabs = new List<MessageDialog>();
  [SerializeField]
  private Color badColorBG;
  [SerializeField]
  private Color badColor = Color.red;
  [SerializeField]
  private Color normalColorBG;
  [SerializeField]
  private Color normalColor = Color.white;
  [SerializeField]
  private Color warningColorBG;
  [SerializeField]
  private Color warningColor;
  [SerializeField]
  private Color messageColorBG;
  [SerializeField]
  private Color messageColor;
  public Sprite icon_normal;
  public Sprite icon_warning;
  public Sprite icon_bad;
  public Sprite icon_threatening;
  public Sprite icon_message;
  public Sprite icon_video;
  private List<Notification> pendingNotifications = new List<Notification>();
  private List<Notification> notifications = new List<Notification>();
  public TextStyleSetting TooltipTextStyle;
  private Dictionary<NotificationType, string> notificationSounds = new Dictionary<NotificationType, string>();
  private Dictionary<string, float> timeOfLastNotification = new Dictionary<string, float>();
  private float soundDecayTime = 10f;
  private List<NotificationScreen.Entry> entries = new List<NotificationScreen.Entry>();
  private Dictionary<string, NotificationScreen.Entry> entriesByMessage = new Dictionary<string, NotificationScreen.Entry>();

  public static NotificationScreen Instance { get; private set; }

  public static void DestroyInstance() => NotificationScreen.Instance = (NotificationScreen) null;

  private void OnAddNotifier(Notifier notifier)
  {
    notifier.OnAdd += new System.Action<Notification>(this.OnAddNotification);
    notifier.OnRemove += new System.Action<Notification>(this.OnRemoveNotification);
  }

  private void OnRemoveNotifier(Notifier notifier)
  {
    notifier.OnAdd -= new System.Action<Notification>(this.OnAddNotification);
    notifier.OnRemove -= new System.Action<Notification>(this.OnRemoveNotification);
  }

  private void OnAddNotification(Notification notification) => this.pendingNotifications.Add(notification);

  private void OnRemoveNotification(Notification notification)
  {
    this.dirty = true;
    this.pendingNotifications.Remove(notification);
    NotificationScreen.Entry entry = (NotificationScreen.Entry) null;
    this.entriesByMessage.TryGetValue(notification.titleText, out entry);
    if (entry == null)
      return;
    this.notifications.Remove(notification);
    entry.Remove(notification);
    if (entry.notifications.Count != 0)
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) entry.label);
    this.entriesByMessage[notification.titleText] = (NotificationScreen.Entry) null;
    this.entries.Remove(entry);
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    NotificationScreen.Instance = this;
    Components.Notifiers.OnAdd += new System.Action<Notifier>(this.OnAddNotifier);
    Components.Notifiers.OnRemove += new System.Action<Notifier>(this.OnRemoveNotifier);
    foreach (Notifier notifier in Components.Notifiers.Items)
      this.OnAddNotifier(notifier);
    this.MessagesPrefab.gameObject.SetActive(false);
    this.LabelPrefab.gameObject.SetActive(false);
    this.InitNotificationSounds();
  }

  private void OnNewMessage(object data) => this.notifier.Add((Notification) new MessageNotification((Message) data));

  private void ShowMessage(MessageNotification mn)
  {
    mn.message.OnClick();
    if (mn.message.ShowDialog())
    {
      for (int index = 0; index < this.dialogPrefabs.Count; ++index)
      {
        if (this.dialogPrefabs[index].CanDisplay(mn.message))
        {
          if ((UnityEngine.Object) this.messageDialog != (UnityEngine.Object) null)
          {
            UnityEngine.Object.Destroy((UnityEngine.Object) this.messageDialog.gameObject);
            this.messageDialog = (MessageDialogFrame) null;
          }
          this.messageDialog = Util.KInstantiateUI<MessageDialogFrame>(ScreenPrefabs.Instance.MessageDialogFrame.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject);
          this.messageDialog.SetMessage(Util.KInstantiateUI<MessageDialog>(this.dialogPrefabs[index].gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject), mn.message);
          this.messageDialog.Show();
          break;
        }
      }
    }
    Messenger.Instance.RemoveMessage(mn.message);
    mn.Clear();
  }

  public void OnClickNextMessage() => this.ShowMessage((MessageNotification) this.notifications.Find((Predicate<Notification>) (notification => notification.Type == NotificationType.Messages)));

  protected override void OnCleanUp()
  {
    Components.Notifiers.OnAdd -= new System.Action<Notifier>(this.OnAddNotifier);
    Components.Notifiers.OnRemove -= new System.Action<Notifier>(this.OnRemoveNotifier);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.initTime = KTime.Instance.UnscaledGameTime;
    foreach (Graphic componentsInChild in this.LabelPrefab.GetComponentsInChildren<LocText>())
      componentsInChild.color = this.normalColor;
    foreach (Graphic componentsInChild in this.MessagesPrefab.GetComponentsInChildren<LocText>())
      componentsInChild.color = this.normalColor;
    this.Subscribe(Messenger.Instance.gameObject, 1558809273, new System.Action<object>(this.OnNewMessage));
    foreach (Message message in Messenger.Instance.Messages)
    {
      Notification notification = (Notification) new MessageNotification(message);
      notification.playSound = false;
      this.notifier.Add(notification);
    }
  }

  protected override void OnActivate()
  {
    base.OnActivate();
    this.dirty = true;
  }

  private void AddNotification(Notification notification)
  {
    this.notifications.Add(notification);
    notification.Idx = this.notificationIncrement++;
    NotificationScreen.Entry entry = (NotificationScreen.Entry) null;
    this.entriesByMessage.TryGetValue(notification.titleText, out entry);
    if (entry == null)
    {
      GameObject label = notification.Type != NotificationType.Messages ? Util.KInstantiateUI(this.LabelPrefab, this.LabelsFolder) : Util.KInstantiateUI(this.MessagesPrefab, this.MessagesFolder);
      label.GetComponentInChildren<NotificationAnimator>().Init();
      label.gameObject.SetActive(true);
      KImage componentInChildren1 = label.GetComponentInChildren<KImage>(true);
      UnityEngine.UI.Button[] componentsInChildren = label.gameObject.GetComponentsInChildren<UnityEngine.UI.Button>();
      ColorBlock colors = componentsInChildren[0].colors;
      if (notification.Type == NotificationType.Bad || notification.Type == NotificationType.DuplicantThreatening)
        colors.normalColor = this.badColorBG;
      else if (notification.Type == NotificationType.Messages)
      {
        colors.normalColor = this.messageColorBG;
        Debug.Assert(notification.GetType() == typeof (MessageNotification), (object) string.Format("Notification: \"{0}\" is not of type MessageNotification", (object) notification.titleText));
        componentsInChildren[1].onClick.AddListener((UnityAction) (() =>
        {
          foreach (MessageNotification messageNotification in this.notifications.FindAll((Predicate<Notification>) (n => n.titleText == notification.titleText)))
          {
            Messenger.Instance.RemoveMessage(messageNotification.message);
            messageNotification.Clear();
          }
        }));
      }
      else
        colors.normalColor = notification.Type != NotificationType.Tutorial ? this.normalColorBG : this.warningColorBG;
      componentsInChildren[0].colors = colors;
      componentsInChildren[0].onClick.AddListener((UnityAction) (() => this.OnClick(entry)));
      if (notification.ToolTip != null)
        label.GetComponentInChildren<ToolTip>().OnToolTip = (Func<string>) (() =>
        {
          ToolTip componentInChildren = label.GetComponentInChildren<ToolTip>();
          componentInChildren.ClearMultiStringTooltip();
          componentInChildren.AddMultiStringTooltip(notification.ToolTip(entry.notifications, notification.tooltipData), (ScriptableObject) this.TooltipTextStyle);
          return "";
        });
      entry = new NotificationScreen.Entry(label);
      this.entriesByMessage[notification.titleText] = entry;
      this.entries.Add(entry);
      foreach (LocText componentsInChild in label.GetComponentsInChildren<LocText>())
      {
        switch (notification.Type)
        {
          case NotificationType.Bad:
            componentsInChild.color = this.badColor;
            componentInChildren1.sprite = this.icon_bad;
            break;
          case NotificationType.Tutorial:
            componentsInChild.color = this.warningColor;
            componentInChildren1.sprite = this.icon_warning;
            break;
          case NotificationType.Messages:
            componentsInChild.color = this.messageColor;
            componentInChildren1.sprite = this.icon_message;
            if (notification is MessageNotification messageNotification && messageNotification.message is TutorialMessage message && !string.IsNullOrEmpty(message.videoClipId))
            {
              componentInChildren1.sprite = this.icon_video;
              break;
            }
            break;
          case NotificationType.DuplicantThreatening:
            componentsInChild.color = this.badColor;
            componentInChildren1.sprite = this.icon_threatening;
            break;
          default:
            componentsInChild.color = this.normalColor;
            componentInChildren1.sprite = this.icon_normal;
            break;
        }
        componentInChildren1.color = componentsInChild.color;
        string str = "";
        if ((double) KTime.Instance.UnscaledGameTime - (double) this.initTime > 5.0 && notification.playSound)
          this.PlayDingSound(notification, 0);
        else
          str = "too early";
        if (AudioDebug.Get().debugNotificationSounds)
          Debug.Log((object) ("Notification(" + notification.titleText + "):" + str));
      }
    }
    entry.Add(notification);
    entry.UpdateMessage(notification);
    this.dirty = true;
    this.SortNotifications();
  }

  private void SortNotifications()
  {
    this.notifications.Sort((Comparison<Notification>) ((n1, n2) => n1.Type == n2.Type ? n1.Idx - n2.Idx : n1.Type - n2.Type));
    foreach (Notification notification in this.notifications)
    {
      NotificationScreen.Entry entry = (NotificationScreen.Entry) null;
      this.entriesByMessage.TryGetValue(notification.titleText, out entry);
      entry?.label.GetComponent<RectTransform>().SetAsLastSibling();
    }
  }

  private void PlayDingSound(Notification notification, int count)
  {
    string str;
    if (!this.notificationSounds.TryGetValue(notification.Type, out str))
      str = "Notification";
    float num1;
    if (!this.timeOfLastNotification.TryGetValue(str, out num1))
      num1 = 0.0f;
    float num2 = notification.volume_attenuation ? (Time.time - num1) / this.soundDecayTime : 1f;
    this.timeOfLastNotification[str] = Time.time;
    string sound = count <= 1 ? GlobalAssets.GetSound(str) : GlobalAssets.GetSound(str + "_AddCount", true) ?? GlobalAssets.GetSound(str);
    if (!notification.playSound)
      return;
    EventInstance instance = KFMOD.BeginOneShot(sound, Vector3.zero);
    int num3 = (int) instance.setParameterValue("timeSinceLast", num2);
    KFMOD.EndOneShot(instance);
  }

  private void Update()
  {
    int index1 = 0;
    while (index1 < this.pendingNotifications.Count)
    {
      if (this.pendingNotifications[index1].IsReady())
      {
        this.AddNotification(this.pendingNotifications[index1]);
        this.pendingNotifications.RemoveAt(index1);
      }
      else
        ++index1;
    }
    int num1 = 0;
    int num2 = 0;
    for (int index2 = 0; index2 < this.notifications.Count; ++index2)
    {
      Notification notification = this.notifications[index2];
      if (notification.Type == NotificationType.Messages)
        ++num2;
      else
        ++num1;
      if (notification.expires && (double) KTime.Instance.UnscaledGameTime - (double) notification.Time > (double) this.lifetime)
      {
        this.dirty = true;
        if ((UnityEngine.Object) notification.Notifier == (UnityEngine.Object) null)
          this.OnRemoveNotification(notification);
        else
          notification.Clear();
      }
    }
  }

  private void OnClick(NotificationScreen.Entry entry)
  {
    Notification clickedNotification = entry.NextClickedNotification;
    this.PlaySound3D(GlobalAssets.GetSound("HUD_Click_Open"));
    if (clickedNotification.customClickCallback != null)
    {
      clickedNotification.customClickCallback(clickedNotification.customClickData);
    }
    else
    {
      if ((UnityEngine.Object) clickedNotification.clickFocus != (UnityEngine.Object) null)
      {
        Vector3 position = clickedNotification.clickFocus.GetPosition();
        position.z = -40f;
        CameraController.Instance.SetTargetPos(position, 8f, true);
        if ((UnityEngine.Object) clickedNotification.clickFocus.GetComponent<KSelectable>() != (UnityEngine.Object) null)
          SelectTool.Instance.Select(clickedNotification.clickFocus.GetComponent<KSelectable>());
      }
      else if ((UnityEngine.Object) clickedNotification.Notifier != (UnityEngine.Object) null)
        SelectTool.Instance.Select(clickedNotification.Notifier.GetComponent<KSelectable>());
      if (clickedNotification.Type != NotificationType.Messages)
        return;
      this.ShowMessage((MessageNotification) clickedNotification);
    }
  }

  private void PositionLocatorIcon()
  {
  }

  private void InitNotificationSounds()
  {
    this.notificationSounds[NotificationType.Good] = "Notification";
    this.notificationSounds[NotificationType.BadMinor] = "Notification";
    this.notificationSounds[NotificationType.Bad] = "Warning";
    this.notificationSounds[NotificationType.Neutral] = "Notification";
    this.notificationSounds[NotificationType.Tutorial] = "Notification";
    this.notificationSounds[NotificationType.Messages] = "Message";
    this.notificationSounds[NotificationType.DuplicantThreatening] = "Warning_DupeThreatening";
  }

  public Color32 BadColorBG => (Color32) this.badColorBG;

  public Sprite GetNotificationIcon(NotificationType type)
  {
    switch (type)
    {
      case NotificationType.Bad:
        return this.icon_bad;
      case NotificationType.Tutorial:
        return this.icon_warning;
      case NotificationType.Messages:
        return this.icon_message;
      case NotificationType.DuplicantThreatening:
        return this.icon_threatening;
      default:
        return this.icon_normal;
    }
  }

  public Color GetNotificationColour(NotificationType type)
  {
    switch (type)
    {
      case NotificationType.Bad:
        return this.badColor;
      case NotificationType.Tutorial:
        return this.warningColor;
      case NotificationType.Messages:
        return this.messageColor;
      case NotificationType.DuplicantThreatening:
        return this.badColor;
      default:
        return this.normalColor;
    }
  }

  public Color GetNotificationBGColour(NotificationType type)
  {
    switch (type)
    {
      case NotificationType.Bad:
        return this.badColorBG;
      case NotificationType.Tutorial:
        return this.warningColorBG;
      case NotificationType.Messages:
        return this.messageColorBG;
      case NotificationType.DuplicantThreatening:
        return this.badColorBG;
      default:
        return this.normalColorBG;
    }
  }

  public string GetNotificationSound(NotificationType type) => this.notificationSounds[type];

  private class Entry
  {
    public string message;
    public int clickIdx;
    public GameObject label;
    public List<Notification> notifications = new List<Notification>();

    public Entry(GameObject label) => this.label = label;

    public void Add(Notification notification)
    {
      this.notifications.Add(notification);
      this.UpdateMessage(notification);
    }

    public void Remove(Notification notification)
    {
      this.notifications.Remove(notification);
      this.UpdateMessage(notification, false);
    }

    public void UpdateMessage(Notification notification, bool playSound = true)
    {
      if (Game.IsQuitting())
        return;
      this.message = notification.titleText;
      if (this.notifications.Count > 1)
      {
        if (playSound && (notification.Type == NotificationType.Bad || notification.Type == NotificationType.DuplicantThreatening))
          NotificationScreen.Instance.PlayDingSound(notification, this.notifications.Count);
        this.message = this.message + " (" + this.notifications.Count.ToString() + ")";
      }
      if (!((UnityEngine.Object) this.label.gameObject != (UnityEngine.Object) null))
        return;
      this.label.GetComponentInChildren<LocText>().text = this.message;
    }

    public Notification NextClickedNotification => this.notifications[this.clickIdx++ % this.notifications.Count];
  }
}
