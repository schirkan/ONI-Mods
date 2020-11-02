﻿// Decompiled with JetBrains decompiler
// Type: DoorToggleSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class DoorToggleSideScreen : SideScreenContent
{
  [SerializeField]
  private KToggle openButton;
  [SerializeField]
  private KToggle autoButton;
  [SerializeField]
  private KToggle closeButton;
  [SerializeField]
  private LocText description;
  private Door target;
  private AccessControl accessTarget;
  private List<DoorToggleSideScreen.DoorButtonInfo> buttonList = new List<DoorToggleSideScreen.DoorButtonInfo>();

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.InitButtons();
  }

  private void InitButtons()
  {
    this.buttonList.Add(new DoorToggleSideScreen.DoorButtonInfo()
    {
      button = this.openButton,
      state = Door.ControlState.Opened,
      currentString = (string) UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.OPEN,
      pendingString = (string) UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.OPEN_PENDING
    });
    this.buttonList.Add(new DoorToggleSideScreen.DoorButtonInfo()
    {
      button = this.autoButton,
      state = Door.ControlState.Auto,
      currentString = (string) UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.AUTO,
      pendingString = (string) UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.AUTO_PENDING
    });
    this.buttonList.Add(new DoorToggleSideScreen.DoorButtonInfo()
    {
      button = this.closeButton,
      state = Door.ControlState.Locked,
      currentString = (string) UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.CLOSE,
      pendingString = (string) UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.CLOSE_PENDING
    });
    foreach (DoorToggleSideScreen.DoorButtonInfo button in this.buttonList)
    {
      DoorToggleSideScreen.DoorButtonInfo info = button;
      info.button.onClick += (System.Action) (() =>
      {
        this.target.QueueStateChange(info.state);
        this.Refresh();
      });
    }
  }

  public override bool IsValidForTarget(GameObject target) => (UnityEngine.Object) target.GetComponent<Door>() != (UnityEngine.Object) null;

  public override void SetTarget(GameObject target)
  {
    if ((UnityEngine.Object) this.target != (UnityEngine.Object) null)
      this.ClearTarget();
    base.SetTarget(target);
    this.target = target.GetComponent<Door>();
    this.accessTarget = target.GetComponent<AccessControl>();
    if ((UnityEngine.Object) this.target == (UnityEngine.Object) null)
      return;
    target.Subscribe(1734268753, new System.Action<object>(this.OnDoorStateChanged));
    target.Subscribe(-1525636549, new System.Action<object>(this.OnAccessControlChanged));
    this.Refresh();
    this.gameObject.SetActive(true);
  }

  public override void ClearTarget()
  {
    if ((UnityEngine.Object) this.target != (UnityEngine.Object) null)
    {
      this.target.Unsubscribe(1734268753, new System.Action<object>(this.OnDoorStateChanged));
      this.target.Unsubscribe(-1525636549, new System.Action<object>(this.OnAccessControlChanged));
    }
    this.target = (Door) null;
  }

  private void Refresh()
  {
    string str1 = (string) null;
    string str2 = (string) null;
    if (this.buttonList == null || this.buttonList.Count == 0)
      this.InitButtons();
    foreach (DoorToggleSideScreen.DoorButtonInfo button in this.buttonList)
    {
      if (this.target.CurrentState == button.state && this.target.RequestedState == button.state)
      {
        button.button.isOn = true;
        str1 = button.currentString;
        foreach (ImageToggleState componentsInChild in button.button.GetComponentsInChildren<ImageToggleState>())
        {
          componentsInChild.SetActive();
          componentsInChild.SetActive();
        }
        button.button.GetComponent<ImageToggleStateThrobber>().enabled = false;
      }
      else if (this.target.RequestedState == button.state)
      {
        button.button.isOn = true;
        str2 = button.pendingString;
        foreach (ImageToggleState componentsInChild in button.button.GetComponentsInChildren<ImageToggleState>())
        {
          componentsInChild.SetActive();
          componentsInChild.SetActive();
        }
        button.button.GetComponent<ImageToggleStateThrobber>().enabled = true;
      }
      else
      {
        button.button.isOn = false;
        foreach (ImageToggleState componentsInChild in button.button.GetComponentsInChildren<ImageToggleState>())
        {
          componentsInChild.SetInactive();
          componentsInChild.SetInactive();
        }
        button.button.GetComponent<ImageToggleStateThrobber>().enabled = false;
      }
    }
    string str3 = str1;
    if (str2 != null)
      str3 = string.Format((string) UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.PENDING_FORMAT, (object) str3, (object) str2);
    if ((UnityEngine.Object) this.accessTarget != (UnityEngine.Object) null && !this.accessTarget.Online)
      str3 = string.Format((string) UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.ACCESS_FORMAT, (object) str3, (object) UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.ACCESS_OFFLINE);
    if (this.target.building.Def.PrefabID == POIDoorInternalConfig.ID)
    {
      str3 = (string) UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.POI_INTERNAL;
      foreach (DoorToggleSideScreen.DoorButtonInfo button in this.buttonList)
        button.button.gameObject.SetActive(false);
    }
    else
    {
      foreach (DoorToggleSideScreen.DoorButtonInfo button in this.buttonList)
        button.button.gameObject.SetActive(button.state != Door.ControlState.Auto || this.target.allowAutoControl);
    }
    this.description.text = str3;
    this.description.gameObject.SetActive(!string.IsNullOrEmpty(str3));
    this.ContentContainer.SetActive(!this.target.isSealed);
  }

  private void OnDoorStateChanged(object data) => this.Refresh();

  private void OnAccessControlChanged(object data) => this.Refresh();

  private struct DoorButtonInfo
  {
    public KToggle button;
    public Door.ControlState state;
    public string currentString;
    public string pendingString;
  }
}
