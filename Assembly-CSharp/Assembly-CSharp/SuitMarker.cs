// Decompiled with JetBrains decompiler
// Type: SuitMarker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/SuitMarker")]
public class SuitMarker : KMonoBehaviour
{
  [MyCmpGet]
  private Building building;
  private ScenePartitionerEntry partitionerEntry;
  private SuitMarker.SuitMarkerReactable reactable;
  private bool hasAvailableSuit;
  [Serialize]
  private bool onlyTraverseIfUnequipAvailable;
  private Grid.SuitMarker.Flags gridFlags;
  private int cell;
  public Tag[] LockerTags;
  public PathFinder.PotentialPath.Flags PathFlag;
  public KAnimFile interactAnim = Assets.GetAnim((HashedString) "anim_equip_clothing_kanim");
  private static readonly EventSystem.IntraObjectHandler<SuitMarker> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<SuitMarker>((System.Action<SuitMarker, object>) ((component, data) => component.OnRefreshUserMenu(data)));
  private static readonly EventSystem.IntraObjectHandler<SuitMarker> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<SuitMarker>((System.Action<SuitMarker, object>) ((component, data) => component.OnOperationalChanged((bool) data)));
  private static readonly EventSystem.IntraObjectHandler<SuitMarker> OnRotatedDelegate = new EventSystem.IntraObjectHandler<SuitMarker>((System.Action<SuitMarker, object>) ((component, data) => component.isRotated = ((Rotatable) data).IsRotated));

  private bool OnlyTraverseIfUnequipAvailable
  {
    get
    {
      DebugUtil.Assert(this.onlyTraverseIfUnequipAvailable == (uint) (this.gridFlags & Grid.SuitMarker.Flags.OnlyTraverseIfUnequipAvailable) > 0U);
      return this.onlyTraverseIfUnequipAvailable;
    }
    set
    {
      this.onlyTraverseIfUnequipAvailable = value;
      this.UpdateGridFlag(Grid.SuitMarker.Flags.OnlyTraverseIfUnequipAvailable, this.onlyTraverseIfUnequipAvailable);
    }
  }

  private bool isRotated
  {
    get => (uint) (this.gridFlags & Grid.SuitMarker.Flags.Rotated) > 0U;
    set => this.UpdateGridFlag(Grid.SuitMarker.Flags.Rotated, value);
  }

