﻿// Decompiled with JetBrains decompiler
// Type: DropAllWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/DropAllWorkable")]
public class DropAllWorkable : Workable
{
  private Chore chore;
  private bool showCmd;
  private Storage[] storages;
  [MyCmpAdd]
  private Prioritizable _prioritizable;
  private static readonly EventSystem.IntraObjectHandler<DropAllWorkable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<DropAllWorkable>((System.Action<DropAllWorkable, object>) ((component, data) => component.OnRefreshUserMenu(data)));
  private static readonly EventSystem.IntraObjectHandler<DropAllWorkable> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<DropAllWorkable>((System.Action<DropAllWorkable, object>) ((component, data) => component.OnStorageChange(data)));

  protected DropAllWorkable() => this.SetOffsetTable(OffsetGroups.InvertedStandardTable);

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<DropAllWorkable>(493375141, DropAllWorkable.OnRefreshUserMenuDelegate);
    this.Subscribe<DropAllWorkable>(-1697596308, DropAllWorkable.OnStorageChangeDelegate);
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Emptying;
    this.synchronizeAnims = false;
    this.SetWorkTime(0.1f);
    Prioritizable.AddRef(this.gameObject);
  }

  private Storage[] GetStorages()
  {
    if (this.storages == null)
      this.storages = this.GetComponents<Storage>();
    return this.storages;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.showCmd = this.GetNewShowCmd();
  }

  public void DropAll()
  {
    if (DebugHandler.InstantBuildMode)
      this.OnCompleteWork((Worker) null);
    else if (this.chore == null)
    {
      this.chore = (Chore) new WorkChore<DropAllWorkable>(Db.Get().ChoreTypes.EmptyStorage, (IStateMachineTarget) this, only_when_operational: false);
    }
    else
    {
      this.chore.Cancel("Cancelled emptying");
      this.chore = (Chore) null;
      this.GetComponent<KSelectable>().RemoveStatusItem(this.workerStatusItem);
      this.ShowProgressBar(false);
    }
  }

  protected override void OnCompleteWork(Worker worker)
  {
    Storage[] storages = this.GetStorages();
    for (int index1 = 0; index1 < storages.Length; ++index1)
    {
      List<GameObject> gameObjectList = new List<GameObject>((IEnumerable<GameObject>) storages[index1].items);
      for (int index2 = 0; index2 < gameObjectList.Count; ++index2)
      {
        GameObject gameObject = storages[index1].Drop(gameObjectList[index2]);
        if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
        {
          Pickupable component = gameObject.GetComponent<Pickupable>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
            component.TryToOffsetIfBuried();
        }
      }
    }
    this.chore = (Chore) null;
    this.Trigger(-1957399615, (object) null);
  }

  private void OnRefreshUserMenu(object data)
  {
    if (!this.showCmd)
      return;
    Game.Instance.userMenu.AddButton(this.gameObject, this.chore == null ? new KIconButtonMenu.ButtonInfo("action_empty_contents", (string) UI.USERMENUACTIONS.EMPTYSTORAGE.NAME, new System.Action(this.DropAll), tooltipText: ((string) UI.USERMENUACTIONS.EMPTYSTORAGE.TOOLTIP)) : new KIconButtonMenu.ButtonInfo("action_empty_contents", (string) UI.USERMENUACTIONS.EMPTYSTORAGE.NAME_OFF, new System.Action(this.DropAll), tooltipText: ((string) UI.USERMENUACTIONS.EMPTYSTORAGE.TOOLTIP_OFF)));
  }

  private bool GetNewShowCmd()
  {
    bool flag = false;
    foreach (Storage storage in this.GetStorages())
      flag = flag || !storage.IsEmpty();
    return flag;
  }

  private void OnStorageChange(object data)
  {
    bool newShowCmd = this.GetNewShowCmd();
    if (newShowCmd == this.showCmd)
      return;
    this.showCmd = newShowCmd;
    Game.Instance.userMenu.Refresh(this.gameObject);
  }
}
