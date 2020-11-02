﻿// Decompiled with JetBrains decompiler
// Type: PlantElementEmitter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class PlantElementEmitter : StateMachineComponent<PlantElementEmitter.StatesInstance>, IGameObjectEffectDescriptor
{
  [MyCmpGet]
  private WiltCondition wiltCondition;
  [MyCmpReq]
  private KSelectable selectable;
  public SimHashes emittedElement;
  public float emitRate;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  public List<Descriptor> GetDescriptors(GameObject go) => new List<Descriptor>();

  public class StatesInstance : GameStateMachine<PlantElementEmitter.States, PlantElementEmitter.StatesInstance, PlantElementEmitter, object>.GameInstance
  {
    public StatesInstance(PlantElementEmitter master)
      : base(master)
    {
    }

    public bool IsWilting() => !((UnityEngine.Object) this.master.wiltCondition == (UnityEngine.Object) null) && (UnityEngine.Object) this.master.wiltCondition != (UnityEngine.Object) null && this.master.wiltCondition.IsWilting();
  }

  public class States : GameStateMachine<PlantElementEmitter.States, PlantElementEmitter.StatesInstance, PlantElementEmitter>
  {
    public GameStateMachine<PlantElementEmitter.States, PlantElementEmitter.StatesInstance, PlantElementEmitter, object>.State wilted;
    public GameStateMachine<PlantElementEmitter.States, PlantElementEmitter.StatesInstance, PlantElementEmitter, object>.State healthy;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.healthy;
      this.serializable = true;
      this.healthy.EventTransition(GameHashes.Wilt, this.wilted, (StateMachine<PlantElementEmitter.States, PlantElementEmitter.StatesInstance, PlantElementEmitter, object>.Transition.ConditionCallback) (smi => smi.IsWilting())).Update("PlantEmit", (System.Action<PlantElementEmitter.StatesInstance, float>) ((smi, dt) => SimMessages.EmitMass(Grid.PosToCell(smi.master.gameObject), ElementLoader.FindElementByHash(smi.master.emittedElement).idx, smi.master.emitRate * dt, ElementLoader.FindElementByHash(smi.master.emittedElement).defaultValues.temperature, byte.MaxValue, 0)), UpdateRate.SIM_4000ms);
      this.wilted.EventTransition(GameHashes.WiltRecover, this.healthy);
    }
  }
}
