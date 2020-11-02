// Decompiled with JetBrains decompiler
// Type: WiltCondition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/WiltCondition")]
public class WiltCondition : KMonoBehaviour
{
  [MyCmpGet]
  private ReceptacleMonitor rm;
  [Serialize]
  private bool goingToWilt;
  [Serialize]
  private bool wilting;
  private Dictionary<int, bool> WiltConditions = new Dictionary<int, bool>();
  public float WiltDelay = 1f;
  public float RecoveryDelay = 1f;
  private SchedulerHandle wiltSchedulerHandler;
  private SchedulerHandle recoverSchedulerHandler;
  private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetTemperatureFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>((System.Action<WiltCondition, object>) ((component, data) => component.SetCondition(WiltCondition.Condition.Temperature, false)));
  private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetTemperatureTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>((System.Action<WiltCondition, object>) ((component, data) => component.SetCondition(WiltCondition.Condition.Temperature, true)));
  private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetPressureFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>((System.Action<WiltCondition, object>) ((component, data) => component.SetCondition(WiltCondition.Condition.Pressure, false)));
  private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetPressureTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>((System.Action<WiltCondition, object>) ((component, data) => component.SetCondition(WiltCondition.Condition.Pressure, true)));
  private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetAtmosphereElementFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>((System.Action<WiltCondition, object>) ((component, data) => component.SetCondition(WiltCondition.Condition.AtmosphereElement, false)));
  private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetAtmosphereElementTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>((System.Action<WiltCondition, object>) ((component, data) => component.SetCondition(WiltCondition.Condition.AtmosphereElement, true)));
  private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetDrowningFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>((System.Action<WiltCondition, object>) ((component, data) => component.SetCondition(WiltCondition.Condition.Drowning, false)));
  private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetDrowningTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>((System.Action<WiltCondition, object>) ((component, data) => component.SetCondition(WiltCondition.Condition.Drowning, true)));
  private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetDryingOutFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>((System.Action<WiltCondition, object>) ((component, data) => component.SetCondition(WiltCondition.Condition.DryingOut, false)));
  private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetDryingOutTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>((System.Action<WiltCondition, object>) ((component, data) => component.SetCondition(WiltCondition.Condition.DryingOut, true)));
  private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetIrrigationFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>((System.Action<WiltCondition, object>) ((component, data) => component.SetCondition(WiltCondition.Condition.Irrigation, false)));
  private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetIrrigationTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>((System.Action<WiltCondition, object>) ((component, data) => component.SetCondition(WiltCondition.Condition.Irrigation, true)));
  private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetFertilizedFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>((System.Action<WiltCondition, object>) ((component, data) => component.SetCondition(WiltCondition.Condition.Fertilized, false)));
  private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetFertilizedTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>((System.Action<WiltCondition, object>) ((component, data) => component.SetCondition(WiltCondition.Condition.Fertilized, true)));
  private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetIlluminationComfortFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>((System.Action<WiltCondition, object>) ((component, data) => component.SetCondition(WiltCondition.Condition.IlluminationComfort, false)));
  private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetIlluminationComfortTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>((System.Action<WiltCondition, object>) ((component, data) => component.SetCondition(WiltCondition.Condition.IlluminationComfort, true)));
  private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetReceptacleFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>((System.Action<WiltCondition, object>) ((component, data) => component.SetCondition(WiltCondition.Condition.Receptacle, false)));
  private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetReceptacleTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>((System.Action<WiltCondition, object>) ((component, data) => component.SetCondition(WiltCondition.Condition.Receptacle, true)));
  private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetEntombedDelegate = new EventSystem.IntraObjectHandler<WiltCondition>((System.Action<WiltCondition, object>) ((component, data) => component.SetCondition(WiltCondition.Condition.Entombed, !(bool) data)));
  private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetRootHealthDelegate = new EventSystem.IntraObjectHandler<WiltCondition>((System.Action<WiltCondition, object>) ((component, data) => component.SetCondition(WiltCondition.Condition.UnhealthyRoot, (bool) data)));

  public bool IsWilting() => this.wilting;

