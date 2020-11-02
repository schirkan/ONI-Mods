// Decompiled with JetBrains decompiler
// Type: CringeMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

public class CringeMonitor : GameStateMachine<CringeMonitor, CringeMonitor.Instance>
{
  private static readonly HashedString[] CringeAnims = new HashedString[3]
  {
    (HashedString) "cringe_pre",
    (HashedString) "cringe_loop",
    (HashedString) "cringe_pst"
  };
  public GameStateMachine<CringeMonitor, CringeMonitor.Instance, IStateMachineTarget, object>.State idle;
  public GameStateMachine<CringeMonitor, CringeMonitor.Instance, IStateMachineTarget, object>.State cringe;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.idle;
    this.idle.EventHandler(GameHashes.Cringe, new GameStateMachine<CringeMonitor, CringeMonitor.Instance, IStateMachineTarget, object>.GameEvent.Callback(this.TriggerCringe));
    this.cringe.ToggleChore((Func<CringeMonitor.Instance, Chore>) (smi => (Chore) new EmoteChore(smi.master, Db.Get().ChoreTypes.EmoteHighPriority, (HashedString) "anim_cringe_kanim", CringeMonitor.CringeAnims, new Func<StatusItem>(smi.GetStatusItem))), this.idle).ScheduleGoTo(3f, (StateMachine.BaseState) this.idle);
  }

  private void TriggerCringe(CringeMonitor.Instance smi, object data)
  {
    if (smi.GetComponent<KPrefabID>().HasTag(GameTags.Suit))
      return;
    smi.SetCringeSourceData(data);
    smi.GoTo((StateMachine.BaseState) this.cringe);
  }

  public new class Instance : GameStateMachine<CringeMonitor, CringeMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    private StatusItem statusItem;

    public Instance(IStateMachineTarget master)
      : base(master)
    {
    }

    public void SetCringeSourceData(object data) => this.statusItem = new StatusItem("CringeSource", (string) data, (string) null, "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID);

    public StatusItem GetStatusItem() => this.statusItem;
  }
}
