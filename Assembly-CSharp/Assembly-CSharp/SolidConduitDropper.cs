// Decompiled with JetBrains decompiler
// Type: SolidConduitDropper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

[SerializationConfig(MemberSerialization.OptIn)]
public class SolidConduitDropper : StateMachineComponent<SolidConduitDropper.SMInstance>
{
  [MyCmpReq]
  private Operational operational;
  [MyCmpReq]
  private SolidConduitConsumer consumer;
  [MyCmpAdd]
  private Storage storage;

  protected override void OnPrefabInit() => base.OnPrefabInit();

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  protected override void OnCleanUp() => base.OnCleanUp();

  private void Update()
  {
    this.smi.sm.consuming.Set(this.consumer.IsConsuming, this.smi);
    this.smi.sm.isclosed.Set(!this.operational.IsOperational, this.smi);
    this.storage.DropAll();
  }

  public class SMInstance : GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.GameInstance
  {
    public SMInstance(SolidConduitDropper master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper>
  {
    public StateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.BoolParameter consuming;
    public StateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.BoolParameter isclosed;
    public GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.State idle;
    public GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.State working;
    public GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.State post;
    public GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.State closed;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.idle;
      this.root.Update("Update", (System.Action<SolidConduitDropper.SMInstance, float>) ((smi, dt) => smi.master.Update()), UpdateRate.SIM_1000ms);
      this.idle.PlayAnim("on").ParamTransition<bool>((StateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.Parameter<bool>) this.consuming, this.working, GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.IsTrue).ParamTransition<bool>((StateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.Parameter<bool>) this.isclosed, this.closed, GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.IsTrue);
      this.working.PlayAnim("working_pre").QueueAnim("working_loop", true).ParamTransition<bool>((StateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.Parameter<bool>) this.consuming, this.post, GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.IsFalse);
      this.post.PlayAnim("working_pst").OnAnimQueueComplete(this.idle);
      this.closed.PlayAnim("closed").ParamTransition<bool>((StateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.Parameter<bool>) this.consuming, this.working, GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.IsTrue).ParamTransition<bool>((StateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.Parameter<bool>) this.isclosed, this.idle, GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.IsFalse);
    }
  }
}
