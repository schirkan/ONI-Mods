// Decompiled with JetBrains decompiler
// Type: JetSuitLocker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

public class JetSuitLocker : StateMachineComponent<JetSuitLocker.StatesInstance>, ISecondaryInput
{
  [MyCmpReq]
  private Building building;
  [MyCmpReq]
  private Storage storage;
  [MyCmpReq]
  private SuitLocker suit_locker;
  [MyCmpReq]
  private KBatchedAnimController anim_controller;
  public const float FUEL_CAPACITY = 100f;
  [SerializeField]
  public ConduitPortInfo portInfo;
  private int secondaryInputCell = -1;
  private FlowUtilityNetwork.NetworkItem flowNetworkItem;
  private ConduitConsumer fuel_consumer;
  private Tag fuel_tag;
  private MeterController o2_meter;
  private MeterController fuel_meter;

  public float FuelAvailable
  {
    get
    {
      GameObject fuel = this.GetFuel();
      float num = 0.0f;
      if ((UnityEngine.Object) fuel != (UnityEngine.Object) null)
        num = Math.Min(fuel.GetComponent<PrimaryElement>().Mass / 100f, 1f);
      return num;
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.fuel_tag = SimHashes.Petroleum.CreateTag();
    this.fuel_consumer = this.gameObject.AddComponent<ConduitConsumer>();
    this.fuel_consumer.conduitType = this.portInfo.conduitType;
    this.fuel_consumer.consumptionRate = 10f;
    this.fuel_consumer.capacityTag = this.fuel_tag;
    this.fuel_consumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
    this.fuel_consumer.forceAlwaysSatisfied = true;
    this.fuel_consumer.capacityKG = 100f;
    this.fuel_consumer.useSecondaryInput = true;
    RequireInputs requireInputs = this.gameObject.AddComponent<RequireInputs>();
    requireInputs.conduitConsumer = this.fuel_consumer;
    requireInputs.SetRequirements(false, true);
    this.secondaryInputCell = Grid.OffsetCell(Grid.PosToCell(this.transform.GetPosition()), this.building.GetRotatedOffset(this.portInfo.offset));
    IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.portInfo.conduitType);
    this.flowNetworkItem = new FlowUtilityNetwork.NetworkItem(this.portInfo.conduitType, Endpoint.Sink, this.secondaryInputCell, this.gameObject);
    int secondaryInputCell = this.secondaryInputCell;
    FlowUtilityNetwork.NetworkItem flowNetworkItem = this.flowNetworkItem;
    networkManager.AddToNetworks(secondaryInputCell, (object) flowNetworkItem, true);
    this.fuel_meter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target_1", "meter_petrol", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Vector3.zero, new string[1]
    {
      "meter_target_1"
    });
    this.o2_meter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target_2", "meter_oxygen", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Vector3.zero, new string[1]
    {
      "meter_target_2"
    });
    this.smi.StartSM();
  }

  protected override void OnCleanUp()
  {
    Conduit.GetNetworkManager(this.portInfo.conduitType).RemoveFromNetworks(this.secondaryInputCell, (object) this.flowNetworkItem, true);
    base.OnCleanUp();
  }

  private GameObject GetFuel() => this.storage.FindFirst(this.fuel_tag);

  public bool IsSuitFullyCharged() => this.suit_locker.IsSuitFullyCharged();

  public KPrefabID GetStoredOutfit() => this.suit_locker.GetStoredOutfit();

  private void FuelSuit(float dt)
  {
    KPrefabID storedOutfit = this.suit_locker.GetStoredOutfit();
    if ((UnityEngine.Object) storedOutfit == (UnityEngine.Object) null)
      return;
    GameObject fuel = this.GetFuel();
    if ((UnityEngine.Object) fuel == (UnityEngine.Object) null)
      return;
    PrimaryElement component1 = fuel.GetComponent<PrimaryElement>();
    if ((UnityEngine.Object) component1 == (UnityEngine.Object) null)
      return;
    JetSuitTank component2 = storedOutfit.GetComponent<JetSuitTank>();
    float b = Mathf.Min((float) (375.0 * (double) dt / 600.0), 25f - component2.amount);
    float num = Mathf.Min(component1.Mass, b);
    component1.Mass -= num;
    component2.amount += num;
  }

  ConduitType ISecondaryInput.GetSecondaryConduitType() => this.portInfo.conduitType;

  CellOffset ISecondaryInput.GetSecondaryConduitOffset() => this.portInfo.offset;

  public bool HasFuel()
  {
    GameObject fuel = this.GetFuel();
    return (UnityEngine.Object) fuel != (UnityEngine.Object) null && (double) fuel.GetComponent<PrimaryElement>().Mass > 0.0;
  }

  private void RefreshMeter()
  {
    this.o2_meter.SetPositionPercent(this.suit_locker.OxygenAvailable);
    this.fuel_meter.SetPositionPercent(this.FuelAvailable);
    this.anim_controller.SetSymbolVisiblity((KAnimHashedString) "oxygen_yes_bloom", this.IsOxygenTankAboveMinimumLevel());
    this.anim_controller.SetSymbolVisiblity((KAnimHashedString) "petrol_yes_bloom", this.IsFuelTankAboveMinimumLevel());
  }

  public bool IsOxygenTankAboveMinimumLevel()
  {
    KPrefabID storedOutfit = this.GetStoredOutfit();
    if (!((UnityEngine.Object) storedOutfit != (UnityEngine.Object) null))
      return false;
    SuitTank component = storedOutfit.GetComponent<SuitTank>();
    return (UnityEngine.Object) component == (UnityEngine.Object) null || (double) component.PercentFull() >= (double) TUNING.EQUIPMENT.SUITS.MINIMUM_USABLE_SUIT_CHARGE;
  }

  public bool IsFuelTankAboveMinimumLevel()
  {
    KPrefabID storedOutfit = this.GetStoredOutfit();
    if (!((UnityEngine.Object) storedOutfit != (UnityEngine.Object) null))
      return false;
    JetSuitTank component = storedOutfit.GetComponent<JetSuitTank>();
    return (UnityEngine.Object) component == (UnityEngine.Object) null || (double) component.PercentFull() >= (double) TUNING.EQUIPMENT.SUITS.MINIMUM_USABLE_SUIT_CHARGE;
  }

  public class States : GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker>
  {
    public GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State empty;
    public JetSuitLocker.States.ChargingStates charging;
    public GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State charged;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.empty;
      this.serializable = true;
      this.root.Update("RefreshMeter", (System.Action<JetSuitLocker.StatesInstance, float>) ((smi, dt) => smi.master.RefreshMeter()), UpdateRate.RENDER_200ms);
      this.empty.EventTransition(GameHashes.OnStorageChange, (GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State) this.charging, (StateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.Transition.ConditionCallback) (smi => (UnityEngine.Object) smi.master.GetStoredOutfit() != (UnityEngine.Object) null));
      this.charging.DefaultState(this.charging.notoperational).EventTransition(GameHashes.OnStorageChange, this.empty, (StateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.Transition.ConditionCallback) (smi => (UnityEngine.Object) smi.master.GetStoredOutfit() == (UnityEngine.Object) null)).Transition(this.charged, (StateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.Transition.ConditionCallback) (smi => smi.master.IsSuitFullyCharged()));
      this.charging.notoperational.TagTransition(GameTags.Operational, this.charging.operational);
      this.charging.operational.TagTransition(GameTags.Operational, this.charging.notoperational, true).Transition(this.charging.nofuel, (StateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.Transition.ConditionCallback) (smi => !smi.master.HasFuel())).Update("FuelSuit", (System.Action<JetSuitLocker.StatesInstance, float>) ((smi, dt) => smi.master.FuelSuit(dt)), UpdateRate.SIM_1000ms);
      this.charging.nofuel.TagTransition(GameTags.Operational, this.charging.notoperational, true).Transition(this.charging.operational, (StateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.Transition.ConditionCallback) (smi => smi.master.HasFuel())).ToggleStatusItem((string) BUILDING.STATUSITEMS.SUIT_LOCKER.NO_FUEL.NAME, (string) BUILDING.STATUSITEMS.SUIT_LOCKER.NO_FUEL.TOOLTIP, "status_item_no_liquid_to_pump", StatusItem.IconType.Custom, NotificationType.BadMinor);
      this.charged.EventTransition(GameHashes.OnStorageChange, this.empty, (StateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.Transition.ConditionCallback) (smi => (UnityEngine.Object) smi.master.GetStoredOutfit() == (UnityEngine.Object) null));
    }

    public class ChargingStates : GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State
    {
      public GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State notoperational;
      public GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State operational;
      public GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State nofuel;
    }
  }

  public class StatesInstance : GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.GameInstance
  {
    public StatesInstance(JetSuitLocker jet_suit_locker)
      : base(jet_suit_locker)
    {
    }
  }
}
