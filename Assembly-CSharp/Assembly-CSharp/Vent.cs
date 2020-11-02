// Decompiled with JetBrains decompiler
// Type: Vent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Vent")]
public class Vent : KMonoBehaviour, IGameObjectEffectDescriptor
{
  private int cell = -1;
  private int sortKey;
  [Serialize]
  public Dictionary<SimHashes, float> lifeTimeVentMass = new Dictionary<SimHashes, float>();
  private Vent.StatesInstance smi;
  [SerializeField]
  public ConduitType conduitType = ConduitType.Gas;
  [SerializeField]
  public Endpoint endpointType;
  [SerializeField]
  public float overpressureMass = 1f;
  [NonSerialized]
  public bool showConnectivityIcons = true;
  [MyCmpGet]
  [NonSerialized]
  public Structure structure;
  [MyCmpGet]
  [NonSerialized]
  public Operational operational;

  public int SortKey
  {
    get => this.sortKey;
    set => this.sortKey = value;
  }

  public void UpdateVentedMass(SimHashes element, float mass)
  {
    if (!this.lifeTimeVentMass.ContainsKey(element))
      this.lifeTimeVentMass.Add(element, mass);
    else
      this.lifeTimeVentMass[element] += mass;
  }

  public float GetVentedMass(SimHashes element) => this.lifeTimeVentMass.ContainsKey(element) ? this.lifeTimeVentMass[element] : 0.0f;

  public bool Closed()
  {
    bool flag = false;
    return this.operational.Flags.TryGetValue(LogicOperationalController.LogicOperationalFlag, out flag) && !flag || this.operational.Flags.TryGetValue(BuildingEnabledButton.EnabledFlag, out flag) && !flag;
  }

  protected override void OnSpawn()
  {
    this.cell = this.GetComponent<Building>().GetUtilityOutputCell();
    this.smi = new Vent.StatesInstance(this);
    this.smi.StartSM();
  }

  public Vent.State GetEndPointState()
  {
    Vent.State state = Vent.State.Invalid;
    switch (this.endpointType)
    {
      case Endpoint.Source:
        state = this.IsConnected() ? Vent.State.Ready : Vent.State.Blocked;
        break;
      case Endpoint.Sink:
        state = Vent.State.Ready;
        int cell = this.cell;
        if (!this.IsValidOutputCell(cell))
        {
          state = Grid.Solid[cell] ? Vent.State.Blocked : Vent.State.OverPressure;
          break;
        }
        break;
    }
    return state;
  }

  public bool IsConnected()
  {
    UtilityNetwork networkForCell = Conduit.GetNetworkManager(this.conduitType).GetNetworkForCell(this.cell);
    return networkForCell != null && (networkForCell as FlowUtilityNetwork).HasSinks;
  }

  public bool IsBlocked => this.GetEndPointState() != Vent.State.Ready;

  private bool IsValidOutputCell(int output_cell)
  {
    bool flag = false;
    if (((UnityEngine.Object) this.structure == (UnityEngine.Object) null || !this.structure.IsEntombed() || !this.Closed()) && !Grid.Solid[output_cell])
      flag = (double) Grid.Mass[output_cell] < (double) this.overpressureMass;
    return flag;
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    string formattedMass = GameUtil.GetFormattedMass(this.overpressureMass);
    return new List<Descriptor>()
    {
      new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.OVER_PRESSURE_MASS, (object) formattedMass), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.OVER_PRESSURE_MASS, (object) formattedMass))
    };
  }

  public enum State
  {
    Invalid,
    Ready,
    Blocked,
    OverPressure,
    Closed,
  }

  public class StatesInstance : GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.GameInstance
  {
    private Exhaust exhaust;

    public StatesInstance(Vent master)
      : base(master)
      => this.exhaust = master.GetComponent<Exhaust>();

    public bool NeedsExhaust() => (UnityEngine.Object) this.exhaust != (UnityEngine.Object) null && this.master.GetEndPointState() != Vent.State.Ready && this.master.endpointType == Endpoint.Source;

    public bool Blocked() => this.master.GetEndPointState() == Vent.State.Blocked && (uint) this.master.endpointType > 0U;

    public bool OverPressure() => (UnityEngine.Object) this.exhaust != (UnityEngine.Object) null && this.master.GetEndPointState() == Vent.State.OverPressure && (uint) this.master.endpointType > 0U;

    public void CheckTransitions()
    {
      if (this.NeedsExhaust())
        this.smi.GoTo((StateMachine.BaseState) this.sm.needExhaust);
      else if (this.master.Closed())
        this.smi.GoTo((StateMachine.BaseState) this.sm.closed);
      else if (this.Blocked())
        this.smi.GoTo((StateMachine.BaseState) this.sm.open.blocked);
      else if (this.OverPressure())
        this.smi.GoTo((StateMachine.BaseState) this.sm.open.overPressure);
      else
        this.smi.GoTo((StateMachine.BaseState) this.sm.open.idle);
    }

    public StatusItem SelectStatusItem(
      StatusItem gas_status_item,
      StatusItem liquid_status_item)
    {
      return this.master.conduitType != ConduitType.Gas ? liquid_status_item : gas_status_item;
    }
  }

  public class States : GameStateMachine<Vent.States, Vent.StatesInstance, Vent>
  {
    public Vent.States.OpenState open;
    public GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State closed;
    public GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State needExhaust;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.open.idle;
      this.root.Update("CheckTransitions", (System.Action<Vent.StatesInstance, float>) ((smi, dt) => smi.CheckTransitions()));
      this.open.TriggerOnEnter(GameHashes.VentOpen);
      this.closed.TriggerOnEnter(GameHashes.VentClosed);
      this.open.blocked.ToggleStatusItem((Func<Vent.StatesInstance, StatusItem>) (smi => smi.SelectStatusItem(Db.Get().BuildingStatusItems.GasVentObstructed, Db.Get().BuildingStatusItems.LiquidVentObstructed)));
      this.open.overPressure.ToggleStatusItem((Func<Vent.StatesInstance, StatusItem>) (smi => smi.SelectStatusItem(Db.Get().BuildingStatusItems.GasVentOverPressure, Db.Get().BuildingStatusItems.LiquidVentOverPressure)));
    }

    public class OpenState : GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State
    {
      public GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State idle;
      public GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State blocked;
      public GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State overPressure;
    }
  }
}
