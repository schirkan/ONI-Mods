﻿// Decompiled with JetBrains decompiler
// Type: HotTubWorkerStateMachine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class HotTubWorkerStateMachine : GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, Worker>
{
  private GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, Worker, object>.State pre_front;
  private GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, Worker, object>.State pre_back;
  private GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, Worker, object>.State loop;
  private GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, Worker, object>.State loop_reenter;
  private GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, Worker, object>.State pst_back;
  private GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, Worker, object>.State pst_front;
  private GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, Worker, object>.State complete;
  public StateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, Worker, object>.TargetParameter worker;
  public static string[] workAnimLoopVariants = new string[3]
  {
    "working_loop1",
    "working_loop2",
    "working_loop3"
  };

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.pre_front;
    this.Target(this.worker);
    this.root.ToggleAnims("anim_interacts_hottub_kanim");
    this.pre_front.PlayAnim("working_pre_front").OnAnimQueueComplete(this.pre_back);
    this.pre_back.PlayAnim("working_pre_back").Enter((StateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, Worker, object>.State.Callback) (smi =>
    {
      Vector3 position = smi.transform.GetPosition();
      position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
      smi.transform.SetPosition(position);
    })).OnAnimQueueComplete(this.loop);
    this.loop.PlayAnim((Func<HotTubWorkerStateMachine.StatesInstance, string>) (smi => HotTubWorkerStateMachine.workAnimLoopVariants[UnityEngine.Random.Range(0, HotTubWorkerStateMachine.workAnimLoopVariants.Length)]), KAnim.PlayMode.Once).OnAnimQueueComplete(this.loop_reenter).EventTransition(GameHashes.WorkerPlayPostAnim, this.pst_back, (StateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, Worker, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Worker>().state == Worker.State.PendingCompletion));
    this.loop_reenter.GoTo(this.loop).EventTransition(GameHashes.WorkerPlayPostAnim, this.pst_back, (StateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, Worker, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Worker>().state == Worker.State.PendingCompletion));
    this.pst_back.PlayAnim("working_pst_back").OnAnimQueueComplete(this.pst_front);
    this.pst_front.PlayAnim("working_pst_front").Enter((StateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, Worker, object>.State.Callback) (smi =>
    {
      Vector3 position = smi.transform.GetPosition();
      position.z = Grid.GetLayerZ(Grid.SceneLayer.Move);
      smi.transform.SetPosition(position);
    })).OnAnimQueueComplete(this.complete);
  }

  public class StatesInstance : GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, Worker, object>.GameInstance
  {
    public StatesInstance(Worker master)
      : base(master)
      => this.sm.worker.Set((KMonoBehaviour) master, this.smi);
  }
}
