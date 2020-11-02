// Decompiled with JetBrains decompiler
// Type: SuitLocker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SuitLocker : StateMachineComponent<SuitLocker.StatesInstance>
{
  [MyCmpGet]
  private Building building;
  public Tag[] OutfitTags;
  private FetchChore fetchChore;
  [MyCmpAdd]
  public SuitLocker.ReturnSuitWorkable returnSuitWorkable;
  private MeterController meter;
  private SuitLocker.SuitMarkerState suitMarkerState;

  public float OxygenAvailable
  {
    get
    {
      GameObject oxygen = this.GetOxygen();
      float num = 0.0f;
      if ((UnityEngine.Object) oxygen != (UnityEngine.Object) null)
        num = Math.Min(oxygen.GetComponent<PrimaryElement>().Mass / this.GetComponent<ConduitConsumer>().capacityKG, 1f);
      return num;
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.meter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[3]
    {
      "meter_target",
      "meter_arrow",
      "meter_scale"
    });
    SuitLocker.UpdateSuitMarkerStates(Grid.PosToCell(this.transform.position), this.gameObject);
    this.smi.StartSM();
    Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Suits);
  }

  public KPrefabID GetStoredOutfit()
  {
    foreach (GameObject gameObject in this.GetComponent<Storage>().items)
    {
      if (!((UnityEngine.Object) gameObject == (UnityEngine.Object) null))
      {
        KPrefabID component = gameObject.GetComponent<KPrefabID>();
        if (!((UnityEngine.Object) component == (UnityEngine.Object) null) && component.HasAnyTags(this.OutfitTags))
          return component;
      }
    }
    return (KPrefabID) null;
  }

  public float GetSuitScore()
  {
    float num = -1f;
    KPrefabID partiallyChargedOutfit = this.GetPartiallyChargedOutfit();
    if ((bool) (UnityEngine.Object) partiallyChargedOutfit)
    {
      num = partiallyChargedOutfit.GetComponent<SuitTank>().PercentFull();
      JetSuitTank component = partiallyChargedOutfit.GetComponent<JetSuitTank>();
      if ((bool) (UnityEngine.Object) component && (double) component.PercentFull() < (double) num)
        num = component.PercentFull();
    }
    return num;
  }

  public KPrefabID GetPartiallyChargedOutfit()
  {
    KPrefabID storedOutfit = this.GetStoredOutfit();
    if (!(bool) (UnityEngine.Object) storedOutfit)
      return (KPrefabID) null;
    if ((double) storedOutfit.GetComponent<SuitTank>().PercentFull() < (double) TUNING.EQUIPMENT.SUITS.MINIMUM_USABLE_SUIT_CHARGE)
      return (KPrefabID) null;
    JetSuitTank component = storedOutfit.GetComponent<JetSuitTank>();
    return (bool) (UnityEngine.Object) component && (double) component.PercentFull() < (double) TUNING.EQUIPMENT.SUITS.MINIMUM_USABLE_SUIT_CHARGE ? (KPrefabID) null : storedOutfit;
  }

  public KPrefabID GetFullyChargedOutfit()
  {
    KPrefabID storedOutfit = this.GetStoredOutfit();
    if (!(bool) (UnityEngine.Object) storedOutfit)
      return (KPrefabID) null;
    if (!storedOutfit.GetComponent<SuitTank>().IsFull())
      return (KPrefabID) null;
    JetSuitTank component = storedOutfit.GetComponent<JetSuitTank>();
    return (bool) (UnityEngine.Object) component && !component.IsFull() ? (KPrefabID) null : storedOutfit;
  }

  private void CreateFetchChore()
  {
    this.fetchChore = new FetchChore(Db.Get().ChoreTypes.StorageFetch, this.GetComponent<Storage>(), 1f, this.OutfitTags, forbidden_tags: new Tag[1]
    {
      GameTags.Assigned
    }, operational_requirement: FetchOrder2.OperationalRequirement.None);
    this.fetchChore.allowMultifetch = false;
  }

  private void CancelFetchChore()
  {
    if (this.fetchChore == null)
      return;
    this.fetchChore.Cancel("SuitLocker.CancelFetchChore");
    this.fetchChore = (FetchChore) null;
  }

  public bool HasOxygen()
  {
    GameObject oxygen = this.GetOxygen();
    return (UnityEngine.Object) oxygen != (UnityEngine.Object) null && (double) oxygen.GetComponent<PrimaryElement>().Mass > 0.0;
  }

  private void RefreshMeter()
  {
    GameObject oxygen = this.GetOxygen();
    float percent_full = 0.0f;
    if ((UnityEngine.Object) oxygen != (UnityEngine.Object) null)
      percent_full = Math.Min(oxygen.GetComponent<PrimaryElement>().Mass / this.GetComponent<ConduitConsumer>().capacityKG, 1f);
    this.meter.SetPositionPercent(percent_full);
  }

  public bool IsSuitFullyCharged()
  {
    KPrefabID storedOutfit = this.GetStoredOutfit();
    if (!((UnityEngine.Object) storedOutfit != (UnityEngine.Object) null))
      return false;
    SuitTank component1 = storedOutfit.GetComponent<SuitTank>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && (double) component1.PercentFull() < 1.0)
      return false;
    JetSuitTank component2 = storedOutfit.GetComponent<JetSuitTank>();
    return !((UnityEngine.Object) component2 != (UnityEngine.Object) null) || (double) component2.PercentFull() >= 1.0;
  }

  public bool IsOxygenTankFull()
  {
    KPrefabID storedOutfit = this.GetStoredOutfit();
    if (!((UnityEngine.Object) storedOutfit != (UnityEngine.Object) null))
      return false;
    SuitTank component = storedOutfit.GetComponent<SuitTank>();
    return (UnityEngine.Object) component == (UnityEngine.Object) null || (double) component.PercentFull() >= 1.0;
  }

  private void OnRequestOutfit() => this.smi.sm.isWaitingForSuit.Set(true, this.smi);

  private void OnCancelRequest() => this.smi.sm.isWaitingForSuit.Set(false, this.smi);

  public void DropSuit()
  {
    KPrefabID storedOutfit = this.GetStoredOutfit();
    if ((UnityEngine.Object) storedOutfit == (UnityEngine.Object) null)
      return;
    this.GetComponent<Storage>().Drop(storedOutfit.gameObject);
  }

  public void EquipTo(Equipment equipment)
  {
    KPrefabID storedOutfit = this.GetStoredOutfit();
    if ((UnityEngine.Object) storedOutfit == (UnityEngine.Object) null)
      return;
    this.GetComponent<Storage>().Drop(storedOutfit.gameObject);
    storedOutfit.GetComponent<Equippable>().Assign(equipment.GetComponent<IAssignableIdentity>());
    storedOutfit.GetComponent<EquippableWorkable>().CancelChore();
    equipment.Equip(storedOutfit.GetComponent<Equippable>());
    this.returnSuitWorkable.CreateChore();
  }

  public void UnequipFrom(Equipment equipment)
  {
    Assignable assignable = equipment.GetAssignable(Db.Get().AssignableSlots.Suit);
    assignable.Unassign();
    this.GetComponent<Storage>().Store(assignable.gameObject);
  }

  public void ConfigRequestSuit()
  {
    this.smi.sm.isConfigured.Set(true, this.smi);
    this.smi.sm.isWaitingForSuit.Set(true, this.smi);
  }

  public void ConfigNoSuit()
  {
    this.smi.sm.isConfigured.Set(true, this.smi);
    this.smi.sm.isWaitingForSuit.Set(false, this.smi);
  }

  public bool CanDropOffSuit() => this.smi.sm.isConfigured.Get(this.smi) && !this.smi.sm.isWaitingForSuit.Get(this.smi) && (UnityEngine.Object) this.GetStoredOutfit() == (UnityEngine.Object) null;

  private GameObject GetOxygen() => this.GetComponent<Storage>().FindFirst(GameTags.Oxygen);

  private void ChargeSuit(float dt)
  {
    KPrefabID storedOutfit = this.GetStoredOutfit();
    if ((UnityEngine.Object) storedOutfit == (UnityEngine.Object) null)
      return;
    GameObject oxygen = this.GetOxygen();
    if ((UnityEngine.Object) oxygen == (UnityEngine.Object) null)
      return;
    SuitTank component = storedOutfit.GetComponent<SuitTank>();
    float b = Mathf.Min((float) ((double) component.capacity * 15.0 * (double) dt / 600.0), component.capacity - component.amount);
    float num = Mathf.Min(oxygen.GetComponent<PrimaryElement>().Mass, b);
    oxygen.GetComponent<PrimaryElement>().Mass -= num;
    component.amount += num;
  }

  public void SetSuitMarker(SuitMarker suit_marker)
  {
    SuitLocker.SuitMarkerState suitMarkerState = SuitLocker.SuitMarkerState.HasMarker;
    if ((UnityEngine.Object) suit_marker == (UnityEngine.Object) null)
      suitMarkerState = SuitLocker.SuitMarkerState.NoMarker;
    else if ((double) suit_marker.transform.GetPosition().x > (double) this.transform.GetPosition().x && suit_marker.GetComponent<Rotatable>().IsRotated)
      suitMarkerState = SuitLocker.SuitMarkerState.WrongSide;
    else if ((double) suit_marker.transform.GetPosition().x < (double) this.transform.GetPosition().x && !suit_marker.GetComponent<Rotatable>().IsRotated)
      suitMarkerState = SuitLocker.SuitMarkerState.WrongSide;
    else if (!suit_marker.GetComponent<Operational>().IsOperational)
      suitMarkerState = SuitLocker.SuitMarkerState.NotOperational;
    if (suitMarkerState == this.suitMarkerState)
      return;
    this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoSuitMarker);
    this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.SuitMarkerWrongSide);
    switch (suitMarkerState)
    {
      case SuitLocker.SuitMarkerState.NoMarker:
        this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoSuitMarker);
        break;
      case SuitLocker.SuitMarkerState.WrongSide:
        this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.SuitMarkerWrongSide);
        break;
    }
    this.suitMarkerState = suitMarkerState;
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    SuitLocker.UpdateSuitMarkerStates(Grid.PosToCell(this.transform.position), (GameObject) null);
  }

  private static void GatherSuitBuildings(
    int cell,
    int dir,
    List<SuitLocker.SuitLockerEntry> suit_lockers,
    List<SuitLocker.SuitMarkerEntry> suit_markers)
  {
    int x = dir;
    while (true)
    {
      int cell1 = Grid.OffsetCell(cell, x, 0);
      if (!Grid.IsValidCell(cell1) || SuitLocker.GatherSuitBuildingsOnCell(cell1, suit_lockers, suit_markers))
        x += dir;
      else
        break;
    }
  }

  private static bool GatherSuitBuildingsOnCell(
    int cell,
    List<SuitLocker.SuitLockerEntry> suit_lockers,
    List<SuitLocker.SuitMarkerEntry> suit_markers)
  {
    GameObject gameObject = Grid.Objects[cell, 1];
    if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
      return false;
    SuitMarker component1 = gameObject.GetComponent<SuitMarker>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      suit_markers.Add(new SuitLocker.SuitMarkerEntry()
      {
        suitMarker = component1,
        cell = cell
      });
      return true;
    }
    SuitLocker component2 = gameObject.GetComponent<SuitLocker>();
    if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null))
      return false;
    suit_lockers.Add(new SuitLocker.SuitLockerEntry()
    {
      suitLocker = component2,
      cell = cell
    });
    return true;
  }

  private static SuitMarker FindSuitMarker(
    int cell,
    List<SuitLocker.SuitMarkerEntry> suit_markers)
  {
    if (!Grid.IsValidCell(cell))
      return (SuitMarker) null;
    foreach (SuitLocker.SuitMarkerEntry suitMarker in suit_markers)
    {
      if (suitMarker.cell == cell)
        return suitMarker.suitMarker;
    }
    return (SuitMarker) null;
  }

  public static void UpdateSuitMarkerStates(int cell, GameObject self)
  {
    ListPool<SuitLocker.SuitLockerEntry, SuitLocker>.PooledList pooledList1 = ListPool<SuitLocker.SuitLockerEntry, SuitLocker>.Allocate();
    ListPool<SuitLocker.SuitMarkerEntry, SuitLocker>.PooledList pooledList2 = ListPool<SuitLocker.SuitMarkerEntry, SuitLocker>.Allocate();
    if ((UnityEngine.Object) self != (UnityEngine.Object) null)
    {
      SuitLocker component1 = self.GetComponent<SuitLocker>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        pooledList1.Add(new SuitLocker.SuitLockerEntry()
        {
          suitLocker = component1,
          cell = cell
        });
      SuitMarker component2 = self.GetComponent<SuitMarker>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        pooledList2.Add(new SuitLocker.SuitMarkerEntry()
        {
          suitMarker = component2,
          cell = cell
        });
    }
    SuitLocker.GatherSuitBuildings(cell, 1, (List<SuitLocker.SuitLockerEntry>) pooledList1, (List<SuitLocker.SuitMarkerEntry>) pooledList2);
    SuitLocker.GatherSuitBuildings(cell, -1, (List<SuitLocker.SuitLockerEntry>) pooledList1, (List<SuitLocker.SuitMarkerEntry>) pooledList2);
    pooledList1.Sort((IComparer<SuitLocker.SuitLockerEntry>) SuitLocker.SuitLockerEntry.comparer);
    for (int index1 = 0; index1 < pooledList1.Count; ++index1)
    {
      SuitLocker.SuitLockerEntry suitLockerEntry1 = pooledList1[index1];
      SuitLocker.SuitLockerEntry suitLockerEntry2 = suitLockerEntry1;
      ListPool<SuitLocker.SuitLockerEntry, SuitLocker>.PooledList pooledList3 = ListPool<SuitLocker.SuitLockerEntry, SuitLocker>.Allocate();
      pooledList3.Add(suitLockerEntry1);
      for (int index2 = index1 + 1; index2 < pooledList1.Count; ++index2)
      {
        SuitLocker.SuitLockerEntry suitLockerEntry3 = pooledList1[index2];
        if (Grid.CellRight(suitLockerEntry2.cell) == suitLockerEntry3.cell)
        {
          ++index1;
          suitLockerEntry2 = suitLockerEntry3;
          pooledList3.Add(suitLockerEntry3);
        }
        else
          break;
      }
      int cell1 = Grid.CellLeft(suitLockerEntry1.cell);
      int cell2 = Grid.CellRight(suitLockerEntry2.cell);
      ListPool<SuitLocker.SuitMarkerEntry, SuitLocker>.PooledList pooledList4 = pooledList2;
      SuitMarker suitMarker = SuitLocker.FindSuitMarker(cell1, (List<SuitLocker.SuitMarkerEntry>) pooledList4);
      if ((UnityEngine.Object) suitMarker == (UnityEngine.Object) null)
        suitMarker = SuitLocker.FindSuitMarker(cell2, (List<SuitLocker.SuitMarkerEntry>) pooledList2);
      foreach (SuitLocker.SuitLockerEntry suitLockerEntry3 in (List<SuitLocker.SuitLockerEntry>) pooledList3)
        suitLockerEntry3.suitLocker.SetSuitMarker(suitMarker);
      pooledList3.Recycle();
    }
    pooledList1.Recycle();
    pooledList2.Recycle();
  }

  [AddComponentMenu("KMonoBehaviour/Workable/ReturnSuitWorkable")]
  public class ReturnSuitWorkable : Workable
  {
    public static readonly Chore.Precondition DoesSuitNeedRechargingUrgent = new Chore.Precondition()
    {
      id = nameof (DoesSuitNeedRechargingUrgent),
      description = (string) DUPLICANTS.CHORES.PRECONDITIONS.DOES_SUIT_NEED_RECHARGING_URGENT,
      fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
      {
        AssignableSlotInstance slot = context.consumerState.equipment.GetSlot(Db.Get().AssignableSlots.Suit);
        if ((UnityEngine.Object) slot.assignable == (UnityEngine.Object) null)
          return false;
        SuitTank component1 = slot.assignable.GetComponent<SuitTank>();
        if ((UnityEngine.Object) component1 == (UnityEngine.Object) null)
          return false;
        if (component1.NeedsRecharging())
          return true;
        JetSuitTank component2 = slot.assignable.GetComponent<JetSuitTank>();
        return !((UnityEngine.Object) component2 == (UnityEngine.Object) null) && component2.NeedsRecharging();
      })
    };
    public static readonly Chore.Precondition DoesSuitNeedRechargingIdle = new Chore.Precondition()
    {
      id = nameof (DoesSuitNeedRechargingIdle),
      description = (string) DUPLICANTS.CHORES.PRECONDITIONS.DOES_SUIT_NEED_RECHARGING_IDLE,
      fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
      {
        AssignableSlotInstance slot = context.consumerState.equipment.GetSlot(Db.Get().AssignableSlots.Suit);
        return !((UnityEngine.Object) slot.assignable == (UnityEngine.Object) null) && !((UnityEngine.Object) slot.assignable.GetComponent<SuitTank>() == (UnityEngine.Object) null);
      })
    };
    public Chore.Precondition HasSuitMarker;
    public Chore.Precondition SuitTypeMatchesLocker;
    private WorkChore<SuitLocker.ReturnSuitWorkable> urgentChore;
    private WorkChore<SuitLocker.ReturnSuitWorkable> idleChore;

    protected override void OnPrefabInit()
    {
      base.OnPrefabInit();
      this.resetProgressOnStop = true;
      this.workTime = 0.25f;
      this.synchronizeAnims = false;
    }

    public void CreateChore()
    {
      if (this.urgentChore != null)
        return;
      SuitLocker component = this.GetComponent<SuitLocker>();
      this.urgentChore = new WorkChore<SuitLocker.ReturnSuitWorkable>(Db.Get().ChoreTypes.ReturnSuitUrgent, (IStateMachineTarget) this, only_when_operational: false, allow_prioritization: false, priority_class: PriorityScreen.PriorityClass.personalNeeds, add_to_daily_report: false);
      this.urgentChore.AddPrecondition(SuitLocker.ReturnSuitWorkable.DoesSuitNeedRechargingUrgent);
      this.urgentChore.AddPrecondition(this.HasSuitMarker, (object) component);
      this.urgentChore.AddPrecondition(this.SuitTypeMatchesLocker, (object) component);
      this.idleChore = new WorkChore<SuitLocker.ReturnSuitWorkable>(Db.Get().ChoreTypes.ReturnSuitIdle, (IStateMachineTarget) this, only_when_operational: false, allow_prioritization: false, priority_class: PriorityScreen.PriorityClass.idle, add_to_daily_report: false);
      this.idleChore.AddPrecondition(SuitLocker.ReturnSuitWorkable.DoesSuitNeedRechargingIdle);
      this.idleChore.AddPrecondition(this.HasSuitMarker, (object) component);
      this.idleChore.AddPrecondition(this.SuitTypeMatchesLocker, (object) component);
    }

    public void CancelChore()
    {
      if (this.urgentChore != null)
      {
        this.urgentChore.Cancel("ReturnSuitWorkable.CancelChore");
        this.urgentChore = (WorkChore<SuitLocker.ReturnSuitWorkable>) null;
      }
      if (this.idleChore == null)
        return;
      this.idleChore.Cancel("ReturnSuitWorkable.CancelChore");
      this.idleChore = (WorkChore<SuitLocker.ReturnSuitWorkable>) null;
    }

    protected override void OnStartWork(Worker worker) => this.ShowProgressBar(false);

    protected override bool OnWorkTick(Worker worker, float dt) => true;

    protected override void OnCompleteWork(Worker worker)
    {
      Equipment equipment = worker.GetComponent<MinionIdentity>().GetEquipment();
      if (equipment.IsSlotOccupied(Db.Get().AssignableSlots.Suit))
      {
        if (this.GetComponent<SuitLocker>().CanDropOffSuit())
          this.GetComponent<SuitLocker>().UnequipFrom(equipment);
        else
          equipment.GetAssignable(Db.Get().AssignableSlots.Suit).Unassign();
      }
      if (this.urgentChore == null)
        return;
      this.CancelChore();
      this.CreateChore();
    }

    public override HashedString[] GetWorkAnims(Worker worker) => new HashedString[1]
    {
      new HashedString("none")
    };

    public ReturnSuitWorkable()
    {
      Chore.Precondition precondition = new Chore.Precondition();
      precondition.id = "IsValid";
      precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.HAS_SUIT_MARKER;
      precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => ((SuitLocker) data).suitMarkerState == SuitLocker.SuitMarkerState.HasMarker);
      this.HasSuitMarker = precondition;
      precondition = new Chore.Precondition();
      precondition.id = "IsValid";
      precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.HAS_SUIT_MARKER;
      precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
      {
        SuitLocker suitLocker = (SuitLocker) data;
        AssignableSlotInstance slot = context.consumerState.equipment.GetSlot(Db.Get().AssignableSlots.Suit);
        return !((UnityEngine.Object) slot.assignable == (UnityEngine.Object) null) && (UnityEngine.Object) slot.assignable.GetComponent<JetSuitTank>() != (UnityEngine.Object) null == ((UnityEngine.Object) suitLocker.GetComponent<JetSuitLocker>() != (UnityEngine.Object) null);
      });
      this.SuitTypeMatchesLocker = precondition;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }

  public class StatesInstance : GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.GameInstance
  {
    public StatesInstance(SuitLocker suit_locker)
      : base(suit_locker)
    {
    }
  }

  public class States : GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker>
  {
    public SuitLocker.States.EmptyStates empty;
    public SuitLocker.States.ChargingStates charging;
    public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State waitingforsuit;
    public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State suitfullycharged;
    public StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.BoolParameter isWaitingForSuit;
    public StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.BoolParameter isConfigured;
    public StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.BoolParameter hasSuitMarker;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.empty;
      this.serializable = true;
      this.root.Update("RefreshMeter", (System.Action<SuitLocker.StatesInstance, float>) ((smi, dt) => smi.master.RefreshMeter()), UpdateRate.RENDER_200ms);
      this.empty.DefaultState(this.empty.notconfigured).EventTransition(GameHashes.OnStorageChange, (GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State) this.charging, (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.Transition.ConditionCallback) (smi => (UnityEngine.Object) smi.master.GetStoredOutfit() != (UnityEngine.Object) null)).ParamTransition<bool>((StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.Parameter<bool>) this.isWaitingForSuit, this.waitingforsuit, GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.IsTrue).Enter("CreateReturnSuitChore", (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State.Callback) (smi => smi.master.returnSuitWorkable.CreateChore())).RefreshUserMenuOnEnter().Exit("CancelReturnSuitChore", (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State.Callback) (smi => smi.master.returnSuitWorkable.CancelChore())).PlayAnim("no_suit_pre").QueueAnim("no_suit");
      this.empty.notconfigured.ParamTransition<bool>((StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.Parameter<bool>) this.isConfigured, this.empty.configured, GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.IsTrue).ToggleStatusItem((string) BUILDING.STATUSITEMS.SUIT_LOCKER_NEEDS_CONFIGURATION.NAME, (string) BUILDING.STATUSITEMS.SUIT_LOCKER_NEEDS_CONFIGURATION.TOOLTIP, "status_item_no_filter_set", StatusItem.IconType.Custom, NotificationType.BadMinor, category: Db.Get().StatusItemCategories.Main);
      this.empty.configured.RefreshUserMenuOnEnter().ToggleStatusItem((string) BUILDING.STATUSITEMS.SUIT_LOCKER.READY.NAME, (string) BUILDING.STATUSITEMS.SUIT_LOCKER.READY.TOOLTIP, category: Db.Get().StatusItemCategories.Main);
      this.waitingforsuit.EventTransition(GameHashes.OnStorageChange, (GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State) this.charging, (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.Transition.ConditionCallback) (smi => (UnityEngine.Object) smi.master.GetStoredOutfit() != (UnityEngine.Object) null)).Enter("CreateFetchChore", (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State.Callback) (smi => smi.master.CreateFetchChore())).ParamTransition<bool>((StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.Parameter<bool>) this.isWaitingForSuit, (GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State) this.empty, GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.IsFalse).RefreshUserMenuOnEnter().PlayAnim("no_suit_pst").QueueAnim("awaiting_suit").Exit("ClearIsWaitingForSuit", (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State.Callback) (smi => this.isWaitingForSuit.Set(false, smi))).Exit("CancelFetchChore", (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State.Callback) (smi => smi.master.CancelFetchChore())).ToggleStatusItem((string) BUILDING.STATUSITEMS.SUIT_LOCKER.SUIT_REQUESTED.NAME, (string) BUILDING.STATUSITEMS.SUIT_LOCKER.SUIT_REQUESTED.TOOLTIP, category: Db.Get().StatusItemCategories.Main);
      this.charging.DefaultState(this.charging.pre).RefreshUserMenuOnEnter().EventTransition(GameHashes.OnStorageChange, (GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State) this.empty, (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.Transition.ConditionCallback) (smi => (UnityEngine.Object) smi.master.GetStoredOutfit() == (UnityEngine.Object) null));
      this.charging.pre.Enter((StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State.Callback) (smi =>
      {
        if (smi.master.IsSuitFullyCharged())
        {
          smi.GoTo((StateMachine.BaseState) this.suitfullycharged);
        }
        else
        {
          smi.GetComponent<KBatchedAnimController>().Play((HashedString) "no_suit_pst");
          smi.GetComponent<KBatchedAnimController>().Queue((HashedString) "charging_pre");
        }
      })).OnAnimQueueComplete(this.charging.operational);
      this.charging.operational.TagTransition(GameTags.Operational, this.charging.notoperational, true).Transition(this.charging.nooxygen, (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.Transition.ConditionCallback) (smi => !smi.master.HasOxygen())).PlayAnim("charging_loop", KAnim.PlayMode.Loop).Enter("SetActive", (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State.Callback) (smi => smi.master.GetComponent<Operational>().SetActive(true))).Transition(this.charging.pst, (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.Transition.ConditionCallback) (smi => smi.master.IsSuitFullyCharged())).Update("ChargeSuit", (System.Action<SuitLocker.StatesInstance, float>) ((smi, dt) => smi.master.ChargeSuit(dt))).Exit("ClearActive", (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State.Callback) (smi => smi.master.GetComponent<Operational>().SetActive(false))).ToggleStatusItem((string) BUILDING.STATUSITEMS.SUIT_LOCKER.CHARGING.NAME, (string) BUILDING.STATUSITEMS.SUIT_LOCKER.CHARGING.TOOLTIP, category: Db.Get().StatusItemCategories.Main);
      this.charging.nooxygen.TagTransition(GameTags.Operational, this.charging.notoperational, true).Transition(this.charging.operational, (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.Transition.ConditionCallback) (smi => smi.master.HasOxygen())).Transition(this.charging.pst, (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.Transition.ConditionCallback) (smi => smi.master.IsSuitFullyCharged())).PlayAnim("no_o2_loop", KAnim.PlayMode.Loop).ToggleStatusItem((string) BUILDING.STATUSITEMS.SUIT_LOCKER.NO_OXYGEN.NAME, (string) BUILDING.STATUSITEMS.SUIT_LOCKER.NO_OXYGEN.TOOLTIP, "status_item_suit_locker_no_oxygen", StatusItem.IconType.Custom, NotificationType.BadMinor, category: Db.Get().StatusItemCategories.Main);
      this.charging.notoperational.TagTransition(GameTags.Operational, this.charging.operational).PlayAnim("not_charging_loop", KAnim.PlayMode.Loop).Transition(this.charging.pst, (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.Transition.ConditionCallback) (smi => smi.master.IsSuitFullyCharged())).ToggleStatusItem((string) BUILDING.STATUSITEMS.SUIT_LOCKER.NOT_OPERATIONAL.NAME, (string) BUILDING.STATUSITEMS.SUIT_LOCKER.NOT_OPERATIONAL.TOOLTIP, category: Db.Get().StatusItemCategories.Main);
      this.charging.pst.PlayAnim("charging_pst").OnAnimQueueComplete(this.suitfullycharged);
      this.suitfullycharged.EventTransition(GameHashes.OnStorageChange, (GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State) this.empty, (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.Transition.ConditionCallback) (smi => (UnityEngine.Object) smi.master.GetStoredOutfit() == (UnityEngine.Object) null)).PlayAnim("has_suit").RefreshUserMenuOnEnter().ToggleStatusItem((string) BUILDING.STATUSITEMS.SUIT_LOCKER.FULLY_CHARGED.NAME, (string) BUILDING.STATUSITEMS.SUIT_LOCKER.FULLY_CHARGED.TOOLTIP, category: Db.Get().StatusItemCategories.Main);
    }

    public class ChargingStates : GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State
    {
      public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State pre;
      public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State pst;
      public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State operational;
      public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State nooxygen;
      public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State notoperational;
    }

    public class EmptyStates : GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State
    {
      public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State configured;
      public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State notconfigured;
    }
  }

  private enum SuitMarkerState
  {
    HasMarker,
    NoMarker,
    WrongSide,
    NotOperational,
  }

  private struct SuitLockerEntry
  {
    public SuitLocker suitLocker;
    public int cell;
    public static SuitLocker.SuitLockerEntry.Comparer comparer = new SuitLocker.SuitLockerEntry.Comparer();

    public class Comparer : IComparer<SuitLocker.SuitLockerEntry>
    {
      public int Compare(SuitLocker.SuitLockerEntry a, SuitLocker.SuitLockerEntry b) => a.cell - b.cell;
    }
  }

  private struct SuitMarkerEntry
  {
    public SuitMarker suitMarker;
    public int cell;
  }
}
