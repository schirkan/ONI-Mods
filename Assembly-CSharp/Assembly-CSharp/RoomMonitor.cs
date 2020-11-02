// Decompiled with JetBrains decompiler
// Type: RoomMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class RoomMonitor : GameStateMachine<RoomMonitor, RoomMonitor.Instance>
{
  public Room currentRoom;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.root.EventHandler(GameHashes.PathAdvanced, new StateMachine<RoomMonitor, RoomMonitor.Instance, IStateMachineTarget, object>.State.Callback(RoomMonitor.UpdateRoomType));
  }

  private static void UpdateRoomType(RoomMonitor.Instance smi)
  {
    Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(smi.master.gameObject);
    if (roomOfGameObject == smi.sm.currentRoom)
      return;
    smi.sm.currentRoom = roomOfGameObject;
    roomOfGameObject?.cavity.OnEnter((object) smi.master.gameObject);
  }

  public new class Instance : GameStateMachine<RoomMonitor, RoomMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Navigator navigator;

    public Instance(IStateMachineTarget master)
      : base(master)
      => this.navigator = this.GetComponent<Navigator>();

    public Room GetCurrentRoomType() => this.sm.currentRoom;
  }
}
