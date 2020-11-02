// Decompiled with JetBrains decompiler
// Type: SetLocker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;

public class SetLocker : StateMachineComponent<SetLocker.StatesInstance>
{
  public string[] possible_contents_ids;
  public string machineSound;
  public string overrideAnim;
  public Vector2I dropOffset = Vector2I.zero;
  [Serialize]
  private string contents = "";
  private static readonly EventSystem.IntraObjectHandler<SetLocker> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<SetLocker>((System.Action<SetLocker, object>) ((component, data) => component.OnRefreshUserMenu(data)));
  [Serialize]
  private bool used;
  private Chore chore;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.contents = this.possible_contents_ids[UnityEngine.Random.Range(0, this.possible_contents_ids.Length)];
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
    this.Subscribe<SetLocker>(493375141, SetLocker.OnRefreshUserMenuDelegate);
  }

  public void DropContents()
  {
    Scenario.SpawnPrefab(Grid.PosToCell(this.gameObject), this.dropOffset.x, this.dropOffset.y, this.contents, Grid.SceneLayer.Front).SetActive(true);
    PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, Assets.GetPrefab(this.contents.ToTag()).GetProperName(), this.smi.master.transform);
  }

  private void OnRefreshUserMenu(object data)
  {
    if (!this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.closed) || this.used)
      return;
    Game.Instance.userMenu.AddButton(this.gameObject, this.chore != null ? new KIconButtonMenu.ButtonInfo("action_empty_contents", (string) UI.USERMENUACTIONS.OPENPOI.NAME_OFF, new System.Action(this.OnClickCancel), tooltipText: ((string) UI.USERMENUACTIONS.OPENPOI.TOOLTIP_OFF)) : new KIconButtonMenu.ButtonInfo("action_empty_contents", (string) UI.USERMENUACTIONS.OPENPOI.NAME, new System.Action(this.OnClickOpen), tooltipText: ((string) UI.USERMENUACTIONS.OPENPOI.TOOLTIP)));
  }

  private void OnClickOpen() => this.ActivateChore();

  private void OnClickCancel() => this.CancelChore();

  public void ActivateChore(object param = null)
  {
    if (this.chore != null)
      return;
    this.GetComponent<Workable>().SetWorkTime(1.5f);
    this.chore = (Chore) new WorkChore<Workable>(Db.Get().ChoreTypes.EmptyStorage, (IStateMachineTarget) this, on_complete: ((System.Action<Chore>) (o => this.CompleteChore())), override_anims: Assets.GetAnim((HashedString) this.overrideAnim), priority_class: PriorityScreen.PriorityClass.high);
    this.OnRefreshUserMenu((object) null);
  }

  public void CancelChore(object param = null)
  {
    if (this.chore == null)
      return;
    this.chore.Cancel("User cancelled");
    this.chore = (Chore) null;
  }

  private void CompleteChore()
  {
    this.used = true;
    this.smi.GoTo((StateMachine.BaseState) this.smi.sm.open);
    this.chore = (Chore) null;
    Game.Instance.userMenu.Refresh(this.gameObject);
  }

  public class StatesInstance : GameStateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker, object>.GameInstance
  {
    public StatesInstance(SetLocker master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker>
  {
    public GameStateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker, object>.State closed;
    public GameStateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker, object>.State open;
    public GameStateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker, object>.State off;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.closed;
      this.serializable = true;
      this.closed.PlayAnim("on").Enter((StateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker, object>.State.Callback) (smi =>
      {
        if (smi.master.machineSound == null)
          return;
        LoopingSounds component = smi.master.GetComponent<LoopingSounds>();
        if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
          return;
        component.StartSound(GlobalAssets.GetSound(smi.master.machineSound));
      }));
      this.open.PlayAnim("working").OnAnimQueueComplete(this.off).Exit((StateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker, object>.State.Callback) (smi => smi.master.DropContents()));
      this.off.PlayAnim("off").Enter((StateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker, object>.State.Callback) (smi =>
      {
        if (smi.master.machineSound == null)
          return;
        LoopingSounds component = smi.master.GetComponent<LoopingSounds>();
        if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
          return;
        component.StopSound(GlobalAssets.GetSound(smi.master.machineSound));
      }));
    }
  }
}
