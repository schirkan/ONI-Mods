// Decompiled with JetBrains decompiler
// Type: Tinkerable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using TUNING;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/Tinkerable")]
public class Tinkerable : Workable
{
  private Chore chore;
  [MyCmpGet]
  private Storage storage;
  [MyCmpGet]
  private Effects effects;
  [MyCmpGet]
  private RoomTracker roomTracker;
  public Tag tinkerMaterialTag;
  public float tinkerMaterialAmount;
  public string addedEffect;
  public HashedString choreTypeTinker = Db.Get().ChoreTypes.PowerTinker.IdHash;
  public HashedString choreTypeFetch = Db.Get().ChoreTypes.PowerFetch.IdHash;
  private static readonly EventSystem.IntraObjectHandler<Tinkerable> OnEffectRemovedDelegate = new EventSystem.IntraObjectHandler<Tinkerable>((System.Action<Tinkerable, object>) ((component, data) => component.OnEffectRemoved(data)));
  private static readonly EventSystem.IntraObjectHandler<Tinkerable> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<Tinkerable>((System.Action<Tinkerable, object>) ((component, data) => component.OnStorageChange(data)));
  private static readonly EventSystem.IntraObjectHandler<Tinkerable> OnUpdateRoomDelegate = new EventSystem.IntraObjectHandler<Tinkerable>((System.Action<Tinkerable, object>) ((component, data) => component.OnUpdateRoom(data)));
  private static readonly EventSystem.IntraObjectHandler<Tinkerable> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<Tinkerable>((System.Action<Tinkerable, object>) ((component, data) => component.OnOperationalChanged(data)));
  private SchedulerHandle updateHandle;
  private bool hasReservedMaterial;

  public static Tinkerable MakePowerTinkerable(GameObject prefab)
  {
    RoomTracker roomTracker = prefab.AddOrGet<RoomTracker>();
    roomTracker.requiredRoomType = Db.Get().RoomTypes.PowerPlant.Id;
    roomTracker.requirement = RoomTracker.Requirement.TrackingOnly;
    Tinkerable tinkerable = prefab.AddOrGet<Tinkerable>();
    tinkerable.tinkerMaterialTag = PowerControlStationConfig.TINKER_TOOLS;
    tinkerable.tinkerMaterialAmount = 1f;
    tinkerable.addedEffect = "PowerTinker";
    tinkerable.requiredSkillPerk = PowerControlStationConfig.ROLE_PERK;
    tinkerable.SetWorkTime(180f);
    tinkerable.workerStatusItem = Db.Get().DuplicantStatusItems.Tinkering;
    tinkerable.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
    tinkerable.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
    tinkerable.choreTypeTinker = Db.Get().ChoreTypes.PowerTinker.IdHash;
    tinkerable.choreTypeFetch = Db.Get().ChoreTypes.PowerFetch.IdHash;
    tinkerable.multitoolContext = (HashedString) "powertinker";
    tinkerable.multitoolHitEffectTag = (Tag) "fx_powertinker_splash";
    tinkerable.shouldShowSkillPerkStatusItem = false;
    prefab.AddOrGet<Storage>();
    prefab.AddOrGet<Effects>();
    prefab.GetComponent<KPrefabID>().prefabInitFn += (KPrefabID.PrefabFn) (inst => inst.GetComponent<Tinkerable>().SetOffsetTable(OffsetGroups.InvertedStandardTable));
    return tinkerable;
  }

