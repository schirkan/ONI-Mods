﻿// Decompiled with JetBrains decompiler
// Type: Compostable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/Compostable")]
public class Compostable : KMonoBehaviour
{
  [SerializeField]
  public bool isMarkedForCompost;
  public GameObject originalPrefab;
  public GameObject compostPrefab;
  private static readonly EventSystem.IntraObjectHandler<Compostable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Compostable>((System.Action<Compostable, object>) ((component, data) => component.OnRefreshUserMenu(data)));
  private static readonly EventSystem.IntraObjectHandler<Compostable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Compostable>((System.Action<Compostable, object>) ((component, data) => component.OnStore(data)));

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.isMarkedForCompost = this.GetComponent<KPrefabID>().HasTag(GameTags.Compostable);
    if (this.isMarkedForCompost)
      this.MarkForCompost();
    this.Subscribe<Compostable>(493375141, Compostable.OnRefreshUserMenuDelegate);
    this.Subscribe<Compostable>(856640610, Compostable.OnStoreDelegate);
  }

  private void MarkForCompost(bool force = false)
  {
    this.RefreshStatusItem();
    Storage storage = this.GetComponent<Pickupable>().storage;
    if (!((UnityEngine.Object) storage != (UnityEngine.Object) null))
      return;
    storage.Drop(this.gameObject);
  }

  private void OnToggleCompost()
  {
    if (!this.isMarkedForCompost)
    {
      Pickupable component = this.GetComponent<Pickupable>();
      if ((UnityEngine.Object) component.storage != (UnityEngine.Object) null)
        component.storage.Drop(this.gameObject);
      Pickupable pickupable = EntitySplitter.Split(component, component.TotalAmount, this.compostPrefab);
      if (!((UnityEngine.Object) pickupable != (UnityEngine.Object) null))
        return;
      SelectTool.Instance.SelectNextFrame(pickupable.GetComponent<KSelectable>(), true);
    }
    else
    {
      Pickupable component = this.GetComponent<Pickupable>();
      Pickupable pickupable = EntitySplitter.Split(component, component.TotalAmount, this.originalPrefab);
      SelectTool.Instance.SelectNextFrame(pickupable.GetComponent<KSelectable>(), true);
    }
  }

  private void RefreshStatusItem()
  {
    KSelectable component = this.GetComponent<KSelectable>();
    component.RemoveStatusItem(Db.Get().MiscStatusItems.MarkedForCompost);
    component.RemoveStatusItem(Db.Get().MiscStatusItems.MarkedForCompostInStorage);
    if (!this.isMarkedForCompost)
      return;
    if ((UnityEngine.Object) this.GetComponent<Pickupable>() != (UnityEngine.Object) null && (UnityEngine.Object) this.GetComponent<Pickupable>().storage == (UnityEngine.Object) null)
      component.AddStatusItem(Db.Get().MiscStatusItems.MarkedForCompost);
    else
      component.AddStatusItem(Db.Get().MiscStatusItems.MarkedForCompostInStorage);
  }

  private void OnStore(object data) => this.RefreshStatusItem();

  private void OnRefreshUserMenu(object data) => Game.Instance.userMenu.AddButton(this.gameObject, this.isMarkedForCompost ? new KIconButtonMenu.ButtonInfo("action_compost", (string) UI.USERMENUACTIONS.COMPOST.NAME_OFF, new System.Action(this.OnToggleCompost), tooltipText: ((string) UI.USERMENUACTIONS.COMPOST.TOOLTIP_OFF)) : new KIconButtonMenu.ButtonInfo("action_compost", (string) UI.USERMENUACTIONS.COMPOST.NAME, new System.Action(this.OnToggleCompost), tooltipText: ((string) UI.USERMENUACTIONS.COMPOST.TOOLTIP)));
}
