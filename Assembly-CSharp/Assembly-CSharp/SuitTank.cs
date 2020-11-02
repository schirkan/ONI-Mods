// Decompiled with JetBrains decompiler
// Type: SuitTank
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/SuitTank")]
public class SuitTank : KMonoBehaviour, IGameObjectEffectDescriptor, OxygenBreather.IGasProvider
{
  [Serialize]
  public string element;
  [Serialize]
  public float amount;
  public float capacity;
  public const float REFILL_PERCENT = 0.25f;
  public bool underwaterSupport;
  private SuitSuffocationMonitor.Instance suitSuffocationMonitor;
  private static readonly EventSystem.IntraObjectHandler<SuitTank> OnEquippedDelegate = new EventSystem.IntraObjectHandler<SuitTank>((System.Action<SuitTank, object>) ((component, data) => component.OnEquipped(data)));
  private static readonly EventSystem.IntraObjectHandler<SuitTank> OnUnequippedDelegate = new EventSystem.IntraObjectHandler<SuitTank>((System.Action<SuitTank, object>) ((component, data) => component.OnUnequipped(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.amount = this.capacity;
    this.Subscribe<SuitTank>(-1617557748, SuitTank.OnEquippedDelegate);
    this.Subscribe<SuitTank>(-170173755, SuitTank.OnUnequippedDelegate);
  }

  public float PercentFull() => this.amount / this.capacity;

  public bool IsEmpty() => (double) this.amount <= 0.0;

  public bool IsFull() => (double) this.PercentFull() >= 1.0;

  public bool NeedsRecharging() => (double) this.PercentFull() < 0.25;

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    if (this.element.ToLower() == "oxygen")
    {
      string str = this.underwaterSupport ? string.Format((string) UI.UISIDESCREENS.FABRICATORSIDESCREEN.EFFECTS.OXYGEN_TANK_UNDERWATER, (object) GameUtil.GetFormattedMass(this.amount)) : string.Format((string) UI.UISIDESCREENS.FABRICATORSIDESCREEN.EFFECTS.OXYGEN_TANK, (object) GameUtil.GetFormattedMass(this.amount));
      descriptorList.Add(new Descriptor(str, str));
    }
    return descriptorList;
  }

  private void OnEquipped(object data)
  {
    Equipment equipment = (Equipment) data;
    NameDisplayScreen.Instance.SetSuitTankDisplay(equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject(), new Func<float>(this.PercentFull), true);
    OxygenBreather component = equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().GetComponent<OxygenBreather>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.SetGasProvider((OxygenBreather.IGasProvider) this);
  }

  private void OnUnequipped(object data)
  {
    Equipment equipment = (Equipment) data;
    if (equipment.destroyed)
      return;
    NameDisplayScreen.Instance.SetSuitTankDisplay(equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject(), new Func<float>(this.PercentFull), false);
    OxygenBreather component = equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().GetComponent<OxygenBreather>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.SetGasProvider((OxygenBreather.IGasProvider) new GasBreatherFromWorldProvider());
  }

  public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
  {
    this.suitSuffocationMonitor = new SuitSuffocationMonitor.Instance((IStateMachineTarget) oxygen_breather, this);
    this.suitSuffocationMonitor.StartSM();
  }

  public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
  {
    this.suitSuffocationMonitor.StopSM("Removed suit tank");
    this.suitSuffocationMonitor = (SuitSuffocationMonitor.Instance) null;
  }

  public bool ConsumeGas(OxygenBreather oxygen_breather, float gas_consumed)
  {
    if (this.IsEmpty())
      return false;
    gas_consumed = Mathf.Min(gas_consumed, this.amount);
    this.amount -= gas_consumed;
    Game.Instance.accumulators.Accumulate(oxygen_breather.O2Accumulator, gas_consumed);
    ReportManager.Instance.ReportValue(ReportManager.ReportType.OxygenCreated, -gas_consumed, oxygen_breather.GetProperName());
    return true;
  }

  public bool ShouldEmitCO2() => !this.GetComponent<KPrefabID>().HasTag(GameTags.AirtightSuit);

  public bool ShouldStoreCO2() => this.GetComponent<KPrefabID>().HasTag(GameTags.AirtightSuit);

  [ContextMenu("SetToRefillAmount")]
  public void SetToRefillAmount() => this.amount = 0.25f * this.capacity;

  [ContextMenu("Empty")]
  public void Empty() => this.amount = 0.0f;
}
