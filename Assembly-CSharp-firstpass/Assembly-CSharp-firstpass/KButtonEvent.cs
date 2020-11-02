// Decompiled with JetBrains decompiler
// Type: KButtonEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

public class KButtonEvent : KInputEvent
{
  private bool[] mIsAction;

  public KButtonEvent(KInputController controller, InputEventType event_type, bool[] is_action)
    : base(controller, event_type)
    => this.mIsAction = is_action;

  public bool TryConsume(Action action)
  {
    if (this.Consumed)
      Debug.LogError((object) (action.ToString() + " was already consumed"));
    if (action != Action.NumActions && this.mIsAction[(int) action])
      this.Consumed = true;
    return this.Consumed;
  }

  public bool IsAction(Action action) => this.mIsAction[(int) action];

  public Action GetAction()
  {
    for (int index = 0; index < this.mIsAction.Length; ++index)
    {
      if (this.mIsAction[index])
        return (Action) index;
    }
    return Action.NumActions;
  }
}
