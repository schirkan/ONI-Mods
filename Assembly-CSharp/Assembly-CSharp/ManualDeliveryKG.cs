// Decompiled with JetBrains decompiler
// Type: ManualDeliveryKG
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ManualDeliveryKG")]
public class ManualDeliveryKG : KMonoBehaviour, ISim1000ms
{
  [MyCmpGet]
  private Operational operational;
  [SerializeField]
  private Storage storage;
  [SerializeField]
  public Tag requestedItemTag;
  [SerializeField]
  public float capacity = 100f;
  [SerializeField]
  public float refillMass = 10f;
  [SerializeField]
  public float minimumMass = 10f;
  [SerializeField]
  public FetchOrder2.OperationalRequirement operationalRequirement;
  [SerializeField]
  public bool allowPause;
  [SerializeField]
  private bool paused;
  [SerializeField]
  public HashedString choreTypeIDHash;
  [Serialize]
  private bool userPaused;
  public bool ShowStatusItem = true;
  private FetchList2 fetchList;
  private int onStorageChangeSubscription = -1;
  private static readonly EventSystem.IntraObjectHandler<ManualDeliveryKG> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<ManualDeliveryKG>((System.Action<ManualDeliveryKG, object>) ((component, data) => component.OnRefreshUserMenu(data)));
  private static readonly EventSystem.IntraObjectHandler<ManualDeliveryKG> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<ManualDeliveryKG>((System.Action<ManualDeliveryKG, object>) ((component, data) => component.OnOperationalChanged(data)));

  public float Capacity => this.capacity;

  public Tag RequestedItemTag
  {
    get => this.requestedItemTag;
    set
    {
      this.requestedItemTag = value;
      this.AbortDelivery("Requested Item Tag Changed");
    }
  }

  public Storage DebugStorage => this.storage;

  public FetchList2 DebugFetchList => this.fetchList;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    DebugUtil.Assert(this.choreTypeIDHash.IsValid, "ManualDeliveryKG Must have a valid chore type specified!", this.name);
    if (this.allowPause)
    {
      this.Subscribe<ManualDeliveryKG>(493375141, ManualDeliveryKG.OnRefreshUserMenuDelegate);
      this.Subscribe<ManualDeliveryKG>(-111137758, ManualDeliveryKG.OnRefreshUserMenuDelegate);
    }
    this.Subscribe<ManualDeliveryKG>(-592767678, ManualDeliveryKG.OnOperationalChangedDelegate);
    if ((UnityEngine.Object) this.storage != (UnityEngine.Object) null)
      this.SetStorage(this.storage);
    Prioritizable.AddRef(this.gameObject);
    if (!this.userPaused || !this.allowPause)
      return;
    this.OnPause();
  }

  protected override void OnCleanUp()
  {
    this.AbortDelivery("ManualDeliverKG destroyed");
    Prioritizable.RemoveRef(this.gameObject);
    base.OnCleanUp();
  }

  public void SetStorage(Storage storage)
  {
    if ((UnityEngine.Object) this.storage != (UnityEngine.Object) null)
    {
      this.storage.Unsubscribe(this.onStorageChangeSubscription);
      this.onStorageChangeSubscription = -1;
    }
    this.AbortDelivery("storage pointer changed");
    this.storage = storage;
    if (!((UnityEngine.Object) this.storage != (UnityEngine.Object) null) || !this.isSpawned)
      return;
    Debug.Assert(this.onStorageChangeSubscription == -1);
    this.onStorageChangeSubscription = this.storage.Subscribe(-1697596308, (System.Action<object>) (eventData => this.OnStorageChanged(this.storage)));
  }

  public void Pause(bool pause, string reason)
  {
    if (this.paused == pause)
      return;
    this.paused = pause;
    if (!pause)
      return;
    this.AbortDelivery(reason);
  }

  public void Sim1000ms(float dt) => this.UpdateDeliveryState();

  [ContextMenu("UpdateDeliveryState")]
  public void UpdateDeliveryState()
  {
    if (!this.requestedItemTag.IsValid || (UnityEngine.Object) this.storage == (UnityEngine.Object) null)
      return;
    this.UpdateFetchList();
  }

  public void RequestDelivery()
  {
    if (this.fetchList != null)
      return;
    float massAvailable = this.storage.GetMassAvailable(this.requestedItemTag);
    if ((double) massAvailable >= (double) this.capacity)
      return;
    float b = this.capacity - massAvailable;
    float amount = Mathf.Max(PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT, b);
    this.fetchList = new FetchList2(this.storage, Db.Get().ChoreTypes.GetByHash(this.choreTypeIDHash));
    this.fetchList.ShowStatusItem = this.ShowStatusItem;
    this.fetchList.MinimumAmount[this.requestedItemTag] = Mathf.Max(PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT, this.minimumMass);
    this.fetchList.Add(new Tag[1]{ this.requestedItemTag }, amount: amount);
    this.fetchList.Submit((System.Action) null, false);
  }

  private void UpdateFetchList()
  {
    if (this.paused)
      return;
    if (this.fetchList != null && this.fetchList.IsComplete)
      this.fetchList = (FetchList2) null;
    if (!this.OperationalRequirementsMet())
    {
      if (this.fetchList == null)
        return;
      this.fetchList.Cancel("Operational requirements");
      this.fetchList = (FetchList2) null;
    }
    else
    {
      if (this.fetchList != null || (double) this.storage.GetMassAvailable(this.requestedItemTag) >= (double) this.refillMass)
        return;
      this.RequestDelivery();
    }
  }

  private bool OperationalRequirementsMet()
  {
    if ((bool) (UnityEngine.Object) this.operational)
    {
      if (this.operationalRequirement == FetchOrder2.OperationalRequirement.Operational)
        return this.operational.IsOperational;
      if (this.operationalRequirement == FetchOrder2.OperationalRequirement.Functional)
        return this.operational.IsFunctional;
    }
    return true;
  }

  public void AbortDelivery(string reason)
  {
    if (this.fetchList == null)
      return;
    FetchList2 fetchList = this.fetchList;
    this.fetchList = (FetchList2) null;
    string reason1 = reason;
    fetchList.Cancel(reason1);
  }

  private void OnStorageChanged(Storage storage)
  {
    if (!((UnityEngine.Object) storage == (UnityEngine.Object) this.storage))
      return;
    this.UpdateDeliveryState();
  }

  private void OnPause()
  {
    this.userPaused = true;
    this.Pause(true, "Forbid manual delivery");
  }

  private void OnResume()
  {
    this.userPaused = false;
    this.Pause(false, "Allow manual delivery");
  }

  private void OnRefreshUserMenu(object data)
  {
    if (!this.allowPause)
      return;
    Game.Instance.userMenu.AddButton(this.gameObject, !this.paused ? new KIconButtonMenu.ButtonInfo("action_move_to_storage", (string) UI.USERMENUACTIONS.MANUAL_DELIVERY.NAME, new System.Action(this.OnPause), tooltipText: ((string) UI.USERMENUACTIONS.MANUAL_DELIVERY.TOOLTIP)) : new KIconButtonMenu.ButtonInfo("action_move_to_storage", (string) UI.USERMENUACTIONS.MANUAL_DELIVERY.NAME_OFF, new System.Action(this.OnResume), tooltipText: ((string) UI.USERMENUACTIONS.MANUAL_DELIVERY.TOOLTIP_OFF)));
  }

  private void OnOperationalChanged(object data) => this.UpdateDeliveryState();
}
