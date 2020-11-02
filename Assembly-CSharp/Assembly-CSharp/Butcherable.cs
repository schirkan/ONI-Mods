// Decompiled with JetBrains decompiler
// Type: Butcherable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/Butcherable")]
public class Butcherable : Workable, ISaveLoadable
{
  [MyCmpGet]
  private KAnimControllerBase controller;
  [MyCmpGet]
  private Harvestable harvestable;
  private bool readyToButcher;
  private bool butchered;
  public string[] Drops;
  private Chore chore;
  private static readonly EventSystem.IntraObjectHandler<Butcherable> SetReadyToButcherDelegate = new EventSystem.IntraObjectHandler<Butcherable>((System.Action<Butcherable, object>) ((component, data) => component.SetReadyToButcher(data)));
  private static readonly EventSystem.IntraObjectHandler<Butcherable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Butcherable>((System.Action<Butcherable, object>) ((component, data) => component.OnRefreshUserMenu(data)));

  public void SetDrops(string[] drops) => this.Drops = drops;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<Butcherable>(1272413801, Butcherable.SetReadyToButcherDelegate);
    this.Subscribe<Butcherable>(493375141, Butcherable.OnRefreshUserMenuDelegate);
    this.workTime = 3f;
    this.multitoolContext = (HashedString) "harvest";
    this.multitoolHitEffectTag = (Tag) "fx_harvest_splash";
  }

  public void SetReadyToButcher(object param) => this.readyToButcher = true;

  public void SetReadyToButcher(bool ready) => this.readyToButcher = ready;

  public void ActivateChore(object param)
  {
    if (this.chore != null)
      return;
    this.chore = (Chore) new WorkChore<Butcherable>(Db.Get().ChoreTypes.Harvest, (IStateMachineTarget) this);
    this.OnRefreshUserMenu((object) null);
  }

  public void CancelChore(object param)
  {
    if (this.chore == null)
      return;
    this.chore.Cancel("User cancelled");
    this.chore = (Chore) null;
  }

  private void OnClickCancel() => this.CancelChore((object) null);

  private void OnClickButcher()
  {
    if (DebugHandler.InstantBuildMode)
      this.OnButcherComplete();
    else
      this.ActivateChore((object) null);
  }

  private void OnRefreshUserMenu(object data)
  {
    if (!this.readyToButcher)
      return;
    Game.Instance.userMenu.AddButton(this.gameObject, this.chore != null ? new KIconButtonMenu.ButtonInfo("action_harvest", "Cancel Meatify", new System.Action(this.OnClickCancel)) : new KIconButtonMenu.ButtonInfo("action_harvest", "Meatify", new System.Action(this.OnClickButcher)));
  }

  protected override void OnCompleteWork(Worker worker) => this.OnButcherComplete();

  public void OnButcherComplete()
  {
    if (this.butchered)
      return;
    KSelectable component1 = this.GetComponent<KSelectable>();
    if ((bool) (UnityEngine.Object) component1 && component1.IsSelected)
      SelectTool.Instance.Select((KSelectable) null);
    for (int index = 0; index < this.Drops.Length; ++index)
    {
      GameObject go = Scenario.SpawnPrefab(this.GetDropSpawnLocation(), 0, 0, this.Drops[index]);
      go.SetActive(true);
      Edible component2 = go.GetComponent<Edible>();
      if ((bool) (UnityEngine.Object) component2)
        ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, component2.Calories, StringFormatter.Replace((string) UI.ENDOFDAYREPORT.NOTES.BUTCHERED, "{0}", go.GetProperName()), (string) UI.ENDOFDAYREPORT.NOTES.BUTCHERED_CONTEXT);
    }
    this.chore = (Chore) null;
    this.butchered = true;
    this.readyToButcher = false;
    Game.Instance.userMenu.Refresh(this.gameObject);
    this.Trigger(395373363, (object) null);
  }

  private int GetDropSpawnLocation()
  {
    int cell = Grid.PosToCell(this.gameObject);
    int num = Grid.CellAbove(cell);
    return Grid.IsValidCell(num) && !Grid.Solid[num] ? num : cell;
  }
}
