// Decompiled with JetBrains decompiler
// Type: ReceptacleMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

[SkipSaveFileSerialization]
public class ReceptacleMonitor : StateMachineComponent<ReceptacleMonitor.StatesInstance>, IGameObjectEffectDescriptor, IWiltCause, ISim1000ms
{
  private bool replanted;

  public bool Replanted => this.replanted;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  public PlantablePlot GetReceptacle() => (PlantablePlot) this.smi.sm.receptacle.Get(this.smi);

  public void SetReceptacle(PlantablePlot plot = null)
  {
    if ((Object) plot == (Object) null)
    {
      this.smi.sm.receptacle.Set((SingleEntityReceptacle) null, this.smi);
      this.replanted = false;
    }
    else
    {
      this.smi.sm.receptacle.Set((SingleEntityReceptacle) plot, this.smi);
      this.replanted = true;
    }
  }

  public void Sim1000ms(float dt)
  {
    if ((Object) this.smi.sm.receptacle.Get(this.smi) == (Object) null)
    {
      this.smi.GoTo((StateMachine.BaseState) this.smi.sm.wild);
    }
    else
    {
      Operational component = this.smi.sm.receptacle.Get(this.smi).GetComponent<Operational>();
      if ((Object) component == (Object) null)
        this.smi.GoTo((StateMachine.BaseState) this.smi.sm.operational);
      else if (component.IsOperational)
        this.smi.GoTo((StateMachine.BaseState) this.smi.sm.operational);
      else
        this.smi.GoTo((StateMachine.BaseState) this.smi.sm.inoperational);
    }
  }

  WiltCondition.Condition[] IWiltCause.Conditions => new WiltCondition.Condition[1]
  {
    WiltCondition.Condition.Receptacle
  };

  public string WiltStateString
  {
    get
    {
      string str = "";
      if (this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.inoperational))
        str += (string) CREATURES.STATUSITEMS.RECEPTACLEINOPERATIONAL.NAME;
      return str;
    }
  }

  public bool HasReceptacle() => !this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.wild);

  public bool HasOperationalReceptacle() => this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.operational);

  public List<Descriptor> GetDescriptors(GameObject go) => new List<Descriptor>()
  {
    new Descriptor((string) UI.GAMEOBJECTEFFECTS.REQUIRES_RECEPTACLE, (string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_RECEPTACLE, Descriptor.DescriptorType.Requirement)
  };

  public class StatesInstance : GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.GameInstance
  {
    public StatesInstance(ReceptacleMonitor master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor>
  {
    public StateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.ObjectParameter<SingleEntityReceptacle> receptacle;
    public GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.State wild;
    public GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.State inoperational;
    public GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.State operational;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.wild;
      this.serializable = true;
      this.wild.TriggerOnEnter(GameHashes.ReceptacleOperational);
      this.inoperational.TriggerOnEnter(GameHashes.ReceptacleInoperational);
      this.operational.TriggerOnEnter(GameHashes.ReceptacleOperational);
    }
  }
}
