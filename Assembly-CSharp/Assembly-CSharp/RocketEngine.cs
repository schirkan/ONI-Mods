// Decompiled with JetBrains decompiler
// Type: RocketEngine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;

[SerializationConfig(MemberSerialization.OptIn)]
public class RocketEngine : StateMachineComponent<RocketEngine.StatesInstance>
{
  public float exhaustEmitRate = 50f;
  public float exhaustTemperature = 1500f;
  public SpawnFXHashes explosionEffectHash;
  public SimHashes exhaustElement = SimHashes.CarbonDioxide;
  public Tag fuelTag;
  public float efficiency = 1f;
  public bool requireOxidizer = true;
  public bool mainEngine = true;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
    if (!this.mainEngine)
      return;
    RequireAttachedComponent attachedComponent = new RequireAttachedComponent(this.gameObject.GetComponent<AttachableBuilding>(), typeof (FuelTank), (string) UI.STARMAP.COMPONENT.FUEL_TANK);
    this.GetComponent<RocketModule>().AddLaunchCondition((RocketLaunchCondition) attachedComponent);
  }

  public class StatesInstance : GameStateMachine<RocketEngine.States, RocketEngine.StatesInstance, RocketEngine, object>.GameInstance
  {
    public StatesInstance(RocketEngine smi)
      : base(smi)
    {
    }
  }

  public class States : GameStateMachine<RocketEngine.States, RocketEngine.StatesInstance, RocketEngine>
  {
    public GameStateMachine<RocketEngine.States, RocketEngine.StatesInstance, RocketEngine, object>.State idle;
    public GameStateMachine<RocketEngine.States, RocketEngine.StatesInstance, RocketEngine, object>.State burning;
    public GameStateMachine<RocketEngine.States, RocketEngine.StatesInstance, RocketEngine, object>.State burnComplete;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.idle;
      this.idle.PlayAnim("grounded", KAnim.PlayMode.Loop).EventTransition(GameHashes.IgniteEngine, this.burning);
      this.burning.EventTransition(GameHashes.LandRocket, this.burnComplete).PlayAnim("launch_pre").QueueAnim("launch_loop", true).Update((System.Action<RocketEngine.StatesInstance, float>) ((smi, dt) =>
      {
        int cell = Grid.PosToCell(smi.master.gameObject.transform.GetPosition() + smi.master.GetComponent<KBatchedAnimController>().Offset);
        if (Grid.IsValidCell(cell))
          SimMessages.EmitMass(cell, (byte) ElementLoader.GetElementIndex(smi.master.exhaustElement), dt * smi.master.exhaustEmitRate, smi.master.exhaustTemperature, (byte) 0, 0);
        int num1 = 10;
        for (int index = 1; index < num1; ++index)
        {
          int num2 = Grid.OffsetCell(cell, -1, -index);
          int num3 = Grid.OffsetCell(cell, 0, -index);
          int num4 = Grid.OffsetCell(cell, 1, -index);
          if (Grid.IsValidCell(num2))
            SimMessages.ModifyEnergy(num2, smi.master.exhaustTemperature / (float) (index + 1), 3200f, SimMessages.EnergySourceID.Burner);
          if (Grid.IsValidCell(num3))
            SimMessages.ModifyEnergy(num3, smi.master.exhaustTemperature / (float) index, 3200f, SimMessages.EnergySourceID.Burner);
          if (Grid.IsValidCell(num4))
            SimMessages.ModifyEnergy(num4, smi.master.exhaustTemperature / (float) (index + 1), 3200f, SimMessages.EnergySourceID.Burner);
        }
      }));
      this.burnComplete.PlayAnim("grounded", KAnim.PlayMode.Loop).EventTransition(GameHashes.IgniteEngine, this.burning);
    }
  }
}
