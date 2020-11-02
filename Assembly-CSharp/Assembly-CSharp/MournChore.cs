// Decompiled with JetBrains decompiler
// Type: MournChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;

public class MournChore : Chore<MournChore.StatesInstance>
{
  private static readonly CellOffset[] ValidStandingOffsets = new CellOffset[3]
  {
    new CellOffset(0, 0),
    new CellOffset(-1, 0),
    new CellOffset(1, 0)
  };
  private static readonly Chore.Precondition HasValidMournLocation = new Chore.Precondition()
  {
    id = "HasPlaceToStand",
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.HAS_PLACE_TO_STAND,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      Navigator component = ((IStateMachineTarget) data).GetComponent<Navigator>();
      bool flag = false;
      Grave graveToMournAt = MournChore.FindGraveToMournAt();
      if ((UnityEngine.Object) graveToMournAt != (UnityEngine.Object) null && Grid.IsValidCell(MournChore.GetStandableCell(Grid.PosToCell((KMonoBehaviour) graveToMournAt), component)))
        flag = true;
      return flag;
    })
  };

  private static int GetStandableCell(int cell, Navigator navigator)
  {
    foreach (CellOffset validStandingOffset in MournChore.ValidStandingOffsets)
    {
      if (Grid.IsCellOffsetValid(cell, validStandingOffset))
      {
        int num = Grid.OffsetCell(cell, validStandingOffset);
        if (!Grid.Reserved[num] && navigator.NavGrid.NavTable.IsValid(num) && navigator.GetNavigationCost(num) != -1)
          return num;
      }
    }
    return -1;
  }

  public MournChore(IStateMachineTarget master)
    : base(Db.Get().ChoreTypes.Mourn, master, master.GetComponent<ChoreProvider>(), false, master_priority_class: PriorityScreen.PriorityClass.high)
  {
    this.smi = new MournChore.StatesInstance(this);
    this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert);
    this.AddPrecondition(ChorePreconditions.instance.NoDeadBodies);
    this.AddPrecondition(MournChore.HasValidMournLocation, (object) master);
  }

  public static Grave FindGraveToMournAt()
  {
    Grave grave1 = (Grave) null;
    float num = -1f;
    foreach (Grave grave2 in Components.Graves)
    {
      if ((double) grave2.burialTime > (double) num)
      {
        num = grave2.burialTime;
        grave1 = grave2;
      }
    }
    return grave1;
  }

  public override void Begin(Chore.Precondition.Context context)
  {
    if ((UnityEngine.Object) context.consumerState.consumer == (UnityEngine.Object) null)
      Debug.LogError((object) "MournChore null context.consumer");
    else if (this.smi == null)
      Debug.LogError((object) "MournChore null smi");
    else if (this.smi.sm == null)
      Debug.LogError((object) "MournChore null smi.sm");
    else if ((UnityEngine.Object) MournChore.FindGraveToMournAt() == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "MournChore no grave");
    }
    else
    {
      this.smi.sm.mourner.Set(context.consumerState.gameObject, this.smi);
      base.Begin(context);
    }
  }

  public class StatesInstance : GameStateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.GameInstance
  {
    private int locatorCell = -1;

    public StatesInstance(MournChore master)
      : base(master)
    {
    }

    public void CreateLocator()
    {
      int standableCell = MournChore.GetStandableCell(Grid.PosToCell(MournChore.FindGraveToMournAt().transform.GetPosition()), this.master.GetComponent<Navigator>());
      if (standableCell < 0)
      {
        this.smi.GoTo((StateMachine.BaseState) null);
      }
      else
      {
        Grid.Reserved[standableCell] = true;
        this.smi.sm.locator.Set(ChoreHelpers.CreateLocator("MournLocator", Grid.CellToPosCBC(standableCell, Grid.SceneLayer.Move)), this.smi);
        this.locatorCell = standableCell;
        this.smi.GoTo((StateMachine.BaseState) this.sm.moveto);
      }
    }

    public void DestroyLocator()
    {
      if (this.locatorCell < 0)
        return;
      Grid.Reserved[this.locatorCell] = false;
      ChoreHelpers.DestroyLocator(this.sm.locator.Get(this));
      this.sm.locator.Set((KMonoBehaviour) null, this);
      this.locatorCell = -1;
    }
  }

  public class States : GameStateMachine<MournChore.States, MournChore.StatesInstance, MournChore>
  {
    public StateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.TargetParameter mourner;
    public StateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.TargetParameter locator;
    public GameStateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.State findOffset;
    public GameStateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.ApproachSubState<IApproachable> moveto;
    public GameStateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.State mourn;
    public GameStateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.State completed;
    private static readonly HashedString[] WORK_ANIMS = new HashedString[2]
    {
      (HashedString) "working_pre",
      (HashedString) "working_loop"
    };

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.findOffset;
      this.Target(this.mourner);
      this.root.ToggleAnims("anim_react_mourning_kanim").Exit("DestroyLocator", (StateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.State.Callback) (smi => smi.DestroyLocator()));
      this.findOffset.Enter("CreateLocator", (StateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.State.Callback) (smi => smi.CreateLocator()));
      this.moveto.InitializeStates(this.mourner, this.locator, this.mourn);
      this.mourn.PlayAnims((Func<MournChore.StatesInstance, HashedString[]>) (smi => MournChore.States.WORK_ANIMS), KAnim.PlayMode.Loop).ScheduleGoTo(10f, (StateMachine.BaseState) this.completed);
      this.completed.PlayAnim("working_pst").OnAnimQueueComplete((GameStateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.State) null).Exit((StateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.State.Callback) (smi => this.mourner.Get<Effects>(smi).Remove(Db.Get().effects.Get("Mourning"))));
    }
  }
}
