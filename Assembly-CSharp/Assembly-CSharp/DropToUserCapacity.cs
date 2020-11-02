﻿// Decompiled with JetBrains decompiler
// Type: DropToUserCapacity
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/DropToUserCapacity")]
public class DropToUserCapacity : Workable
{
  private Chore chore;
  private bool showCmd;
  private Storage[] storages;
  private static readonly EventSystem.IntraObjectHandler<DropToUserCapacity> OnStorageCapacityChangedHandler = new EventSystem.IntraObjectHandler<DropToUserCapacity>((System.Action<DropToUserCapacity, object>) ((component, data) => component.OnStorageChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<DropToUserCapacity> OnStorageChangedHandler = new EventSystem.IntraObjectHandler<DropToUserCapacity>((System.Action<DropToUserCapacity, object>) ((component, data) => component.OnStorageChanged(data)));

  protected DropToUserCapacity() => this.SetOffsetTable(OffsetGroups.InvertedStandardTable);

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Emptying;
    this.Subscribe<DropToUserCapacity>(-945020481, DropToUserCapacity.OnStorageCapacityChangedHandler);
    this.Subscribe<DropToUserCapacity>(-1697596308, DropToUserCapacity.OnStorageChangedHandler);
    this.synchronizeAnims = false;
    this.SetWorkTime(0.1f);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.UpdateChore();
  }

  private Storage[] GetStorages()
  {
    if (this.storages == null)
      this.storages = this.GetComponents<Storage>();
    return this.storages;
  }

  private void OnStorageChanged(object data) => this.UpdateChore();

  public void UpdateChore()
  {
    IUserControlledCapacity component = this.GetComponent<IUserControlledCapacity>();
    if (component != null && (double) component.AmountStored > (double) component.UserMaxCapacity)
    {
      if (this.chore != null)
        return;
      this.chore = (Chore) new WorkChore<DropToUserCapacity>(Db.Get().ChoreTypes.EmptyStorage, (IStateMachineTarget) this, only_when_operational: false);
    }
    else
    {
      if (this.chore == null)
        return;
      this.chore.Cancel("Cancelled emptying");
      this.chore = (Chore) null;
      this.GetComponent<KSelectable>().RemoveStatusItem(this.workerStatusItem);
      this.ShowProgressBar(false);
    }
  }

  protected override void OnCompleteWork(Worker worker)
  {
    Storage component1 = this.GetComponent<Storage>();
    IUserControlledCapacity component2 = this.GetComponent<IUserControlledCapacity>();
    float amount = Mathf.Max(0.0f, component2.AmountStored - component2.UserMaxCapacity);
    List<GameObject> gameObjectList = new List<GameObject>((IEnumerable<GameObject>) component1.items);
    for (int index = 0; index < gameObjectList.Count; ++index)
    {
      Pickupable component3 = gameObjectList[index].GetComponent<Pickupable>();
      if ((double) component3.PrimaryElement.Mass <= (double) amount)
      {
        amount -= component3.PrimaryElement.Mass;
        component1.Drop(component3.gameObject);
      }
      else
      {
        component3.Take(amount).transform.SetPosition(this.transform.GetPosition());
        return;
      }
    }
    this.chore = (Chore) null;
  }
}