  public List<WiltCondition.Condition> CurrentWiltSources()
  {
    List<WiltCondition.Condition> conditionList = new List<WiltCondition.Condition>();
    foreach (KeyValuePair<int, bool> wiltCondition in this.WiltConditions)
    {
      if (!wiltCondition.Value)
        conditionList.Add((WiltCondition.Condition) wiltCondition.Key);
    }
    return conditionList;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.WiltConditions.Add(0, true);
    this.WiltConditions.Add(1, true);
    this.WiltConditions.Add(2, true);
    this.WiltConditions.Add(3, true);
    this.WiltConditions.Add(4, true);
    this.WiltConditions.Add(5, true);
    this.WiltConditions.Add(6, true);
    this.WiltConditions.Add(7, true);
    this.WiltConditions.Add(9, true);
    this.WiltConditions.Add(10, true);
    this.WiltConditions.Add(11, true);
    this.Subscribe<WiltCondition>(-107174716, WiltCondition.SetTemperatureFalseDelegate);
    this.Subscribe<WiltCondition>(-1758196852, WiltCondition.SetTemperatureFalseDelegate);
    this.Subscribe<WiltCondition>(-1234705021, WiltCondition.SetTemperatureFalseDelegate);
    this.Subscribe<WiltCondition>(-55477301, WiltCondition.SetTemperatureFalseDelegate);
    this.Subscribe<WiltCondition>(115888613, WiltCondition.SetTemperatureTrueDelegate);
    this.Subscribe<WiltCondition>(-593125877, WiltCondition.SetPressureFalseDelegate);
    this.Subscribe<WiltCondition>(-1175525437, WiltCondition.SetPressureFalseDelegate);
    this.Subscribe<WiltCondition>(-907106982, WiltCondition.SetPressureTrueDelegate);
    this.Subscribe<WiltCondition>(103243573, WiltCondition.SetPressureFalseDelegate);
    this.Subscribe<WiltCondition>(646131325, WiltCondition.SetPressureFalseDelegate);
    this.Subscribe<WiltCondition>(221594799, WiltCondition.SetAtmosphereElementFalseDelegate);
    this.Subscribe<WiltCondition>(777259436, WiltCondition.SetAtmosphereElementTrueDelegate);
    this.Subscribe<WiltCondition>(1949704522, WiltCondition.SetDrowningFalseDelegate);
    this.Subscribe<WiltCondition>(99949694, WiltCondition.SetDrowningTrueDelegate);
    this.Subscribe<WiltCondition>(-2057657673, WiltCondition.SetDryingOutFalseDelegate);
    this.Subscribe<WiltCondition>(1555379996, WiltCondition.SetDryingOutTrueDelegate);
    this.Subscribe<WiltCondition>(-370379773, WiltCondition.SetIrrigationFalseDelegate);
    this.Subscribe<WiltCondition>(207387507, WiltCondition.SetIrrigationTrueDelegate);
    this.Subscribe<WiltCondition>(-1073674739, WiltCondition.SetFertilizedFalseDelegate);
    this.Subscribe<WiltCondition>(-1396791468, WiltCondition.SetFertilizedTrueDelegate);
    this.Subscribe<WiltCondition>(1113102781, WiltCondition.SetIlluminationComfortTrueDelegate);
    this.Subscribe<WiltCondition>(1387626797, WiltCondition.SetIlluminationComfortFalseDelegate);
    this.Subscribe<WiltCondition>(1628751838, WiltCondition.SetReceptacleTrueDelegate);
    this.Subscribe<WiltCondition>(960378201, WiltCondition.SetReceptacleFalseDelegate);
    this.Subscribe<WiltCondition>(-1089732772, WiltCondition.SetEntombedDelegate);
    this.Subscribe<WiltCondition>(912965142, WiltCondition.SetRootHealthDelegate);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.CheckShouldWilt();
    if (this.wilting)
    {
      this.DoWilt();
      if (this.goingToWilt)
        return;
      this.goingToWilt = true;
      this.Recover();
    }
    else
    {
      this.DoRecover();
      if (!this.goingToWilt)
        return;
      this.goingToWilt = false;
      this.Wilt();
    }
  }

  protected override void OnCleanUp()
  {
    this.wiltSchedulerHandler.ClearScheduler();
    this.recoverSchedulerHandler.ClearScheduler();
    base.OnCleanUp();
  }

  private void SetCondition(WiltCondition.Condition condition, bool satisfiedState)
  {
    if (!this.WiltConditions.ContainsKey((int) condition))
      return;
    this.WiltConditions[(int) condition] = satisfiedState;
    this.CheckShouldWilt();
  }

