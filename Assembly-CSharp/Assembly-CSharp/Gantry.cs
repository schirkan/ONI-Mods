// Decompiled with JetBrains decompiler
// Type: Gantry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

public class Gantry : Switch
{
  public static readonly HashedString PORT_ID = (HashedString) nameof (Gantry);
  [MyCmpReq]
  private Building building;
  public static CellOffset[] TileOffsets = new CellOffset[2]
  {
    new CellOffset(-2, 1),
    new CellOffset(-1, 1)
  };
  public static CellOffset[] RetractableOffsets = new CellOffset[4]
  {
    new CellOffset(0, 1),
    new CellOffset(1, 1),
    new CellOffset(2, 1),
    new CellOffset(3, 1)
  };
  private Gantry.Instance smi;
  private static StatusItem infoStatusItem;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (Gantry.infoStatusItem == null)
    {
      Gantry.infoStatusItem = new StatusItem("GantryAutomationInfo", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      Gantry.infoStatusItem.resolveStringCallback = new Func<string, object, string>(Gantry.ResolveInfoStatusItemString);
    }
    this.GetComponent<KAnimControllerBase>().PlaySpeedMultiplier = 0.5f;
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    PrimaryElement component = this.GetComponent<PrimaryElement>();
    for (int index = 0; index < Gantry.TileOffsets.Length; ++index)
    {
      CellOffset rotatedOffset = this.building.GetRotatedOffset(Gantry.TileOffsets[index]);
      int num = Grid.OffsetCell(cell, rotatedOffset);
      SimMessages.ReplaceAndDisplaceElement(num, component.ElementID, CellEventLogger.Instance.SimCellOccupierOnSpawn, component.Mass, component.Temperature);
      Grid.Objects[num, 1] = this.gameObject;
      Grid.Foundation[num] = true;
      Grid.Objects[num, 9] = this.gameObject;
      Grid.SetSolid(num, true, CellEventLogger.Instance.SimCellOccupierForceSolid);
      Grid.RenderedByWorld[num] = false;
      World.Instance.OnSolidChanged(num);
      GameScenePartitioner.Instance.TriggerEvent(num, GameScenePartitioner.Instance.solidChangedLayer, (object) null);
    }
    this.smi = new Gantry.Instance(this, this.IsSwitchedOn);
    this.smi.StartSM();
    this.GetComponent<KSelectable>().ToggleStatusItem(Gantry.infoStatusItem, true, (object) this.smi);
  }

  protected override void OnCleanUp()
  {
    if (this.smi != null)
      this.smi.StopSM("cleanup");
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    foreach (CellOffset tileOffset in Gantry.TileOffsets)
    {
      CellOffset rotatedOffset = this.building.GetRotatedOffset(tileOffset);
      int num = Grid.OffsetCell(cell, rotatedOffset);
      SimMessages.ReplaceAndDisplaceElement(num, SimHashes.Vacuum, CellEventLogger.Instance.SimCellOccupierOnSpawn, 0.0f);
      Grid.Objects[num, 1] = (GameObject) null;
      Grid.Objects[num, 9] = (GameObject) null;
      Grid.Foundation[num] = false;
      Grid.SetSolid(num, false, CellEventLogger.Instance.SimCellOccupierDestroy);
      Grid.RenderedByWorld[num] = true;
      World.Instance.OnSolidChanged(num);
      GameScenePartitioner.Instance.TriggerEvent(num, GameScenePartitioner.Instance.solidChangedLayer, (object) null);
    }
    foreach (CellOffset retractableOffset in Gantry.RetractableOffsets)
    {
      CellOffset rotatedOffset = this.building.GetRotatedOffset(retractableOffset);
      int num = Grid.OffsetCell(cell, rotatedOffset);
      Grid.FakeFloor[num] = false;
      Pathfinding.Instance.AddDirtyNavGridCell(num);
    }
    base.OnCleanUp();
  }

  public void SetWalkable(bool active)
  {
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    foreach (CellOffset retractableOffset in Gantry.RetractableOffsets)
    {
      CellOffset rotatedOffset = this.building.GetRotatedOffset(retractableOffset);
      int num = Grid.OffsetCell(cell, rotatedOffset);
      Grid.FakeFloor[num] = active;
      Pathfinding.Instance.AddDirtyNavGridCell(num);
    }
  }