  public static Tinkerable MakeFarmTinkerable(GameObject prefab)
  {
    RoomTracker roomTracker = prefab.AddOrGet<RoomTracker>();
    roomTracker.requiredRoomType = Db.Get().RoomTypes.Farm.Id;
    roomTracker.requirement = RoomTracker.Requirement.TrackingOnly;
    Tinkerable tinkerable = prefab.AddOrGet<Tinkerable>();
    tinkerable.tinkerMaterialTag = FarmStationConfig.TINKER_TOOLS;
    tinkerable.tinkerMaterialAmount = 1f;
    tinkerable.addedEffect = "FarmTinker";
    tinkerable.requiredSkillPerk = Db.Get().SkillPerks.CanFarmTinker.Id;
    tinkerable.workerStatusItem = Db.Get().DuplicantStatusItems.Tinkering;
    tinkerable.SetWorkTime(15f);
    tinkerable.attributeConverter = Db.Get().AttributeConverters.PlantTendSpeed;
    tinkerable.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
    tinkerable.choreTypeTinker = Db.Get().ChoreTypes.CropTend.IdHash;
    tinkerable.choreTypeFetch = Db.Get().ChoreTypes.FarmFetch.IdHash;
    tinkerable.multitoolContext = (HashedString) "tend";
    tinkerable.multitoolHitEffectTag = (Tag) "fx_tend_splash";
    tinkerable.shouldShowSkillPerkStatusItem = false;
    prefab.AddOrGet<Storage>();
    prefab.AddOrGet<Effects>();
    prefab.GetComponent<KPrefabID>().prefabInitFn += (KPrefabID.PrefabFn) (inst => inst.GetComponent<Tinkerable>().SetOffsetTable(OffsetGroups.InvertedStandardTable));
    return tinkerable;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_use_machine_kanim")
    };
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Tinkering;
    this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
    this.faceTargetWhenWorking = true;
    this.synchronizeAnims = false;
    this.Subscribe<Tinkerable>(-1157678353, Tinkerable.OnEffectRemovedDelegate);
    this.Subscribe<Tinkerable>(-1697596308, Tinkerable.OnStorageChangeDelegate);
    this.Subscribe<Tinkerable>(144050788, Tinkerable.OnUpdateRoomDelegate);
    this.Subscribe<Tinkerable>(-592767678, Tinkerable.OnOperationalChangedDelegate);
  }

  protected override void OnCleanUp()
  {
    this.UpdateMaterialReservation(false);
    if (this.updateHandle.IsValid)
      this.updateHandle.ClearScheduler();
    base.OnCleanUp();
  }

  private void OnOperationalChanged(object data) => this.QueueUpdateChore();

  private void OnEffectRemoved(object data) => this.QueueUpdateChore();

  private void OnUpdateRoom(object data) => this.QueueUpdateChore();

  private void OnStorageChange(object data)
  {
    if (!((GameObject) data).HasTag(this.tinkerMaterialTag))
      return;
    this.QueueUpdateChore();
  }

  private void QueueUpdateChore()
  {
    if (this.updateHandle.IsValid)
      this.updateHandle.ClearScheduler();
    this.updateHandle = GameScheduler.Instance.Schedule("UpdateTinkerChore", 1.2f, new System.Action<object>(this.UpdateChoreCallback), (object) null, (SchedulerGroup) null);
  }

  private void UpdateChoreCallback(object obj) => this.UpdateChore();

  private void UpdateChore()
  {
    Operational component = this.GetComponent<Operational>();
    bool flag1 = (UnityEngine.Object) component == (UnityEngine.Object) null || component.IsFunctional;
    int num = this.HasEffect() ? 1 : 0;
    bool flag2 = this.RoomHasActiveTinkerstation();
    bool flag3 = num == 0 & flag2 & flag1;
    bool flag4 = num != 0 || !flag2;
    if (this.chore == null & flag3)
    {
      this.UpdateMaterialReservation(true);
      if (this.HasMaterial())
      {
        this.chore = (Chore) new WorkChore<Tinkerable>(Db.Get().ChoreTypes.GetByHash(this.choreTypeTinker), (IStateMachineTarget) this, only_when_operational: false);
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          this.chore.AddPrecondition(ChorePreconditions.instance.IsFunctional, (object) component);
      }
      else
        this.chore = (Chore) new FetchChore(Db.Get().ChoreTypes.GetByHash(this.choreTypeFetch), this.storage, this.tinkerMaterialAmount, new Tag[1]
        {
          this.tinkerMaterialTag
        }, on_complete: new System.Action<Chore>(this.OnFetchComplete), operational_requirement: FetchOrder2.OperationalRequirement.Functional);
      this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, (object) this.requiredSkillPerk);
      if (string.IsNullOrEmpty(this.GetComponent<RoomTracker>().requiredRoomType))
        return;
      this.chore.AddPrecondition(ChorePreconditions.instance.IsInMyRoom, (object) Grid.PosToCell(this.transform.GetPosition()));
    }
    else
    {
      if (!(this.chore != null & flag4))
        return;
      this.UpdateMaterialReservation(false);
      this.chore.Cancel("No longer needed");
      this.chore = (Chore) null;
    }
  }

  private bool RoomHasActiveTinkerstation()
  {
    if (!this.roomTracker.IsInCorrectRoom() || this.roomTracker.room == null)
      return false;
    foreach (KPrefabID building in this.roomTracker.room.buildings)
    {
      if (!((UnityEngine.Object) building == (UnityEngine.Object) null))
      {
        TinkerStation component = building.GetComponent<TinkerStation>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.outputPrefab == this.tinkerMaterialTag && building.GetComponent<Operational>().IsOperational)
          return true;
      }
    }
    return false;
  }

  private void UpdateMaterialReservation(bool shouldReserve)
  {
    if (shouldReserve && !this.hasReservedMaterial)
    {
      MaterialNeeds.Instance.UpdateNeed(this.tinkerMaterialTag, this.tinkerMaterialAmount);
      this.hasReservedMaterial = shouldReserve;
    }
    else
    {
      if (shouldReserve || !this.hasReservedMaterial)
        return;
      MaterialNeeds.Instance.UpdateNeed(this.tinkerMaterialTag, -this.tinkerMaterialAmount);
      this.hasReservedMaterial = shouldReserve;
    }
  }

  private void OnFetchComplete(Chore data)
  {
    this.UpdateMaterialReservation(false);
    this.chore = (Chore) null;
    this.UpdateChore();
  }

  protected override void OnCompleteWork(Worker worker)
  {
    base.OnCompleteWork(worker);
    this.storage.ConsumeIgnoringDisease(this.tinkerMaterialTag, this.tinkerMaterialAmount);
    this.effects.Add(this.addedEffect, true);
    this.UpdateMaterialReservation(false);
    this.chore = (Chore) null;
    this.UpdateChore();
  }

  private bool HasMaterial() => (double) this.storage.GetAmountAvailable(this.tinkerMaterialTag) >= (double) this.tinkerMaterialAmount;

  private bool HasEffect() => this.effects.HasEffect(this.addedEffect);
}
