// Decompiled with JetBrains decompiler
// Type: Shower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/Shower")]
public class Shower : Workable, IGameObjectEffectDescriptor
{
  private Shower.ShowerSM.Instance smi;
  public static string SHOWER_EFFECT = "Showered";
  public SimHashes outputTargetElement;
  public float fractionalDiseaseRemoval;
  public int absoluteDiseaseRemoval;
  private SimUtil.DiseaseInfo accumulatedDisease;
  public const float WATER_PER_USE = 5f;
  private static readonly string[] EffectsRemoved = new string[2]
  {
    "SoakingWet",
    "WetFeet"
  };

  private Shower() => this.SetReportType(ReportManager.ReportType.PersonalTime);

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.resetProgressOnStop = true;
    this.smi = new Shower.ShowerSM.Instance(this);
    this.smi.StartSM();
  }

  protected override void OnStartWork(Worker worker)
  {
    this.WorkTimeRemaining = this.workTime * worker.GetSMI<HygieneMonitor.Instance>().GetDirtiness();
    this.accumulatedDisease = SimUtil.DiseaseInfo.Invalid;
    this.smi.SetActive(true);
    base.OnStartWork(worker);
  }

  protected override void OnStopWork(Worker worker) => this.smi.SetActive(false);

  protected override void OnCompleteWork(Worker worker)
  {
    base.OnCompleteWork(worker);
    Effects component = worker.GetComponent<Effects>();
    for (int index = 0; index < Shower.EffectsRemoved.Length; ++index)
    {
      string effect_id = Shower.EffectsRemoved[index];
      component.Remove(effect_id);
    }
    component.Add(Shower.SHOWER_EFFECT, true);
    worker.GetSMI<HygieneMonitor.Instance>()?.SetDirtiness(0.0f);
  }

  protected override bool OnWorkTick(Worker worker, float dt)
  {
    PrimaryElement component = worker.GetComponent<PrimaryElement>();
    if (component.DiseaseCount > 0)
    {
      SimUtil.DiseaseInfo b = new SimUtil.DiseaseInfo()
      {
        idx = component.DiseaseIdx,
        count = Mathf.CeilToInt((float) component.DiseaseCount * (1f - Mathf.Pow(this.fractionalDiseaseRemoval, dt)) - (float) this.absoluteDiseaseRemoval)
      };
      component.ModifyDiseaseCount(-b.count, "Shower.RemoveDisease");
      this.accumulatedDisease = SimUtil.CalculateFinalDiseaseInfo(this.accumulatedDisease, b);
      PrimaryElement primaryElement = this.GetComponent<Storage>().FindPrimaryElement(this.outputTargetElement);
      if ((UnityEngine.Object) primaryElement != (UnityEngine.Object) null)
      {
        primaryElement.GetComponent<PrimaryElement>().AddDisease(this.accumulatedDisease.idx, this.accumulatedDisease.count, "Shower.RemoveDisease");
        this.accumulatedDisease = SimUtil.DiseaseInfo.Invalid;
      }
    }
    return false;
  }

  protected override void OnAbortWork(Worker worker)
  {
    base.OnAbortWork(worker);
    worker.GetSMI<HygieneMonitor.Instance>()?.SetDirtiness(1f - this.GetPercentComplete());
  }

  public override List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptors = base.GetDescriptors(go);
    if (Shower.EffectsRemoved.Length != 0)
    {
      Descriptor descriptor1 = new Descriptor();
      descriptor1.SetupDescriptor((string) UI.BUILDINGEFFECTS.REMOVESEFFECTSUBTITLE, (string) UI.BUILDINGEFFECTS.TOOLTIPS.REMOVESEFFECTSUBTITLE);
      descriptors.Add(descriptor1);
      for (int index = 0; index < Shower.EffectsRemoved.Length; ++index)
      {
        string str1 = Shower.EffectsRemoved[index];
        string str2 = (string) Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + str1.ToUpper() + ".NAME");
        string str3 = (string) Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + str1.ToUpper() + ".CAUSE");
        Descriptor descriptor2 = new Descriptor();
        descriptor2.IncreaseIndent();
        descriptor2.SetupDescriptor("• " + string.Format((string) UI.BUILDINGEFFECTS.REMOVEDEFFECT, (object) str2), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.REMOVEDEFFECT, (object) str3));
        descriptors.Add(descriptor2);
      }
    }
    Effect.AddModifierDescriptions(this.gameObject, descriptors, Shower.SHOWER_EFFECT, true);
    return descriptors;
  }

  public class ShowerSM : GameStateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower>
  {
    public GameStateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower, object>.State unoperational;
    public Shower.ShowerSM.OperationalState operational;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.unoperational;
      this.root.Update(new System.Action<Shower.ShowerSM.Instance, float>(this.UpdateStatusItems));
      this.unoperational.EventTransition(GameHashes.OperationalChanged, (GameStateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower, object>.State) this.operational, (StateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower, object>.Transition.ConditionCallback) (smi => smi.IsOperational)).PlayAnim("off");
      this.operational.DefaultState(this.operational.not_ready).EventTransition(GameHashes.OperationalChanged, this.unoperational, (StateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower, object>.Transition.ConditionCallback) (smi => !smi.IsOperational));
      this.operational.not_ready.EventTransition(GameHashes.OnStorageChange, this.operational.ready, (StateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower, object>.Transition.ConditionCallback) (smi => smi.IsReady())).PlayAnim("off");
      this.operational.ready.ToggleChore(new Func<Shower.ShowerSM.Instance, Chore>(this.CreateShowerChore), this.operational.not_ready);
    }

    private Chore CreateShowerChore(Shower.ShowerSM.Instance smi) => (Chore) new WorkChore<Shower>(Db.Get().ChoreTypes.Shower, (IStateMachineTarget) smi.master, allow_in_red_alert: false, schedule_block: Db.Get().ScheduleBlockTypes.Hygiene, allow_prioritization: false, priority_class: PriorityScreen.PriorityClass.high);

    private void UpdateStatusItems(Shower.ShowerSM.Instance smi, float dt)
    {
      if (smi.OutputFull())
        smi.master.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.OutputPipeFull, (object) this);
      else
        smi.master.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.OutputPipeFull);
    }

    public class OperationalState : GameStateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower, object>.State
    {
      public GameStateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower, object>.State not_ready;
      public GameStateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower, object>.State ready;
    }

    public new class Instance : GameStateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower, object>.GameInstance
    {
      private Operational operational;
      private ConduitConsumer consumer;
      private ConduitDispenser dispenser;

      public Instance(Shower master)
        : base(master)
      {
        this.operational = master.GetComponent<Operational>();
        this.consumer = master.GetComponent<ConduitConsumer>();
        this.dispenser = master.GetComponent<ConduitDispenser>();
      }

      public bool IsOperational => this.operational.IsOperational && this.consumer.IsConnected && this.dispenser.IsConnected;

      public void SetActive(bool active) => this.operational.SetActive(active);

      private bool HasSufficientMass()
      {
        bool flag = false;
        PrimaryElement primaryElement = this.GetComponent<Storage>().FindPrimaryElement(SimHashes.Water);
        if ((UnityEngine.Object) primaryElement != (UnityEngine.Object) null)
          flag = (double) primaryElement.Mass >= 5.0;
        return flag;
      }

      public bool OutputFull()
      {
        PrimaryElement primaryElement = this.GetComponent<Storage>().FindPrimaryElement(SimHashes.DirtyWater);
        return (UnityEngine.Object) primaryElement != (UnityEngine.Object) null && (double) primaryElement.Mass >= 5.0;
      }

      public bool IsReady() => this.HasSufficientMass() && !this.OutputFull();
    }
  }
}
