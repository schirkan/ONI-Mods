// Decompiled with JetBrains decompiler
// Type: FuelTank
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class FuelTank : Storage, IUserControlledCapacity
{
  private bool isSuspended;
  private MeterController meter;
  [Serialize]
  public float targetFillMass = BUILDINGS.ROCKETRY_MASS_KG.FUEL_TANK_WET_MASS[0];
  [SerializeField]
  private Tag fuelType;
  public float minimumLaunchMass = BUILDINGS.ROCKETRY_MASS_KG.FUEL_TANK_WET_MASS[0];
  private static readonly EventSystem.IntraObjectHandler<FuelTank> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<FuelTank>((System.Action<FuelTank, object>) ((component, data) => component.OnCopySettings(data)));

  public bool IsSuspended => this.isSuspended;

  public float UserMaxCapacity
  {
    get => this.targetFillMass;
    set
    {
      this.targetFillMass = value;
      this.capacityKg = this.targetFillMass;
      ConduitConsumer component1 = this.GetComponent<ConduitConsumer>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        component1.capacityKG = this.targetFillMass;
      ManualDeliveryKG component2 = this.GetComponent<ManualDeliveryKG>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        component2.capacity = component2.refillMass = this.targetFillMass;
      this.Trigger(-945020481, (object) this);
    }
  }

  public float MinCapacity => 0.0f;

  public float MaxCapacity => 900f;

  public float AmountStored => this.MassStored();

  public bool WholeValues => false;

  public LocString CapacityUnits => GameUtil.GetCurrentMassUnit();

  public Tag FuelType
  {
    get => this.fuelType;
    set
    {
      this.fuelType = value;
      if (this.storageFilters == null)
        this.storageFilters = new List<Tag>();
      this.storageFilters.Add(this.fuelType);
      ManualDeliveryKG component = this.GetComponent<ManualDeliveryKG>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      component.requestedItemTag = this.fuelType;
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<FuelTank>(-905833192, FuelTank.OnCopySettingsDelegate);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.GetComponent<KBatchedAnimController>().Play((HashedString) "grounded", KAnim.PlayMode.Loop);
    this.gameObject.Subscribe(1366341636, new System.Action<object>(this.OnReturn));
    this.UserMaxCapacity = this.UserMaxCapacity;
    this.meter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[4]
    {
      "meter_target",
      "meter_fill",
      "meter_frame",
      "meter_OL"
    });
    this.meter.gameObject.GetComponent<KBatchedAnimTracker>().matchParentOffset = true;
    this.Subscribe(-1697596308, (System.Action<object>) (data => this.meter.SetPositionPercent(this.MassStored() / this.capacityKg)));
  }

  public void FillTank()
  {
    RocketEngine rocketEngine = (RocketEngine) null;
    foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.GetComponent<AttachableBuilding>()))
    {
      rocketEngine = gameObject.GetComponent<RocketEngine>();
      if ((UnityEngine.Object) rocketEngine != (UnityEngine.Object) null)
      {
        if (rocketEngine.mainEngine)
          break;
      }
    }
    if ((UnityEngine.Object) rocketEngine != (UnityEngine.Object) null)
      this.AddLiquid(ElementLoader.GetElementID(rocketEngine.fuelTag), this.targetFillMass - this.MassStored(), ElementLoader.GetElement(rocketEngine.fuelTag).defaultValues.temperature, (byte) 0, 0);
    else
      Debug.LogWarning((object) "Fuel tank couldn't find rocket engine");
  }

  private void OnReturn(object data)
  {
    for (int index = this.items.Count - 1; index >= 0; --index)
      Util.KDestroyGameObject(this.items[index]);
    this.items.Clear();
  }

  private void OnCopySettings(object data)
  {
    FuelTank component = ((GameObject) data).GetComponent<FuelTank>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.UserMaxCapacity = component.UserMaxCapacity;
  }
}
