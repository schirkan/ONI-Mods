// Decompiled with JetBrains decompiler
// Type: WindTunnelWorkerStateMachine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class WindTunnelWorkerStateMachine : GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, Worker>
{
  private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, Worker, object>.State pre_front;
  private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, Worker, object>.State pre_back;
  private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, Worker, object>.State loop;
  private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, Worker, object>.State pst_back;
  private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, Worker, object>.State pst_front;
  private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, Worker, object>.State complete;
  public StateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, Worker, object>.TargetParameter worker;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.pre_front;
    this.Target(this.worker);
    this.root.ToggleAnims((Func<WindTunnelWorkerStateMachine.StatesInstance, HashedString>) (smi => smi.OverrideAnim));
    this.pre_front.PlayAnim((Func<WindTunnelWorkerStateMachine.StatesInstance, string>) (smi => smi.PreFrontAnim), KAnim.PlayMode.Once).OnAnimQueueComplete(this.pre_back);
    this.pre_back.PlayAnim((Func<WindTunnelWorkerStateMachine.StatesInstance, string>) (smi => smi.PreBackAnim), KAnim.PlayMode.Once).Enter((StateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, Worker, object>.State.Callback) (smi =>
    {
      Vector3 position = smi.transform.GetPosition();
      position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
      smi.transform.SetPosition(position);
    })).OnAnimQueueComplete(this.loop);
    this.loop.PlayAnim((Func<WindTunnelWorkerStateMachine.StatesInstance, string>) (smi => smi.LoopAnim), KAnim.PlayMode.Loop).EventTransition(GameHashes.WorkerPlayPostAnim, this.pst_back, (StateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, Worker, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Worker>().state == Worker.State.PendingCompletion));
    this.pst_back.PlayAnim((Func<WindTunnelWorkerStateMachine.StatesInstance, string>) (smi => smi.PstBackAnim), KAnim.PlayMode.Once).OnAnimQueueComplete(this.pst_front);
    this.pst_front.PlayAnim((Func<WindTunnelWorkerStateMachine.StatesInstance, string>) (smi => smi.PstFrontAnim), KAnim.PlayMode.Once).Enter((StateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, Worker, object>.State.Callback) (smi =>
    {
      Vector3 position = smi.transform.GetPosition();
      position.z = Grid.GetLayerZ(Grid.SceneLayer.Move);
      smi.transform.SetPosition(position);
    })).OnAnimQueueComplete(this.complete);
  }

  public class StatesInstance : GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, Worker, object>.GameInstance
  {
    private VerticalWindTunnelWorkable workable;

    public StatesInstance(Worker master, VerticalWindTunnelWorkable workable)
      : base(master)
    {
      this.workable = workable;
      this.sm.worker.Set((KMonoBehaviour) master, this.smi);
    }

    public HashedString OverrideAnim => this.workable.overrideAnim;

    public string PreFrontAnim => this.workable.preAnims[0];

    public string PreBackAnim => this.workable.preAnims[1];

    public string LoopAnim => this.workable.loopAnim;

    public string PstBackAnim => this.workable.pstAnims[0];

    public string PstFrontAnim => this.workable.pstAnims[1];
  }
}