  protected override void Toggle()
  {
    base.Toggle();
    this.smi.SetSwitchState(this.switchedOn);
  }

  protected override void OnRefreshUserMenu(object data)
  {
    if (this.smi.IsAutomated())
      return;
    base.OnRefreshUserMenu(data);
  }

  protected override void UpdateSwitchStatus()
  {
  }

  private static string ResolveInfoStatusItemString(string format_str, object data)
  {
    Gantry.Instance instance = (Gantry.Instance) data;
    return string.Format((string) (instance.IsAutomated() ? BUILDING.STATUSITEMS.GANTRY.AUTOMATION_CONTROL : BUILDING.STATUSITEMS.GANTRY.MANUAL_CONTROL), (object) (string) (instance.IsExtended() ? BUILDING.STATUSITEMS.GANTRY.EXTENDED : BUILDING.STATUSITEMS.GANTRY.RETRACTED));
  }

  public class States : GameStateMachine<Gantry.States, Gantry.Instance, Gantry>
  {
    public GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State retracted_pre;
    public GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State retracted;
    public GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State extended_pre;
    public GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State extended;
    public StateMachine<Gantry.States, Gantry.Instance, Gantry, object>.BoolParameter should_extend;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.extended;
      this.serializable = true;
      this.retracted_pre.Enter((StateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State.Callback) (smi => smi.SetActive(true))).Exit((StateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State.Callback) (smi => smi.SetActive(false))).PlayAnim("off_pre").OnAnimQueueComplete(this.retracted);
      this.retracted.PlayAnim("off").ParamTransition<bool>((StateMachine<Gantry.States, Gantry.Instance, Gantry, object>.Parameter<bool>) this.should_extend, this.extended_pre, GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.IsTrue);
      this.extended_pre.Enter((StateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State.Callback) (smi => smi.SetActive(true))).Exit((StateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State.Callback) (smi => smi.SetActive(false))).PlayAnim("on_pre").OnAnimQueueComplete(this.extended);
      this.extended.Enter((StateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State.Callback) (smi => smi.master.SetWalkable(true))).Exit((StateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State.Callback) (smi => smi.master.SetWalkable(false))).PlayAnim("on").ParamTransition<bool>((StateMachine<Gantry.States, Gantry.Instance, Gantry, object>.Parameter<bool>) this.should_extend, this.retracted_pre, GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.IsFalse);
    }
  }

  public class Instance : GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.GameInstance
  {
    private Operational operational;
    public LogicPorts logic;
    public bool logic_on = true;
    private bool manual_on;

    public Instance(Gantry master, bool manual_start_state)
      : base(master)
    {
      this.manual_on = manual_start_state;
      this.operational = this.GetComponent<Operational>();
      this.logic = this.GetComponent<LogicPorts>();
      this.Subscribe(-592767678, new System.Action<object>(this.OnOperationalChanged));
      this.Subscribe(-801688580, new System.Action<object>(this.OnLogicValueChanged));
      this.smi.sm.should_extend.Set(true, this.smi);
    }

    public bool IsAutomated() => this.logic.IsPortConnected(Gantry.PORT_ID);

    public bool IsExtended() => !this.IsAutomated() ? this.manual_on : this.logic_on;

    public void SetSwitchState(bool on)
    {
      this.manual_on = on;
      this.UpdateShouldExtend();
    }

    public void SetActive(bool active) => this.operational.SetActive(this.operational.IsOperational & active);

    private void OnOperationalChanged(object data) => this.UpdateShouldExtend();

    private void OnLogicValueChanged(object data)
    {
      LogicValueChanged logicValueChanged = (LogicValueChanged) data;
      if (logicValueChanged.portID != Gantry.PORT_ID)
        return;
      this.logic_on = LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue);
      this.UpdateShouldExtend();
    }

    private void UpdateShouldExtend()
    {
      if (!this.operational.IsOperational)
        return;
      if (this.IsAutomated())
        this.smi.sm.should_extend.Set(this.logic_on, this.smi);
      else
        this.smi.sm.should_extend.Set(this.manual_on, this.smi);
    }
  }
}
