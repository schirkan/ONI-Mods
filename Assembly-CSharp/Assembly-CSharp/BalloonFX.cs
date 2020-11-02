// Decompiled with JetBrains decompiler
// Type: BalloonFX
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class BalloonFX : GameStateMachine<BalloonFX, BalloonFX.Instance>
{
  public StateMachine<BalloonFX, BalloonFX.Instance, IStateMachineTarget, object>.TargetParameter fx;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.Target(this.fx);
    this.root.Exit("DestroyFX", (StateMachine<BalloonFX, BalloonFX.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.DestroyFX()));
  }

  public new class Instance : GameStateMachine<BalloonFX, BalloonFX.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Instance(IStateMachineTarget master)
      : base(master)
    {
      KBatchedAnimController effect = FXHelpers.CreateEffect("balloon_anim_kanim", master.gameObject.transform.GetPosition() + new Vector3(0.0f, 0.3f, 1f), master.transform, true, Grid.SceneLayer.Creatures);
      this.sm.fx.Set(effect.gameObject, this.smi);
      effect.GetComponent<KBatchedAnimController>().defaultAnim = "idle_default";
      master.GetComponent<KBatchedAnimController>().GetSynchronizer().Add((KAnimControllerBase) effect.GetComponent<KBatchedAnimController>());
    }

    public void DestroyFX() => Util.KDestroyGameObject(this.sm.fx.Get(this.smi));
  }
}
