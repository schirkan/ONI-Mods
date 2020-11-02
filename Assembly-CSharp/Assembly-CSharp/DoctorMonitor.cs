// Decompiled with JetBrains decompiler
// Type: DoctorMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class DoctorMonitor : GameStateMachine<DoctorMonitor, DoctorMonitor.Instance>
{
  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.serializable = true;
    this.root.ToggleUrge(Db.Get().Urges.Doctor);
  }

  public new class Instance : GameStateMachine<DoctorMonitor, DoctorMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Instance(IStateMachineTarget master)
      : base(master)
    {
    }
  }
}
