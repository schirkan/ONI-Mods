// Decompiled with JetBrains decompiler
// Type: DoctorStation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/DoctorStation")]
public class DoctorStation : Workable
{
  private static readonly EventSystem.IntraObjectHandler<DoctorStation> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<DoctorStation>((System.Action<DoctorStation, object>) ((component, data) => component.OnStorageChange(data)));
  [MyCmpReq]
  public Storage storage;
  [MyCmpReq]
  public Operational operational;
  private DoctorStationDoctorWorkable doctor_workable;
  [SerializeField]
  public Tag supplyTag;
  private Dictionary<HashedString, Tag> treatments_available = new Dictionary<HashedString, Tag>();
  private DoctorStation.StatesInstance smi;
  public static readonly Chore.Precondition TreatmentAvailable = new Chore.Precondition()
  {
    id = nameof (TreatmentAvailable),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.TREATMENT_AVAILABLE,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => ((DoctorStation) data).IsTreatmentAvailable(context.consumerState.gameObject))
  };
  public static readonly Chore.Precondition DoctorAvailable = new Chore.Precondition()
  {
    id = nameof (DoctorAvailable),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.DOCTOR_AVAILABLE,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => ((DoctorStation) data).IsDoctorAvailable(context.consumerState.gameObject))
  };

  protected override void OnPrefabInit() => base.OnPrefabInit();

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Prioritizable.AddRef(this.gameObject);
    this.doctor_workable = this.GetComponent<DoctorStationDoctorWorkable>();
    this.SetWorkTime(float.PositiveInfinity);
    this.smi = new DoctorStation.StatesInstance(this);
    this.smi.StartSM();
    this.OnStorageChange();
    this.Subscribe<DoctorStation>(-1697596308, DoctorStation.OnStorageChangeDelegate);
  }

  protected override void OnCleanUp()
  {
    Prioritizable.RemoveRef(this.gameObject);
    if (this.smi != null)
    {
      this.smi.StopSM(nameof (OnCleanUp));
      this.smi = (DoctorStation.StatesInstance) null;
    }
    base.OnCleanUp();
  }

  private void OnStorageChange(object data = null)
  {
    this.treatments_available.Clear();
    foreach (GameObject go in this.storage.items)
    {
      if (go.HasTag(GameTags.MedicalSupplies))
      {
        Tag tag = go.PrefabID();
        if (tag == (Tag) "IntermediateCure")
          this.AddTreatment("SlimeSickness", tag);
        if (tag == (Tag) "AdvancedCure")
          this.AddTreatment("ZombieSickness", tag);
      }
    }
    this.smi.sm.hasSupplies.Set(this.treatments_available.Count > 0, this.smi);
  }

  private void AddTreatment(string id, Tag tag)
  {
    if (this.treatments_available.ContainsKey((HashedString) id))
      return;
    this.treatments_available.Add((HashedString) id, tag);
  }

  protected override void OnStartWork(Worker worker)
  {
    base.OnStartWork(worker);
    this.smi.sm.hasPatient.Set(true, this.smi);
  }

  protected override void OnStopWork(Worker worker)
  {
    base.OnStopWork(worker);
    this.smi.sm.hasPatient.Set(false, this.smi);
  }

  public override bool InstantlyFinish(Worker worker) => false;

  public void SetHasDoctor(bool has) => this.smi.sm.hasDoctor.Set(has, this.smi);

  public void CompleteDoctoring()
  {
    if (!(bool) (UnityEngine.Object) this.worker)
      return;
    this.CompleteDoctoring(this.worker.gameObject);
  }

  private void CompleteDoctoring(GameObject target)
  {
    Sicknesses sicknesses = target.GetSicknesses();
    if (sicknesses == null)
      return;
    bool flag = false;
    foreach (SicknessInstance sicknessInstance in (Modifications<Sickness, SicknessInstance>) sicknesses)
    {
      Tag tag;
      if (this.treatments_available.TryGetValue(sicknessInstance.Sickness.id, out tag))
      {
        Game.Instance.savedInfo.curedDisease = true;
        sicknessInstance.Cure();
        this.storage.ConsumeIgnoringDisease(tag, 1f);
        flag = true;
        break;
      }
    }
    if (flag)
      return;
    Debug.LogWarningFormat((UnityEngine.Object) this.gameObject, "Failed to treat any disease for {0}", (object) target);
  }

  public bool IsDoctorAvailable(GameObject target) => string.IsNullOrEmpty(this.doctor_workable.requiredSkillPerk) || MinionResume.AnyOtherMinionHasPerk(this.doctor_workable.requiredSkillPerk, target.GetComponent<MinionResume>());

  public bool IsTreatmentAvailable(GameObject target)
  {
    Sicknesses sicknesses = target.GetSicknesses();
    if (sicknesses != null)
    {
      foreach (SicknessInstance sicknessInstance in (Modifications<Sickness, SicknessInstance>) sicknesses)
      {
        if (this.treatments_available.TryGetValue(sicknessInstance.Sickness.id, out Tag _))
          return true;
      }
    }
    return false;
  }

  public class States : GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation>
  {
    public GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State unoperational;
    public DoctorStation.States.OperationalStates operational;
    public StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.BoolParameter hasSupplies;
    public StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.BoolParameter hasPatient;
    public StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.BoolParameter hasDoctor;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      this.serializable = false;
      default_state = (StateMachine.BaseState) this.unoperational;
      this.unoperational.EventTransition(GameHashes.OperationalChanged, (GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State) this.operational, (StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.Transition.ConditionCallback) (smi => smi.master.operational.IsOperational));
      this.operational.EventTransition(GameHashes.OperationalChanged, (GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State) this.operational, (StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.Transition.ConditionCallback) (smi => !smi.master.operational.IsOperational)).DefaultState(this.operational.not_ready);
      this.operational.not_ready.ParamTransition<bool>((StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.Parameter<bool>) this.hasSupplies, (GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State) this.operational.ready, (StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.Parameter<bool>.Callback) ((smi, p) => p));
      this.operational.ready.DefaultState(this.operational.ready.idle).ToggleRecurringChore(new Func<DoctorStation.StatesInstance, Chore>(this.CreatePatientChore)).ParamTransition<bool>((StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.Parameter<bool>) this.hasSupplies, this.operational.not_ready, (StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.Parameter<bool>.Callback) ((smi, p) => !p));
      this.operational.ready.idle.ParamTransition<bool>((StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.Parameter<bool>) this.hasPatient, (GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State) this.operational.ready.has_patient, (StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.Parameter<bool>.Callback) ((smi, p) => p));
      this.operational.ready.has_patient.ParamTransition<bool>((StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.Parameter<bool>) this.hasPatient, this.operational.ready.idle, (StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.Parameter<bool>.Callback) ((smi, p) => !p)).DefaultState(this.operational.ready.has_patient.waiting).ToggleRecurringChore(new Func<DoctorStation.StatesInstance, Chore>(this.CreateDoctorChore));
      this.operational.ready.has_patient.waiting.ParamTransition<bool>((StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.Parameter<bool>) this.hasDoctor, this.operational.ready.has_patient.being_treated, (StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.Parameter<bool>.Callback) ((smi, p) => p));
      this.operational.ready.has_patient.being_treated.ParamTransition<bool>((StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.Parameter<bool>) this.hasDoctor, this.operational.ready.has_patient.waiting, (StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.Parameter<bool>.Callback) ((smi, p) => !p)).Enter((StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State.Callback) (smi => smi.GetComponent<Operational>().SetActive(true))).Exit((StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State.Callback) (smi => smi.GetComponent<Operational>().SetActive(false)));
    }

    private Chore CreatePatientChore(DoctorStation.StatesInstance smi)
    {
      WorkChore<DoctorStation> workChore = new WorkChore<DoctorStation>(Db.Get().ChoreTypes.GetDoctored, (IStateMachineTarget) smi.master, allow_in_red_alert: false, allow_prioritization: false, priority_class: PriorityScreen.PriorityClass.personalNeeds);
      workChore.AddPrecondition(DoctorStation.TreatmentAvailable, (object) smi.master);
      workChore.AddPrecondition(DoctorStation.DoctorAvailable, (object) smi.master);
      return (Chore) workChore;
    }

    private Chore CreateDoctorChore(DoctorStation.StatesInstance smi)
    {
      DoctorStationDoctorWorkable component = smi.master.GetComponent<DoctorStationDoctorWorkable>();
      return (Chore) new WorkChore<DoctorStationDoctorWorkable>(Db.Get().ChoreTypes.Doctor, (IStateMachineTarget) component, allow_in_red_alert: false, allow_prioritization: false, priority_class: PriorityScreen.PriorityClass.high);
    }

    public class OperationalStates : GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State
    {
      public GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State not_ready;
      public DoctorStation.States.ReadyStates ready;
    }

    public class ReadyStates : GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State
    {
      public GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State idle;
      public DoctorStation.States.PatientStates has_patient;
    }

    public class PatientStates : GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State
    {
      public GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State waiting;
      public GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State being_treated;
    }
  }

  public class StatesInstance : GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.GameInstance
  {
    public StatesInstance(DoctorStation master)
      : base(master)
    {
    }
  }
}