  private bool isOperational
  {
    get => (uint) (this.gridFlags & Grid.SuitMarker.Flags.Operational) > 0U;
    set => this.UpdateGridFlag(Grid.SuitMarker.Flags.Operational, value);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.OnlyTraverseIfUnequipAvailable = this.onlyTraverseIfUnequipAvailable;
    Debug.Assert((UnityEngine.Object) this.interactAnim != (UnityEngine.Object) null, (object) "interactAnim is null");
    this.Subscribe<SuitMarker>(493375141, SuitMarker.OnRefreshUserMenuDelegate);
    this.isOperational = this.GetComponent<Operational>().IsOperational;
    this.Subscribe<SuitMarker>(-592767678, SuitMarker.OnOperationalChangedDelegate);
    this.isRotated = this.GetComponent<Rotatable>().IsRotated;
    this.Subscribe<SuitMarker>(-1643076535, SuitMarker.OnRotatedDelegate);
    this.CreateNewReactable();
    this.cell = Grid.PosToCell((KMonoBehaviour) this);
    Grid.RegisterSuitMarker(this.cell);
    this.GetComponent<KAnimControllerBase>().Play((HashedString) "no_suit");
    Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Suits);
    this.RefreshTraverseIfUnequipStatusItem();
    SuitLocker.UpdateSuitMarkerStates(Grid.PosToCell(this.transform.position), this.gameObject);
  }

  private void CreateNewReactable() => this.reactable = new SuitMarker.SuitMarkerReactable(this);

  public void GetAttachedLockers(List<SuitLocker> suit_lockers)
  {
    int num1 = this.isRotated ? 1 : -1;
    int num2 = 1;
    while (true)
    {
      int cell = Grid.OffsetCell(this.cell, num2 * num1, 0);
      GameObject gameObject = Grid.Objects[cell, 1];
      if (!((UnityEngine.Object) gameObject == (UnityEngine.Object) null))
      {
        KPrefabID component1 = gameObject.GetComponent<KPrefabID>();
        if (!((UnityEngine.Object) component1 == (UnityEngine.Object) null))
        {
          if (component1.HasAnyTags(this.LockerTags))
          {
            SuitLocker component2 = gameObject.GetComponent<SuitLocker>();
            if (!((UnityEngine.Object) component2 == (UnityEngine.Object) null))
            {
              if (!suit_lockers.Contains(component2))
                suit_lockers.Add(component2);
            }
            else
              goto label_10;
          }
          else
            goto label_9;
        }
        ++num2;
      }
      else
        break;
    }
    return;
label_9:
    return;
label_10:;
  }

  public static bool DoesTraversalDirectionRequireSuit(
    int source_cell,
    int dest_cell,
    Grid.SuitMarker.Flags flags)
  {
    return Grid.CellColumn(dest_cell) > Grid.CellColumn(source_cell) == ((flags & Grid.SuitMarker.Flags.Rotated) == (Grid.SuitMarker.Flags) 0);
  }

  public bool DoesTraversalDirectionRequireSuit(int source_cell, int dest_cell) => SuitMarker.DoesTraversalDirectionRequireSuit(source_cell, dest_cell, this.gridFlags);

  private void Update()
  {
    ListPool<SuitLocker, SuitMarker>.PooledList pooledList = ListPool<SuitLocker, SuitMarker>.Allocate();
    this.GetAttachedLockers((List<SuitLocker>) pooledList);
    int emptyLockerCount = 0;
    int fullLockerCount = 0;
    KPrefabID kprefabId = (KPrefabID) null;
    foreach (SuitLocker suitLocker in (List<SuitLocker>) pooledList)
    {
      if (suitLocker.CanDropOffSuit())
        ++emptyLockerCount;
      if ((UnityEngine.Object) suitLocker.GetPartiallyChargedOutfit() != (UnityEngine.Object) null)
        ++fullLockerCount;
      if ((UnityEngine.Object) kprefabId == (UnityEngine.Object) null)
        kprefabId = suitLocker.GetStoredOutfit();
    }
    pooledList.Recycle();
    bool flag = (UnityEngine.Object) kprefabId != (UnityEngine.Object) null;
    if (flag != this.hasAvailableSuit)
    {
      this.GetComponent<KAnimControllerBase>().Play((HashedString) (flag ? "off" : "no_suit"));
      this.hasAvailableSuit = flag;
    }
    Grid.UpdateSuitMarker(this.cell, fullLockerCount, emptyLockerCount, this.gridFlags, this.PathFlag);
  }

  private void RefreshTraverseIfUnequipStatusItem()
  {
    if (this.OnlyTraverseIfUnequipAvailable)
    {
      this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.SuitMarkerTraversalOnlyWhenRoomAvailable);
      this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.SuitMarkerTraversalAnytime);
    }
    else
    {
      this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.SuitMarkerTraversalOnlyWhenRoomAvailable);
      this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.SuitMarkerTraversalAnytime);
    }
  }

  private void OnEnableTraverseIfUnequipAvailable()
  {
    this.OnlyTraverseIfUnequipAvailable = true;
    this.RefreshTraverseIfUnequipStatusItem();
  }

  private void OnDisableTraverseIfUnequipAvailable()
  {
    this.OnlyTraverseIfUnequipAvailable = false;
    this.RefreshTraverseIfUnequipStatusItem();
  }

  private void UpdateGridFlag(Grid.SuitMarker.Flags flag, bool state)
  {
    if (state)
      this.gridFlags |= flag;
    else
      this.gridFlags &= ~flag;
  }

  private void OnOperationalChanged(bool isOperational)
  {
    SuitLocker.UpdateSuitMarkerStates(Grid.PosToCell(this.transform.position), this.gameObject);
    this.isOperational = isOperational;
  }

  private void OnRefreshUserMenu(object data) => Game.Instance.userMenu.AddButton(this.gameObject, !this.OnlyTraverseIfUnequipAvailable ? new KIconButtonMenu.ButtonInfo("action_clearance", (string) UI.USERMENUACTIONS.SUIT_MARKER_TRAVERSAL.ONLY_WHEN_ROOM_AVAILABLE.NAME, new System.Action(this.OnEnableTraverseIfUnequipAvailable), tooltipText: ((string) UI.USERMENUACTIONS.SUIT_MARKER_TRAVERSAL.ONLY_WHEN_ROOM_AVAILABLE.TOOLTIP)) : new KIconButtonMenu.ButtonInfo("action_clearance", (string) UI.USERMENUACTIONS.SUIT_MARKER_TRAVERSAL.ALWAYS.NAME, new System.Action(this.OnDisableTraverseIfUnequipAvailable), tooltipText: ((string) UI.USERMENUACTIONS.SUIT_MARKER_TRAVERSAL.ALWAYS.TOOLTIP)));

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    if (this.isSpawned)
      Grid.UnregisterSuitMarker(this.cell);
    if (this.partitionerEntry != null)
    {
      this.partitionerEntry.Release();
      this.partitionerEntry = (ScenePartitionerEntry) null;
    }
    if (this.reactable != null)
      this.reactable.Cleanup();
    SuitLocker.UpdateSuitMarkerStates(Grid.PosToCell(this.transform.position), (GameObject) null);
  }

  private class SuitMarkerReactable : Reactable
  {
    private SuitMarker suitMarker;
    private float startTime;

    public SuitMarkerReactable(SuitMarker suit_marker)
      : base(suit_marker.gameObject, (HashedString) nameof (SuitMarkerReactable), Db.Get().ChoreTypes.SuitMarker, 1, 1)
      => this.suitMarker = suit_marker;

    public override bool InternalCanBegin(
      GameObject new_reactor,
      Navigator.ActiveTransition transition)
    {
      if ((UnityEngine.Object) this.reactor != (UnityEngine.Object) null)
        return false;
      if ((UnityEngine.Object) this.suitMarker == (UnityEngine.Object) null)
      {
        this.Cleanup();
        return false;
      }
      if (!this.suitMarker.isOperational)
        return false;
      int x = (int) transition.navGridTransition.x;
      if (x == 0)
        return false;
      return new_reactor.GetComponent<MinionIdentity>().GetEquipment().IsSlotOccupied(Db.Get().AssignableSlots.Suit) ? (x >= 0 || !this.suitMarker.isRotated) && (x <= 0 || this.suitMarker.isRotated) : (x <= 0 || !this.suitMarker.isRotated) && (x >= 0 || this.suitMarker.isRotated) && Grid.HasSuit(Grid.PosToCell((KMonoBehaviour) this.suitMarker), new_reactor.GetComponent<KPrefabID>().InstanceID);
    }

    protected override void InternalBegin()
    {
      this.startTime = Time.time;
      KBatchedAnimController component1 = this.reactor.GetComponent<KBatchedAnimController>();
      component1.AddAnimOverrides(this.suitMarker.interactAnim, 1f);
      component1.Play((HashedString) "working_pre");
      component1.Queue((HashedString) "working_loop");
      component1.Queue((HashedString) "working_pst");
      if (this.suitMarker.HasTag(GameTags.JetSuitBlocker))
      {
        KBatchedAnimController component2 = this.suitMarker.GetComponent<KBatchedAnimController>();
        component2.Play((HashedString) "working_pre");
        component2.Queue((HashedString) "working_loop");
        component2.Queue((HashedString) "working_pst");
      }
      this.suitMarker.CreateNewReactable();
    }

    public override void Update(float dt)
    {
      Facing facing = (bool) (UnityEngine.Object) this.reactor ? this.reactor.GetComponent<Facing>() : (Facing) null;
      if ((bool) (UnityEngine.Object) facing && (bool) (UnityEngine.Object) this.suitMarker)
        facing.SetFacing(this.suitMarker.GetComponent<Rotatable>().GetOrientation() == Orientation.FlipH);
      if ((double) Time.time - (double) this.startTime <= 2.79999995231628)
        return;
      this.Run();
      this.Cleanup();
    }

    private void Run()
    {
      if ((UnityEngine.Object) this.reactor == (UnityEngine.Object) null || (UnityEngine.Object) this.suitMarker == (UnityEngine.Object) null)
        return;
      GameObject reactor = this.reactor;
      Equipment equipment = reactor.GetComponent<MinionIdentity>().GetEquipment();
      bool flag1 = !equipment.IsSlotOccupied(Db.Get().AssignableSlots.Suit);
      reactor.GetComponent<KBatchedAnimController>().RemoveAnimOverrides(this.suitMarker.interactAnim);
      bool flag2 = false;
      Navigator component = reactor.GetComponent<Navigator>();
      bool flag3 = (UnityEngine.Object) component != (UnityEngine.Object) null && (uint) (component.flags & this.suitMarker.PathFlag) > 0U;
      if (flag1 | flag3)
      {
        ListPool<SuitLocker, SuitMarker>.PooledList pooledList = ListPool<SuitLocker, SuitMarker>.Allocate();
        this.suitMarker.GetAttachedLockers((List<SuitLocker>) pooledList);
        foreach (SuitLocker suitLocker in (List<SuitLocker>) pooledList)
        {
          if ((UnityEngine.Object) suitLocker.GetFullyChargedOutfit() != (UnityEngine.Object) null & flag1)
          {
            suitLocker.EquipTo(equipment);
            flag2 = true;
            break;
          }
          if (!flag1 && suitLocker.CanDropOffSuit())
          {
            suitLocker.UnequipFrom(equipment);
            flag2 = true;
            break;
          }
        }
        if (flag1 && !flag2)
        {
          SuitLocker suitLocker1 = (SuitLocker) null;
          float num = 0.0f;
          foreach (SuitLocker suitLocker2 in (List<SuitLocker>) pooledList)
          {
            if ((double) suitLocker2.GetSuitScore() > (double) num)
            {
              suitLocker1 = suitLocker2;
              num = suitLocker2.GetSuitScore();
            }
          }
          if ((UnityEngine.Object) suitLocker1 != (UnityEngine.Object) null)
          {
            suitLocker1.EquipTo(equipment);
            flag2 = true;
          }
        }
        pooledList.Recycle();
      }
      if (flag2 || flag1)
        return;
      Assignable assignable = equipment.GetAssignable(Db.Get().AssignableSlots.Suit);
      assignable.Unassign();
      Notification notification = new Notification((string) MISC.NOTIFICATIONS.SUIT_DROPPED.NAME, NotificationType.BadMinor, HashedString.Invalid, (Func<List<Notification>, object, string>) ((notificationList, data) => (string) MISC.NOTIFICATIONS.SUIT_DROPPED.TOOLTIP));
      assignable.GetComponent<Notifier>().Add(notification);
    }

    protected override void InternalEnd()
    {
      if (!((UnityEngine.Object) this.reactor != (UnityEngine.Object) null))
        return;
      this.reactor.GetComponent<KBatchedAnimController>().RemoveAnimOverrides(this.suitMarker.interactAnim);
    }

    protected override void InternalCleanup()
    {
    }
  }
}
