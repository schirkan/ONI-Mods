// Decompiled with JetBrains decompiler
// Type: Clearable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Clearable")]
public class Clearable : Workable, ISaveLoadable, IRender1000ms
{
  [MyCmpReq]
  private Pickupable pickupable;
  [MyCmpReq]
  private KSelectable selectable;
  [Serialize]
  private bool isMarkedForClear;
  private HandleVector<int>.Handle clearHandle;
  public bool isClearable = true;
  private Guid pendingClearGuid;
  private Guid pendingClearNoStorageGuid;
  private static readonly EventSystem.IntraObjectHandler<Clearable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Clearable>((System.Action<Clearable, object>) ((component, data) => component.OnCancel(data)));
  private static readonly EventSystem.IntraObjectHandler<Clearable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Clearable>((System.Action<Clearable, object>) ((component, data) => component.OnStore(data)));
  private static readonly EventSystem.IntraObjectHandler<Clearable> OnAbsorbDelegate = new EventSystem.IntraObjectHandler<Clearable>((System.Action<Clearable, object>) ((component, data) => component.OnAbsorb(data)));
  private static readonly EventSystem.IntraObjectHandler<Clearable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Clearable>((System.Action<Clearable, object>) ((component, data) => component.OnRefreshUserMenu(data)));
  private static readonly EventSystem.IntraObjectHandler<Clearable> OnEquippedDelegate = new EventSystem.IntraObjectHandler<Clearable>((System.Action<Clearable, object>) ((component, data) => component.OnEquipped(data)));

  protected override void OnPrefabInit()
  {
    this.Subscribe<Clearable>(2127324410, Clearable.OnCancelDelegate);
    this.Subscribe<Clearable>(856640610, Clearable.OnStoreDelegate);
    this.Subscribe<Clearable>(-2064133523, Clearable.OnAbsorbDelegate);
    this.Subscribe<Clearable>(493375141, Clearable.OnRefreshUserMenuDelegate);
    this.Subscribe<Clearable>(-1617557748, Clearable.OnEquippedDelegate);
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Clearing;
    this.simRenderLoadBalance = true;
    this.autoRegisterSimRender = false;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (this.isMarkedForClear)
    {
      if (this.HasTag(GameTags.Stored))
      {
        if (!this.transform.parent.GetComponent<Storage>().allowClearable)
          this.isMarkedForClear = false;
        else
          this.MarkForClear(true, true);
      }
      else
        this.MarkForClear(true);
    }
    this.RefreshClearableStatus(true);
  }

  private void OnStore(object data) => this.CancelClearing();

  private void OnCancel(object data)
  {
    for (ObjectLayerListItem objectLayerListItem = this.pickupable.objectLayerListItem; objectLayerListItem != null; objectLayerListItem = objectLayerListItem.nextItem)
    {
      if ((UnityEngine.Object) objectLayerListItem.gameObject != (UnityEngine.Object) null)
        objectLayerListItem.gameObject.GetComponent<Clearable>().CancelClearing();
    }
  }

  public void CancelClearing()
  {
    if (!this.isMarkedForClear)
      return;
    this.isMarkedForClear = false;
    this.GetComponent<KPrefabID>().RemoveTag(GameTags.Garbage);
    Prioritizable.RemoveRef(this.gameObject);
    if (this.clearHandle.IsValid())
    {
      GlobalChoreProvider.Instance.UnregisterClearable(this.clearHandle);
      this.clearHandle.Clear();
    }
    this.RefreshClearableStatus(true);
    SimAndRenderScheduler.instance.Remove((object) this);
  }

  public void MarkForClear(bool restoringFromSave = false, bool allowWhenStored = false)
  {
    if (!this.isClearable || !(!this.isMarkedForClear | restoringFromSave) || (this.pickupable.IsEntombed || this.clearHandle.IsValid()) || !(!this.HasTag(GameTags.Stored) | allowWhenStored))
      return;
    Prioritizable.AddRef(this.gameObject);
    this.GetComponent<KPrefabID>().AddTag(GameTags.Garbage);
    this.isMarkedForClear = true;
    this.clearHandle = GlobalChoreProvider.Instance.RegisterClearable(this);
    this.RefreshClearableStatus(true);
    SimAndRenderScheduler.instance.Add((object) this, this.simRenderLoadBalance);
  }

  private void OnClickClear() => this.MarkForClear();

  private void OnClickCancel() => this.CancelClearing();

  private void OnEquipped(object data) => this.CancelClearing();

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    if (!this.clearHandle.IsValid())
      return;
    GlobalChoreProvider.Instance.UnregisterClearable(this.clearHandle);
    this.clearHandle.Clear();
  }

  private void OnRefreshUserMenu(object data)
  {
    if (!this.isClearable || (UnityEngine.Object) this.GetComponent<Health>() != (UnityEngine.Object) null || this.HasTag(GameTags.Stored))
      return;
    Game.Instance.userMenu.AddButton(this.gameObject, this.isMarkedForClear ? new KIconButtonMenu.ButtonInfo("action_move_to_storage", (string) UI.USERMENUACTIONS.CLEAR.NAME_OFF, new System.Action(this.OnClickCancel), tooltipText: ((string) UI.USERMENUACTIONS.CLEAR.TOOLTIP_OFF)) : new KIconButtonMenu.ButtonInfo("action_move_to_storage", (string) UI.USERMENUACTIONS.CLEAR.NAME, new System.Action(this.OnClickClear), tooltipText: ((string) UI.USERMENUACTIONS.CLEAR.TOOLTIP)));
  }

  private void OnAbsorb(object data)
  {
    Pickupable pickupable = (Pickupable) data;
    if (!((UnityEngine.Object) pickupable != (UnityEngine.Object) null))
      return;
    Clearable component = pickupable.GetComponent<Clearable>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !component.isMarkedForClear)
      return;
    this.MarkForClear();
  }

  public void Render1000ms(float dt) => this.RefreshClearableStatus(false);

  public void RefreshClearableStatus(bool force_update)
  {
    if (!force_update && !this.isMarkedForClear)
      return;
    bool show1 = false;
    bool show2 = false;
    if (this.isMarkedForClear)
      show2 = !(show1 = GlobalChoreProvider.Instance.ClearableHasDestination(this.pickupable));
    this.pendingClearGuid = this.selectable.ToggleStatusItem(Db.Get().MiscStatusItems.PendingClear, this.pendingClearGuid, show1, (object) this);
    this.pendingClearNoStorageGuid = this.selectable.ToggleStatusItem(Db.Get().MiscStatusItems.PendingClearNoStorage, this.pendingClearNoStorageGuid, show2, (object) this);
  }
}
