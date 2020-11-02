﻿// Decompiled with JetBrains decompiler
// Type: SuperProductiveFX
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class SuperProductiveFX : GameStateMachine<SuperProductiveFX, SuperProductiveFX.Instance>
{
  public StateMachine<SuperProductiveFX, SuperProductiveFX.Instance, IStateMachineTarget, object>.Signal wasProductive;
  public StateMachine<SuperProductiveFX, SuperProductiveFX.Instance, IStateMachineTarget, object>.Signal destroyFX;
  public StateMachine<SuperProductiveFX, SuperProductiveFX.Instance, IStateMachineTarget, object>.TargetParameter fx;
  public GameStateMachine<SuperProductiveFX, SuperProductiveFX.Instance, IStateMachineTarget, object>.State pre;
  public GameStateMachine<SuperProductiveFX, SuperProductiveFX.Instance, IStateMachineTarget, object>.State idle;
  public GameStateMachine<SuperProductiveFX, SuperProductiveFX.Instance, IStateMachineTarget, object>.State productive;
  public GameStateMachine<SuperProductiveFX, SuperProductiveFX.Instance, IStateMachineTarget, object>.State pst;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.pre;
    this.Target(this.fx);
    this.root.OnSignal(this.wasProductive, this.productive, (Func<SuperProductiveFX.Instance, bool>) (smi => smi.GetCurrentState() != smi.sm.pst)).OnSignal(this.destroyFX, this.pst);
    this.pre.PlayAnim("productive_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.idle);
    this.idle.PlayAnim("productive_loop", KAnim.PlayMode.Loop);
    this.productive.QueueAnim("productive_achievement").OnAnimQueueComplete(this.idle);
    this.pst.PlayAnim("productive_pst").EventHandler(GameHashes.AnimQueueComplete, (StateMachine<SuperProductiveFX, SuperProductiveFX.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.DestroyFX()));
  }

  public new class Instance : GameStateMachine<SuperProductiveFX, SuperProductiveFX.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Instance(IStateMachineTarget master, Vector3 offset)
      : base(master)
      => this.sm.fx.Set(FXHelpers.CreateEffect("productive_fx_kanim", master.gameObject.transform.GetPosition() + offset, master.gameObject.transform, true).gameObject, this.smi);

    public void DestroyFX()
    {
      Util.KDestroyGameObject(this.sm.fx.Get(this.smi));
      this.smi.StopSM("destroyed");
    }
  }
}