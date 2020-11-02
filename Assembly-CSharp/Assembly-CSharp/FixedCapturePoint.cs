// Decompiled with JetBrains decompiler
// Type: FixedCapturePoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using UnityEngine;

public class FixedCapturePoint : GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>
{
  private StateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.BoolParameter automated;
  public GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State unoperational;
  public FixedCapturePoint.OperationalState operational;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.operational;
    this.serializable = true;
    this.unoperational.TagTransition(GameTags.Operational, (GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State) this.operational);
    this.operational.DefaultState(this.operational.manual).TagTransition(GameTags.Operational, this.unoperational, true);
    this.operational.manual.ParamTransition<bool>((StateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.Parameter<bool>) this.automated, this.operational.automated, GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.IsTrue);
    this.operational.automated.ParamTransition<bool>((StateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.Parameter<bool>) this.automated, this.operational.manual, GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.IsFalse).ToggleChore((Func<FixedCapturePoint.Instance, Chore>) (smi => smi.CreateChore()), this.unoperational, this.unoperational).Update("FindFixedCapturable", (System.Action<FixedCapturePoint.Instance, float>) ((smi, dt) => smi.FindFixedCapturable()), UpdateRate.SIM_1000ms);
  }

  public class Def : StateMachine.BaseDef
  {
    public Func<GameObject, FixedCapturePoint.Instance, bool> isCreatureEligibleToBeCapturedCb;
    public Func<FixedCapturePoint.Instance, int> getTargetCapturePoint = (Func<FixedCapturePoint.Instance, int>) (smi =>
    {
      int cell = Grid.PosToCell((StateMachine.Instance) smi);
      Navigator component = smi.targetCapturable.GetComponent<Navigator>();
      if (Grid.IsValidCell(cell - 1) && component.CanReach(cell - 1))
        return cell - 1;
      return Grid.IsValidCell(cell + 1) && component.CanReach(cell + 1) ? cell + 1 : cell;
    });
  }

  public class OperationalState : GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State
  {
    public GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State manual;
    public GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State automated;
  }

  [SerializationConfig(MemberSerialization.OptIn)]
  public new class Instance : GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.GameInstance, ICheckboxControl
  {
    public FixedCapturableMonitor.Instance targetCapturable { get; private set; }

    public bool shouldCreatureGoGetCaptured { get; private set; }

    public Instance(IStateMachineTarget master, FixedCapturePoint.Def def)
      : base(master, def)
      => this.Subscribe(-905833192, new System.Action<object>(this.OnCopySettings));

    private void OnCopySettings(object data)
    {
      GameObject go = (GameObject) data;
      if ((UnityEngine.Object) go == (UnityEngine.Object) null)
        return;
      FixedCapturePoint.Instance smi = go.GetSMI<FixedCapturePoint.Instance>();
      if (smi == null)
        return;
      this.sm.automated.Set(this.sm.automated.Get(smi), this);
    }

    public Chore CreateChore()
    {
      this.FindFixedCapturable();
      return (Chore) new FixedCaptureChore(this.GetComponent<KPrefabID>());
    }

    public bool IsCreatureAvailableForFixedCapture()
    {
      if (this.targetCapturable.IsNullOrStopped())
        return false;
      int cell = Grid.PosToCell(this.transform.GetPosition());
      return FixedCapturePoint.Instance.CanCapturableBeCapturedAtCapturePoint(this.targetCapturable, this, Game.Instance.roomProber.GetCavityForCell(cell), cell);
    }

    public void SetRancherIsAvailableForCapturing() => this.shouldCreatureGoGetCaptured = true;

    public void ClearRancherIsAvailableForCapturing() => this.shouldCreatureGoGetCaptured = false;

    private static bool CanCapturableBeCapturedAtCapturePoint(
      FixedCapturableMonitor.Instance capturable,
      FixedCapturePoint.Instance capture_point,
      CavityInfo capture_cavity_info,
      int capture_cell)
    {
      if (!capturable.IsRunning() || capturable.targetCapturePoint != capture_point && !capturable.targetCapturePoint.IsNullOrStopped() || capture_point.def.isCreatureEligibleToBeCapturedCb != null && !capture_point.def.isCreatureEligibleToBeCapturedCb(capturable.gameObject, capture_point))
        return false;
      CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(capturable.transform.GetPosition()));
      if (cavityForCell == null || cavityForCell != capture_cavity_info || (capturable.HasTag(GameTags.Creatures.Bagged) || !capturable.GetComponent<ChoreConsumer>().IsChoreEqualOrAboveCurrentChorePriority<FixedCaptureStates>()) || capturable.GetComponent<Navigator>().GetNavigationCost(capture_cell) == -1)
        return false;
      TreeFilterable component1 = capture_point.GetComponent<TreeFilterable>();
      IUserControlledCapacity component2 = capture_point.GetComponent<IUserControlledCapacity>();
      Tag prefabTag = capturable.GetComponent<KPrefabID>().PrefabTag;
      return !component1.ContainsTag(prefabTag) || (double) component2.AmountStored > (double) component2.UserMaxCapacity;
    }

    public void FindFixedCapturable()
    {
      int cell = Grid.PosToCell(this.transform.GetPosition());
      CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
      if (cavityForCell == null)
      {
        this.ResetCapturePoint();
      }
      else
      {
        if (!this.targetCapturable.IsNullOrStopped() && !FixedCapturePoint.Instance.CanCapturableBeCapturedAtCapturePoint(this.targetCapturable, this, cavityForCell, cell))
          this.ResetCapturePoint();
        if (!this.targetCapturable.IsNullOrStopped())
          return;
        foreach (FixedCapturableMonitor.Instance capturableMonitor in Components.FixedCapturableMonitors)
        {
          if (FixedCapturePoint.Instance.CanCapturableBeCapturedAtCapturePoint(capturableMonitor, this, cavityForCell, cell))
          {
            this.targetCapturable = capturableMonitor;
            if (this.targetCapturable.IsNullOrStopped())
              break;
            this.targetCapturable.targetCapturePoint = this;
            break;
          }
        }
      }
    }

    public void ResetCapturePoint()
    {
      this.Trigger(643180843);
      if (this.targetCapturable.IsNullOrStopped())
        return;
      this.targetCapturable.targetCapturePoint = (FixedCapturePoint.Instance) null;
      this.targetCapturable.Trigger(1034952693);
      this.targetCapturable = (FixedCapturableMonitor.Instance) null;
    }

    string ICheckboxControl.CheckboxTitleKey => UI.UISIDESCREENS.CAPTURE_POINT_SIDE_SCREEN.TITLE.key.String;

    string ICheckboxControl.CheckboxLabel => (string) UI.UISIDESCREENS.CAPTURE_POINT_SIDE_SCREEN.AUTOWRANGLE;

    string ICheckboxControl.CheckboxTooltip => (string) UI.UISIDESCREENS.CAPTURE_POINT_SIDE_SCREEN.AUTOWRANGLE_TOOLTIP;

    bool ICheckboxControl.GetCheckboxValue() => this.sm.automated.Get(this);

    void ICheckboxControl.SetCheckboxValue(bool value) => this.sm.automated.Set(value, this);
  }
}
