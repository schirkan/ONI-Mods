// Decompiled with JetBrains decompiler
// Type: CellEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class CellEvent : EventBase
{
  public string reason;
  public bool isSend;
  public bool enableLogging;

  public CellEvent(string id, string reason, bool is_send, bool enable_logging = true)
    : base(id)
  {
    this.reason = reason;
    this.isSend = is_send;
    this.enableLogging = enable_logging;
  }

  public string GetMessagePrefix() => this.isSend ? ">>>: " : "<<<: ";
}
