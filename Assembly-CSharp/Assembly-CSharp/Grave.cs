// Decompiled with JetBrains decompiler
// Type: Grave
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

public class Grave : StateMachineComponent<Grave.StatesInstance>
{
  [Serialize]
  public string graveName;
  [Serialize]
  public int epitaphIdx;
  [Serialize]
  public float burialTime = -1f;
  private static readonly CellOffset[] DELIVERY_OFFSETS = new CellOffset[1];
  private static readonly EventSystem.IntraObjectHandler<Grave> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<Grave>((System.Action<Grave, object>) ((component, data) => component.OnStorageChanged(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<Grave>(-1697596308, Grave.OnStorageChangedDelegate);
    this.epitaphIdx = UnityEngine.Random.Range(0, int.MaxValue);
  }

  protected override void OnSpawn()
  {
    this.GetComponent<Storage>().SetOffsets(Grave.DELIVERY_OFFSETS);
    Storage component = this.GetComponent<Storage>();
    Storage storage = component;
    storage.OnWorkableEventCB = storage.OnWorkableEventCB + new System.Action<Workable.WorkableEvent>(this.OnWorkEvent);
    KAnimFile anim1 = Assets.GetAnim((HashedString) "anim_bury_dupe_kanim");
    int index = 0;
    KAnim.Anim anim2;
    while (true)
    {
      anim2 = anim1.GetData().GetAnim(index);
      if (anim2 != null)
      {
        if (!(anim2.name == "working_pre"))
          ++index;
        else
          break;
      }
      else
        goto label_5;
    }
    float work_time = (float) (anim2.numFrames - 3) / anim2.frameRate;
    component.SetWorkTime(work_time);
label_5:
    base.OnSpawn();
    this.smi.StartSM();
    Components.Graves.Add(this);
  }

  protected override void OnCleanUp()
  {
    Components.Graves.Remove(this);
    base.OnCleanUp();
  }

  private void OnStorageChanged(object data)
  {
    GameObject original = (GameObject) data;
    if (!((UnityEngine.Object) original != (UnityEngine.Object) null))
      return;
    this.graveName = original.name;
    Util.KDestroyGameObject(original);
  }

  private void OnWorkEvent(Workable.WorkableEvent evt)
  {
  }

  public class StatesInstance : GameStateMachine<Grave.States, Grave.StatesInstance, Grave, object>.GameInstance
  {
    private FetchChore chore;

    public StatesInstance(Grave master)
      : base(master)
    {
    }

    public void CreateFetchTask()
    {
      this.chore = new FetchChore(Db.Get().ChoreTypes.FetchCritical, this.GetComponent<Storage>(), 1f, new Tag[1]
      {
        GameTags.Corpse
      });
      this.chore.allowMultifetch = false;
    }

    public void CancelFetchTask()
    {
      this.chore.Cancel("Exit State");
      this.chore = (FetchChore) null;
    }
  }

  public class States : GameStateMachine<Grave.States, Grave.StatesInstance, Grave>
  {
    public GameStateMachine<Grave.States, Grave.StatesInstance, Grave, object>.State empty;
    public GameStateMachine<Grave.States, Grave.StatesInstance, Grave, object>.State full;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.empty;
      this.serializable = true;
      this.empty.PlayAnim("open").Enter("CreateFetchTask", (StateMachine<Grave.States, Grave.StatesInstance, Grave, object>.State.Callback) (smi => smi.CreateFetchTask())).Exit("CancelFetchTask", (StateMachine<Grave.States, Grave.StatesInstance, Grave, object>.State.Callback) (smi => smi.CancelFetchTask())).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GraveEmpty).EventTransition(GameHashes.OnStorageChange, this.full);
      this.full.PlayAnim("closed").ToggleMainStatusItem(Db.Get().BuildingStatusItems.Grave).Enter((StateMachine<Grave.States, Grave.StatesInstance, Grave, object>.State.Callback) (smi =>
      {
        if ((double) smi.master.burialTime >= 0.0)
          return;
        smi.master.burialTime = GameClock.Instance.GetTime();
      }));
    }
  }
}
