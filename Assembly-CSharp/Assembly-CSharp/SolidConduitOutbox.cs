// Decompiled with JetBrains decompiler
// Type: SolidConduitOutbox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class SolidConduitOutbox : StateMachineComponent<SolidConduitOutbox.SMInstance>
{
  [MyCmpReq]
  private Operational operational;
  [MyCmpReq]
  private SolidConduitConsumer consumer;
  [MyCmpAdd]
  private Storage storage;
  private MeterController meter;
  private static readonly EventSystem.IntraObjectHandler<SolidConduitOutbox> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<SolidConduitOutbox>((System.Action<SolidConduitOutbox, object>) ((component, data) => component.OnStorageChanged(data)));

  protected override void OnPrefabInit() => base.OnPrefabInit();

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.meter = new MeterController((KMonoBehaviour) this, Meter.Offset.Infront, Grid.SceneLayer.NoLayer, (string[]) Array.Empty<string>());
    this.Subscribe<SolidConduitOutbox>(-1697596308, SolidConduitOutbox.OnStorageChangedDelegate);
    this.UpdateMeter();
    this.smi.StartSM();
  }

  protected override void OnCleanUp() => base.OnCleanUp();

  private void OnStorageChanged(object data) => this.UpdateMeter();

  private void UpdateMeter() => this.meter.SetPositionPercent(Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg));

  private void UpdateConsuming() => this.smi.sm.consuming.Set(this.consumer.IsConsuming, this.smi);

  public class SMInstance : GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.GameInstance
  {
    public SMInstance(SolidConduitOutbox master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox>
  {
    public StateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.BoolParameter consuming;
    public GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.State idle;
    public GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.State working;
    public GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.State post;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.idle;
      this.root.Update("RefreshConsuming", (System.Action<SolidConduitOutbox.SMInstance, float>) ((smi, dt) => smi.master.UpdateConsuming()), UpdateRate.SIM_1000ms);
      this.idle.PlayAnim("on").ParamTransition<bool>((StateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.Parameter<bool>) this.consuming, this.working, GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.IsTrue);
      this.working.PlayAnim("working_pre").QueueAnim("working_loop", true).ParamTransition<bool>((StateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.Parameter<bool>) this.consuming, this.post, GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.IsFalse);
      this.post.PlayAnim("working_pst").OnAnimQueueComplete(this.idle);
    }
  }
}