  private void CheckShouldWilt()
  {
    bool flag = false;
    foreach (KeyValuePair<int, bool> wiltCondition in this.WiltConditions)
    {
      if (!wiltCondition.Value)
      {
        flag = true;
        break;
      }
    }
    if (flag)
    {
      if (this.goingToWilt)
        return;
      this.Wilt();
    }
    else
    {
      if (!this.goingToWilt)
        return;
      this.Recover();
    }
  }

  private void Wilt()
  {
    if (this.goingToWilt)
      return;
    this.goingToWilt = true;
    this.recoverSchedulerHandler.ClearScheduler();
    if (this.wiltSchedulerHandler.IsValid)
      return;
    this.wiltSchedulerHandler = GameScheduler.Instance.Schedule(nameof (Wilt), this.WiltDelay, new System.Action<object>(WiltCondition.DoWiltCallback), (object) this, (SchedulerGroup) null);
  }

  private void Recover()
  {
    if (!this.goingToWilt)
      return;
    this.goingToWilt = false;
    this.wiltSchedulerHandler.ClearScheduler();
    if (this.recoverSchedulerHandler.IsValid)
      return;
    this.recoverSchedulerHandler = GameScheduler.Instance.Schedule(nameof (Recover), this.RecoveryDelay, new System.Action<object>(WiltCondition.DoRecoverCallback), (object) this, (SchedulerGroup) null);
  }

  private static void DoWiltCallback(object data) => ((WiltCondition) data).DoWilt();

  private void DoWilt()
  {
    this.wiltSchedulerHandler.ClearScheduler();
    KSelectable component = this.GetComponent<KSelectable>();
    component.GetComponent<KPrefabID>().AddTag(GameTags.Wilting);
    if (!this.wilting)
    {
      this.wilting = true;
      this.Trigger(-724860998, (object) null);
    }
    if ((UnityEngine.Object) this.rm != (UnityEngine.Object) null)
    {
      if (this.rm.Replanted)
        component.AddStatusItem(Db.Get().CreatureStatusItems.WiltingDomestic, (object) this.GetComponent<ReceptacleMonitor>());
      else
        component.AddStatusItem(Db.Get().CreatureStatusItems.Wilting, (object) this.GetComponent<ReceptacleMonitor>());
    }
    else
    {
      ReceptacleMonitor.StatesInstance smi = component.GetSMI<ReceptacleMonitor.StatesInstance>();
      if (smi != null && !smi.IsInsideState((StateMachine.BaseState) smi.sm.wild))
        component.AddStatusItem(Db.Get().CreatureStatusItems.WiltingNonGrowingDomestic, (object) this);
      else
        component.AddStatusItem(Db.Get().CreatureStatusItems.WiltingNonGrowing, (object) this);
    }
  }

  public string WiltCausesString()
  {
    string str = "";
    List<IWiltCause> allSmi = this.GetAllSMI<IWiltCause>();
    allSmi.AddRange((IEnumerable<IWiltCause>) this.GetComponents<IWiltCause>());
    foreach (IWiltCause wiltCause in allSmi)
    {
      foreach (WiltCondition.Condition condition in wiltCause.Conditions)
      {
        if (this.WiltConditions.ContainsKey((int) condition) && !this.WiltConditions[(int) condition])
        {
          str += "\n";
          str += wiltCause.WiltStateString;
          break;
        }
      }
    }
    return str;
  }

  private static void DoRecoverCallback(object data) => ((WiltCondition) data).DoRecover();

  private void DoRecover()
  {
    this.recoverSchedulerHandler.ClearScheduler();
    KSelectable component = this.GetComponent<KSelectable>();
    this.wilting = false;
    component.RemoveStatusItem(Db.Get().CreatureStatusItems.WiltingDomestic);
    component.RemoveStatusItem(Db.Get().CreatureStatusItems.Wilting);
    component.RemoveStatusItem(Db.Get().CreatureStatusItems.WiltingNonGrowing);
    component.RemoveStatusItem(Db.Get().CreatureStatusItems.WiltingNonGrowingDomestic);
    component.GetComponent<KPrefabID>().RemoveTag(GameTags.Wilting);
    this.Trigger(712767498, (object) null);
  }

  public enum Condition
  {
    Temperature,
    Pressure,
    AtmosphereElement,
    Drowning,
    Fertilized,
    DryingOut,
    Irrigation,
    IlluminationComfort,
    Darkness,
    Receptacle,
    Entombed,
    UnhealthyRoot,
    Count,
  }
}
