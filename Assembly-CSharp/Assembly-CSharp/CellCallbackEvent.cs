// Decompiled with JetBrains decompiler
// Type: CellCallbackEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Diagnostics;

public class CellCallbackEvent : CellEvent
{
  public CellCallbackEvent(string id, bool is_send, bool enable_logging = true)
    : base(id, "Callback", is_send, enable_logging)
  {
  }

  [Conditional("ENABLE_CELL_EVENT_LOGGER")]
  public void Log(int cell, int callback_id)
  {
    if (!this.enableLogging)
      return;
    CellEventInstance ev = new CellEventInstance(cell, callback_id, 0, (CellEvent) this);
    CellEventLogger.Instance.Add(ev);
  }

  public override string GetDescription(EventInstanceBase ev)
  {
    CellEventInstance cellEventInstance = ev as CellEventInstance;
    return this.GetMessagePrefix() + "Callback=" + cellEventInstance.data.ToString();
  }
}
