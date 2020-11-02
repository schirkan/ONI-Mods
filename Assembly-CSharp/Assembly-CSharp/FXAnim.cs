﻿// Decompiled with JetBrains decompiler
// Type: FXAnim
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class FXAnim : GameStateMachine<FXAnim, FXAnim.Instance>
{
  public StateMachine<FXAnim, FXAnim.Instance, IStateMachineTarget, object>.TargetParameter fx;
  public GameStateMachine<FXAnim, FXAnim.Instance, IStateMachineTarget, object>.State loop;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.loop;
    this.Target(this.fx);
    this.loop.Enter((StateMachine<FXAnim, FXAnim.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.Enter())).EventTransition(GameHashes.AnimQueueComplete, this.loop).Exit("Post", (StateMachine<FXAnim, FXAnim.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.Exit()));
  }

  public new class Instance : GameStateMachine<FXAnim, FXAnim.Instance, IStateMachineTarget, object>.GameInstance
  {
    private string anim;
    private KAnim.PlayMode mode;
    private KBatchedAnimController animController;

    public Instance(
      IStateMachineTarget master,
      string kanim_file,
      string anim,
      KAnim.PlayMode mode,
      Vector3 offset,
      Color32 tint_colour)
      : base(master)
    {
      this.animController = FXHelpers.CreateEffect(kanim_file, this.smi.master.transform.GetPosition() + offset, this.smi.master.transform);
      this.animController.gameObject.Subscribe(-1061186183, new System.Action<object>(this.OnAnimQueueComplete));
      this.animController.TintColour = tint_colour;
      this.sm.fx.Set(this.animController.gameObject, this.smi);
      this.anim = anim;
      this.mode = mode;
    }

    public void Enter() => this.animController.Play((HashedString) this.anim, this.mode);

    public void Exit() => this.DestroyFX();

    private void OnAnimQueueComplete(object data) => this.DestroyFX();

    private void DestroyFX() => Util.KDestroyGameObject(this.sm.fx.Get(this.smi));
  }
}
