// Decompiled with JetBrains decompiler
// Type: KInputEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

public class KInputEvent
{
  public KInputController Controller { get; private set; }

  public InputEventType Type { get; private set; }

  public bool Consumed { get; set; }

  public KInputEvent(KInputController controller, InputEventType event_type)
  {
    this.Controller = controller;
    this.Type = event_type;
    this.Consumed = false;
  }
}
