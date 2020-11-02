// Decompiled with JetBrains decompiler
// Type: EntombedChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class EntombedChore : Chore<EntombedChore.StatesInstance>
{
  public EntombedChore(IStateMachineTarget target)
    : base(Db.Get().ChoreTypes.Entombed, target, target.GetComponent<ChoreProvider>(), false, master_priority_class: PriorityScreen.PriorityClass.compulsory)
    => this.smi = new EntombedChore.StatesInstance(this, target.gameObject);

  public class StatesInstance : GameStateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.GameInstance
  {
    public StatesInstance(EntombedChore master, GameObject entombable)
      : base(master)
      => this.sm.entombable.Set(entombable, this.smi);

    public void UpdateFaceEntombed()
    {
      int num = Grid.CellAbove(Grid.PosToCell(this.transform.GetPosition()));
      this.sm.isFaceEntombed.Set(Grid.IsValidCell(num) && Grid.Solid[num], this.smi);
    }
  }

  public class States : GameStateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore>
  {
    public StateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.BoolParameter isFaceEntombed;
    public StateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.TargetParameter entombable;
    public GameStateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.State entombedface;
    public GameStateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.State entombedbody;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.entombedbody;
      this.Target(this.entombable);
      this.root.ToggleAnims("anim_emotes_default_kanim").Update("IsFaceEntombed", (System.Action<EntombedChore.StatesInstance, float>) ((smi, dt) => smi.UpdateFaceEntombed())).ToggleStatusItem(Db.Get().DuplicantStatusItems.EntombedChore);
      this.entombedface.PlayAnim("entombed_ceiling", KAnim.PlayMode.Loop).ParamTransition<bool>((StateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.Parameter<bool>) this.isFaceEntombed, this.entombedbody, GameStateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.IsFalse);
      this.entombedbody.PlayAnim("entombed_floor", KAnim.PlayMode.Loop).StopMoving().ParamTransition<bool>((StateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.Parameter<bool>) this.isFaceEntombed, this.entombedface, GameStateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.IsTrue);
    }
  }
}
