﻿// Decompiled with JetBrains decompiler
// Type: StatusItemGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class StatusItemGroup
{
  private List<StatusItemGroup.Entry> items = new List<StatusItemGroup.Entry>();
  public System.Action<StatusItemGroup.Entry, StatusItemCategory> OnAddStatusItem;
  public System.Action<StatusItemGroup.Entry, bool> OnRemoveStatusItem;
  private Vector3 offset = new Vector3(0.0f, 0.0f, 0.0f);

  public IEnumerator<StatusItemGroup.Entry> GetEnumerator() => (IEnumerator<StatusItemGroup.Entry>) this.items.GetEnumerator();

  public GameObject gameObject { get; private set; }

  public StatusItemGroup(GameObject go) => this.gameObject = go;

  public void SetOffset(Vector3 offset)
  {
    this.offset = offset;
    Game.Instance.SetStatusItemOffset(this.gameObject.transform, offset);
  }

  public StatusItemGroup.Entry GetStatusItem(StatusItemCategory category)
  {
    for (int index = 0; index < this.items.Count; ++index)
    {
      if (this.items[index].category == category)
        return this.items[index];
    }
    return new StatusItemGroup.Entry();
  }

  public Guid SetStatusItem(StatusItemCategory category, StatusItem item, object data = null)
  {
    if (item != null && item.allowMultiples)
      throw new ArgumentException(item.Name + " allows multiple instances of itself to be active so you must access it via its handle");
    if (category == null)
      throw new ArgumentException("SetStatusItem requires a category.");
    for (int index = 0; index < this.items.Count; ++index)
    {
      if (this.items[index].category == category)
      {
        if (this.items[index].item == item)
        {
          this.Log("Set (exists in category)", item, this.items[index].id, category);
          return this.items[index].id;
        }
        this.Log("Set->Remove existing in category", item, this.items[index].id, category);
        this.RemoveStatusItem(this.items[index].id);
      }
    }
    if (item != null)
    {
      Guid guid = this.AddStatusItem(item, data, category);
      this.Log("Set (new)", item, guid, category);
      return guid;
    }
    this.Log("Set (failed)", item, Guid.Empty, category);
    return Guid.Empty;
  }

  public void SetStatusItem(
    Guid guid,
    StatusItemCategory category,
    StatusItem new_item,
    object data = null)
  {
    this.RemoveStatusItem(guid);
    if (new_item == null)
      return;
    this.AddStatusItem(new_item, data, category);
  }

  public bool HasStatusItem(StatusItem status_item)
  {
    for (int index = 0; index < this.items.Count; ++index)
    {
      if (this.items[index].item.Id == status_item.Id)
        return true;
    }
    return false;
  }

  public bool HasStatusItemID(StatusItem status_item)
  {
    for (int index = 0; index < this.items.Count; ++index)
    {
      if (this.items[index].item.Id == status_item.Id)
        return true;
    }
    return false;
  }

  public Guid AddStatusItem(StatusItem item, object data = null, StatusItemCategory category = null)
  {
    if ((UnityEngine.Object) this.gameObject == (UnityEngine.Object) null || !item.allowMultiples && this.HasStatusItem(item))
      return Guid.Empty;
    if (!item.allowMultiples)
    {
      foreach (StatusItemGroup.Entry entry in this.items)
      {
        if (entry.item.Id == item.Id)
          throw new ArgumentException("Tried to add " + item.Id + " multiples times which is not permitted.");
      }
    }
    StatusItemGroup.Entry entry1 = new StatusItemGroup.Entry(item, category, data);
    if (item.shouldNotify)
    {
      ref StatusItemGroup.Entry local = ref entry1;
      string notificationText = item.notificationText;
      int notificationType = (int) item.notificationType;
      HashedString invalid = HashedString.Invalid;
      Func<List<Notification>, object, string> tooltip = new Func<List<Notification>, object, string>(StatusItemGroup.OnToolTip);
      StatusItem statusItem = item;
      Notification.ClickCallback notificationClickCallback = item.notificationClickCallback;
      object obj = data;
      double notificationDelay = (double) item.notificationDelay;
      Notification.ClickCallback custom_click_callback = notificationClickCallback;
      object custom_click_data = obj;
      Notification notification = new Notification(notificationText, (NotificationType) notificationType, invalid, tooltip, (object) statusItem, false, (float) notificationDelay, custom_click_callback, custom_click_data);
      local.notification = notification;
      this.gameObject.AddOrGet<Notifier>().Add(entry1.notification);
    }
    if (item.ShouldShowIcon())
    {
      Game.Instance.AddStatusItem(this.gameObject.transform, item);
      Game.Instance.SetStatusItemOffset(this.gameObject.transform, this.offset);
    }
    this.items.Add(entry1);
    if (this.OnAddStatusItem != null)
      this.OnAddStatusItem(entry1, category);
    return entry1.id;
  }

  public Guid RemoveStatusItem(StatusItem status_item, bool immediate = false)
  {
    if (status_item.allowMultiples)
      throw new ArgumentException(status_item.Name + " allows multiple instances of itself to be active so it must be released via an instance handle");
    for (int index = 0; index < this.items.Count; ++index)
    {
      if (this.items[index].item.Id == status_item.Id)
        return this.RemoveStatusItem(this.items[index].id, immediate);
    }
    return Guid.Empty;
  }

  public Guid RemoveStatusItem(Guid guid, bool immediate = false)
  {
    if (guid == Guid.Empty)
      return guid;
    for (int index = 0; index < this.items.Count; ++index)
    {
      StatusItemGroup.Entry entry1 = this.items[index];
      if (entry1.id == guid)
      {
        StatusItemGroup.Entry entry2 = this.items[index];
        this.items.RemoveAt(index);
        if (entry2.notification != null)
          this.gameObject.GetComponent<Notifier>().Remove(entry2.notification);
        if (entry1.item.ShouldShowIcon() && (UnityEngine.Object) Game.Instance != (UnityEngine.Object) null)
          Game.Instance.RemoveStatusItem(this.gameObject.transform, entry2.item);
        if (this.OnRemoveStatusItem != null)
          this.OnRemoveStatusItem(entry2, immediate);
        return guid;
      }
    }
    return Guid.Empty;
  }

  private static string OnToolTip(List<Notification> notifications, object data) => ((StatusItem) data).notificationTooltipText + notifications.ReduceMessages();

  public void Destroy()
  {
    if (Game.IsQuitting())
      return;
    while (this.items.Count > 0)
      this.RemoveStatusItem(this.items[0].id);
  }

  [Conditional("ENABLE_LOGGER")]
  private void Log(string action, StatusItem item, Guid guid)
  {
  }

  private void Log(string action, StatusItem item, Guid guid, StatusItemCategory category)
  {
  }

  public struct Entry : IComparable<StatusItemGroup.Entry>, IEquatable<StatusItemGroup.Entry>
  {
    public Guid id;
    public StatusItem item;
    public object data;
    public Notification notification;
    public StatusItemCategory category;

    public Entry(StatusItem item, StatusItemCategory category, object data)
    {
      this.id = Guid.NewGuid();
      this.item = item;
      this.data = data;
      this.category = category;
      this.notification = (Notification) null;
    }

    public string GetName() => this.item.GetName(this.data);

    public void ShowToolTip(ToolTip tooltip_widget, TextStyleSetting property_style) => this.item.ShowToolTip(tooltip_widget, this.data, property_style);

    public void SetIcon(Image image) => this.item.SetIcon(image, this.data);

    public int CompareTo(StatusItemGroup.Entry other) => this.id.CompareTo(other.id);

    public bool Equals(StatusItemGroup.Entry other) => this.id == other.id;

    public void OnClick() => this.item.OnClick(this.data);
  }
}
