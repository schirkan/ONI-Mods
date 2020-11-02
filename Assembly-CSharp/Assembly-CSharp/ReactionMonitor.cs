// Decompiled with JetBrains decompiler
// Type: ReactionMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class ReactionMonitor : GameStateMachine<ReactionMonitor, ReactionMonitor.Instance>
{
  public GameStateMachine<ReactionMonitor, ReactionMonitor.Instance, IStateMachineTarget, object>.State idle;
  public GameStateMachine<ReactionMonitor, ReactionMonitor.Instance, IStateMachineTarget, object>.State reacting;
  public GameStateMachine<ReactionMonitor, ReactionMonitor.Instance, IStateMachineTarget, object>.State dead;
  public StateMachine<ReactionMonitor, ReactionMonitor.Instance, IStateMachineTarget, object>.ObjectParameter<Reactable> reactable;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.idle;
    this.serializable = false;
    this.idle.Enter("ClearReactable", (StateMachine<ReactionMonitor, ReactionMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => this.reactable.Set((Reactable) null, smi))).TagTransition(GameTags.Dead, this.dead);
    this.reacting.Enter("Reactable.Begin", (StateMachine<ReactionMonitor, ReactionMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => this.reactable.Get(smi).Begin(smi.gameObject))).Update("Reactable.Update", (System.Action<ReactionMonitor.Instance, float>) ((smi, dt) => this.reactable.Get(smi).Update(dt))).Exit("Reactable.End", (StateMachine<ReactionMonitor, ReactionMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => this.reactable.Get(smi).End())).EventTransition(GameHashes.NavigationFailed, this.idle).Enter((StateMachine<ReactionMonitor, ReactionMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.master.Trigger(-909573545))).Exit((StateMachine<ReactionMonitor, ReactionMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.master.Trigger(824899998))).Enter("Reactable.AddChorePreventionTag", (StateMachine<ReactionMonitor, ReactionMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi =>
    {
      if (!this.reactable.Get(smi).preventChoreInterruption)
        return;
      smi.GetComponent<KPrefabID>().AddTag(GameTags.PreventChoreInterruption);
    })).Exit("Reactable.RemoveChorePreventionTag", (StateMachine<ReactionMonitor, ReactionMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi =>
    {
      if (!this.reactable.Get(smi).preventChoreInterruption)
        return;
      smi.GetComponent<KPrefabID>().RemoveTag(GameTags.PreventChoreInterruption);
    })).TagTransition(GameTags.Dying, this.dead).TagTransition(GameTags.Dead, this.dead);
    this.dead.DoNothing();
  }

  public new class Instance : GameStateMachine<ReactionMonitor, ReactionMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    private bool justReacted;
    private Reactable lastReactable;
    private Dictionary<HashedString, float> lastReactTimes;
    private List<Reactable> oneshotReactables;

    public Instance(IStateMachineTarget master)
      : base(master)
    {
      this.lastReactTimes = new Dictionary<HashedString, float>();
      this.oneshotReactables = new List<Reactable>();
    }

    public void PollForReactables(Navigator.ActiveTransition transition)
    {
      if (this.IsReacting())
        return;
      if (this.justReacted)
        this.justReacted = false;
      else
        this.lastReactable = (Reactable) null;
      for (int index = this.oneshotReactables.Count - 1; index >= 0; --index)
      {
        Reactable oneshotReactable = this.oneshotReactables[index];
        if (oneshotReactable.IsExpired())
          oneshotReactable.Cleanup();
      }
      int cell = Grid.PosToCell(this.smi.gameObject);
      ListPool<ScenePartitionerEntry, ReactionMonitor>.PooledList pooledList = ListPool<ScenePartitionerEntry, ReactionMonitor>.Allocate();
      GameScenePartitioner.Instance.GatherEntries(Grid.CellToXY(cell).x, Grid.CellToXY(cell).y, 1, 1, GameScenePartitioner.Instance.objectLayers[0], (List<ScenePartitionerEntry>) pooledList);
      for (int index = 0; index < pooledList.Count; ++index)
      {
        if (pooledList[index].obj is Reactable reactable && reactable != this.lastReactable && (!this.lastReactTimes.ContainsKey(reactable.id) || (double) GameClock.Instance.GetTime() - (double) this.lastReactTimes[reactable.id] >= (double) reactable.minReactorTime) && reactable.CanBegin(this.gameObject, transition))
        {
          this.justReacted = true;
          this.lastReactable = reactable;
          this.lastReactTimes[reactable.id] = GameClock.Instance.GetTime();
          this.sm.reactable.Set(reactable, this.smi);
          this.smi.GoTo((StateMachine.BaseState) this.sm.reacting);
          break;
        }
      }
      pooledList.Recycle();
    }

    public void StopReaction()
    {
      for (int index = this.oneshotReactables.Count - 1; index >= 0; --index)
      {
        if (this.sm.reactable.Get(this.smi) == this.oneshotReactables[index])
        {
          this.oneshotReactables[index].Cleanup();
          this.oneshotReactables.RemoveAt(index);
        }
      }
      this.smi.GoTo((StateMachine.BaseState) this.sm.idle);
    }

    public bool IsReacting() => this.smi.IsInsideState((StateMachine.BaseState) this.sm.reacting);

    public void AddOneshotReactable(SelfEmoteReactable reactable) => this.oneshotReactables.Add((Reactable) reactable);

    public void CancelOneShotReactable(SelfEmoteReactable cancel_target)
    {
      for (int index = this.oneshotReactables.Count - 1; index >= 0; --index)
      {
        Reactable oneshotReactable = this.oneshotReactables[index];
        if (cancel_target == oneshotReactable)
        {
          oneshotReactable.Cleanup();
          break;
        }
      }
    }
  }
}
