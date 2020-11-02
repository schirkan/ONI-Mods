// Decompiled with JetBrains decompiler
// Type: EventInstanceBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

[SerializationConfig(MemberSerialization.OptIn)]
public class EventInstanceBase : ISaveLoadable
{
  [Serialize]
  public int frame;
  [Serialize]
  public int eventHash;
  public EventBase ev;

  public EventInstanceBase(EventBase ev)
  {
    this.frame = GameClock.Instance.GetFrame();
    this.eventHash = ev.hash;
    this.ev = ev;
  }

  public override string ToString()
  {
    string str = "[" + this.frame.ToString() + "] ";
    return this.ev != null ? str + this.ev.GetDescription(this) : str + "Unknown event";
  }
}
