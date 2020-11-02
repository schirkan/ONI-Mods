// Decompiled with JetBrains decompiler
// Type: StateEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public abstract class StateEvent
{
  protected string name;
  private string debugName;

  public StateEvent(string name)
  {
    this.name = name;
    this.debugName = "(Event)" + name;
  }

  public virtual StateEvent.Context Subscribe(StateMachine.Instance smi) => new StateEvent.Context(this);

  public virtual void Unsubscribe(StateMachine.Instance smi, StateEvent.Context context)
  {
  }

  public string GetName() => this.name;

  public string GetDebugName() => this.debugName;

  public struct Context
  {
    public StateEvent stateEvent;
    public int data;

    public Context(StateEvent state_event)
    {
      this.stateEvent = state_event;
      this.data = 0;
    }
  }
}
